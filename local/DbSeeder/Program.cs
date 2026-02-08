using DbSeeder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Todos.Infrastructure.Persistence;

namespace DbSeeder;

internal class Program
{
    internal static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("TodoAppDb") ?? throw new ArgumentException("Connection string not found");

        builder.Services.AddSingleton<DbMigrate>();

        var app = builder.Build();

        var migrator = app.Services.GetRequiredService<DbMigrate>();

        migrator.Migrate(connectionString);
    }
}