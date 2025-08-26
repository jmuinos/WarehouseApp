using System.Reflection;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using WarehouseApp.Application.Abstractions.Data;
using WarehouseApp.Application.Abstractions.Date;
using WarehouseApp.Persistence.Extensions;
using WarehouseApp.SharedKernel.Core.Abstractions;
using WarehouseApp.SharedKernel.Core.Events;
using WarehouseApp.SharedKernel.Core.Primitives;
using WarehouseApp.SharedKernel.Core.Primitives.Maybe;

namespace WarehouseApp.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTime dateTime, IMediator mediator)
    : DbContext(options), IApplicationDbContext, IUnitOfWork
{
    public new DbSet<TEntity> Set<TEntity>() where TEntity : Entity => base.Set<TEntity>();

    public async Task<Maybe<TEntity>> FindByIdAsync<TEntity>(Guid id, CancellationToken cancellationToken = default) where TEntity : Entity
    {
        var entity = await Set<TEntity>().FindAsync([id], cancellationToken);
        return entity is null ? Maybe<TEntity>.None : Maybe<TEntity>.From(entity);
    }

    public new void Add<TEntity>(TEntity entity) where TEntity : Entity => Set<TEntity>().Add(entity);

    public void AddRange<TEntity>(IReadOnlyCollection<TEntity> entities) where TEntity : Entity => Set<TEntity>().AddRange(entities);

    public new void Remove<TEntity>(TEntity entity) where TEntity : Entity => Set<TEntity>().Remove(entity);

    /// <inheritdoc />
    public Task<int> ExecuteSqlAsync(string sql, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken = default)
        => Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);

    /// <summary>Saves all the pending changes in the unit of work.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of entities that have been saved.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = dateTime.UtcNow;
        UpdateAuditableEntities(utcNow);
        UpdateSoftDeletableEntities(utcNow);

        await PublishDomainEvents(cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => Database.BeginTransactionAsync(cancellationToken);

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyUtcDateTimeConverter();

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>Updates the entities implementing <see cref="IAuditableEntity"/> interface.</summary>
    /// <param name="utcNow">The current date and time in UTC format.</param>
    private void UpdateAuditableEntities(DateTime utcNow)
    {
        foreach (EntityEntry<IAuditableEntity> entityEntry in ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(nameof(IAuditableEntity.CreatedOnUtc)).CurrentValue = utcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(nameof(IAuditableEntity.ModifiedOnUtc)).CurrentValue = utcNow;
            }
        }
    }

    /// <summary>Updates the entities implementing <see cref="ISoftDeletableEntity"/> interface.</summary>
    /// <param name="utcNow">The current date and time in UTC format.</param>
    private void UpdateSoftDeletableEntities(DateTime utcNow)
    {
        foreach (EntityEntry<ISoftDeletableEntity> entityEntry in ChangeTracker.Entries<ISoftDeletableEntity>())
        {
            if (entityEntry.State != EntityState.Deleted)
            {
                continue;
            }

            entityEntry.Property(nameof(ISoftDeletableEntity.DeletedOnUtc)).CurrentValue = utcNow;
            entityEntry.Property(nameof(ISoftDeletableEntity.Deleted)).CurrentValue = true;
            entityEntry.State = EntityState.Modified;

            UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
        }
    }

    /// <summary>
    /// Updates the specified entity entry's referenced entries in the deleted state to the modified state.
    /// This method is recursive.
    /// </summary>
    /// <param name="entityEntry">The entity entry.</param>
    private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
    {
        if (!entityEntry.References.Any())
        {
            return;
        }

        foreach (var targetEntry in entityEntry.References
                                               .Where(r => r.TargetEntry is { State: EntityState.Deleted })
                                               .Select(r => r.TargetEntry!)) 
        {
            targetEntry.State = EntityState.Unchanged;
            UpdateDeletedEntityEntryReferencesToUnchanged(targetEntry);
        }
    }

    /// <summary>Publishes and then clears all the domain events that exist within the current transaction.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task PublishDomainEvents(CancellationToken cancellationToken)
    {
        List<EntityEntry<AggregateRoot>> aggregateRoots = ChangeTracker
                                                          .Entries<AggregateRoot>()
                                                          .Where(entityEntry => entityEntry.Entity.DomainEvents.Count != 0)
                                                          .ToList();

        List<IDomainEvent> domainEvents = aggregateRoots.SelectMany(entityEntry => entityEntry.Entity.DomainEvents).ToList();
        aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());
        IEnumerable<Task> tasks = domainEvents.Select(domainEvent => mediator.Publish(domainEvent, cancellationToken));

        await Task.WhenAll(tasks);
    }
}