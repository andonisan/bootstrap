using DbUp;
using DbUp.Engine;
using Microsoft.Extensions.Logging;

namespace DbSeeder;


public sealed class DbMigrate(ILogger logger)
{
    public DatabaseUpgradeResult Migrate(string connectionString)
    {
        logger.LogInformation("Migrating database");

        string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "migrations");

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsFromFileSystem(scriptPath)
            .WithTransactionPerScript()
            .WithExecutionTimeout(TimeSpan.FromMinutes(5))
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (result.Successful)
        {
            logger.LogInformation("Database migration successful");
        }
        else
        {
            logger.LogError("Database migration failed");
            return result;
        }

        logger.LogInformation("Database migration completed");

        return result;
    }
}
