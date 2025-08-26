var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.WarehouseApp_API>("api");

await builder.Build().RunAsync();