using FluentMigrator.Infra.Metadata;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FluentMigrator.Infra.Middlewares
{
    public static class FluentMigratorMiddleware
    {
        public static void AddFluentMigrator(this IServiceCollection services, string connectionString, params Assembly[] migrationAssemblies)
        {
            services.AddFluentMigratorCore()
                    .ConfigureRunner(rb =>
                        rb.AddPostgres11_0()
                          .WithVersionTable(new FluentMigratorCustomMetadata())
                          .WithGlobalConnectionString(connectionString)
                          .ScanIn(migrationAssemblies).For.Migrations());
        }
    }
}
