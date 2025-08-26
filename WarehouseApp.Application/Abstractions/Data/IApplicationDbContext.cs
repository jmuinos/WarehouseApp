using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WarehouseApp.SharedKernel.Core.Primitives;
using WarehouseApp.SharedKernel.Core.Primitives.Maybe;

namespace WarehouseApp.Application.Abstractions.Data;

/// <summary>Represents the abstraction over the application database context.</summary>
public interface IApplicationDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : Entity;

    Task<Maybe<TEntity>> FindByIdAsync<TEntity>(Guid id, CancellationToken cancellationToken = default) where TEntity : Entity;

    void Add<TEntity>(TEntity entity) where TEntity : Entity;

    void AddRange<TEntity>(IReadOnlyCollection<TEntity> entities) where TEntity : Entity;

    void Remove<TEntity>(TEntity entity) where TEntity : Entity;

    Task<int> ExecuteSqlAsync(string sql, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken = default);
}