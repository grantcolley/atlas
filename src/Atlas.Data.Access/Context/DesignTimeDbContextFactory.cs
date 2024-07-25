using Atlas.Data.Access.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Atlas.Data.Access.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration
                = new ConfigurationBuilder().SetBasePath(
                    Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../Atlas.API/appsettings.Development.json").Build();

            DbContextOptionsBuilder<ApplicationDbContext> builder = new();

            string? connectionString = configuration.GetConnectionString(DataMigrations.CONNECTION_STRING) 
                ?? throw new NullReferenceException(nameof(DataMigrations.CONNECTION_STRING));

            if (connectionString.Contains(DataMigrations.SQLITE_DATABASE))
            {
                builder.UseSqlite(connectionString, x => x.MigrationsAssembly(DataMigrations.SQLITE_MIGRATIONS));
            }
            else
            {
                builder.UseSqlServer(connectionString, x => x.MigrationsAssembly(DataMigrations.SQLSERVER_MIGRATIONS));
            }

            return new ApplicationDbContext(builder.Options);
        }
    }
}
