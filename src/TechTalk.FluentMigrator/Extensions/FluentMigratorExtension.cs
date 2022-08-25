using FluentMigrator.Infra.Metadata;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Serilog;
using System;

namespace FluentMigrator.Api.Extensions
{
    public static class FluentMigratorExtension
    {
        private static readonly object migrationLock = new();
        private static readonly FluentMigratorCustomMetadata metadata = new FluentMigratorCustomMetadata();

        public static IHost ApplyMigrations(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                //var removeDistributedLockPolicy = scope.ServiceProvider.GetRequiredService<IRemoveDistributedLockPolicy>();
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                var logger = scope.ServiceProvider.GetService<ILogger>();

                try
                {
                    lock (migrationLock)
                    {

                        if (runner.HasMigrationsToApplyUp())
                        {
                            InsertDatabaseDistributedLock(runner.Processor);
                            runner.MigrateUp();
                        }
                    }
                }
                catch (PostgresException pex)
                    when (pex.SqlState == PostgresErrorCodes.UniqueViolation)
                {
                    logger?.Error("Fluent Migrator try to duplicate a migration or distributed locked strategy was activated", pex);
                }
                catch (Exception ex)
                    when (ex.InnerException is PostgresException 
                            && ((PostgresException)ex.InnerException).SqlState == PostgresErrorCodes.UniqueViolation)
                {
                    logger?.Error("Fluent Migrator try to duplicate a migration or distributed locked strategy was activated", ex.InnerException);
                }
                finally
                {
                    RemoveDatabaseDistributedLock(runner.Processor);
                }
            }

            return host;
        }

        private static void InsertDatabaseDistributedLock(IMigrationProcessor processor)
        {
            processor.Execute(@$"INSERT INTO {metadata.TableName} ({metadata.ColumnName}, {metadata.AppliedOnColumnName}, {metadata.DescriptionColumnName})
                                                        VALUES (0, CURRENT_TIMESTAMP, 'Migrating Up...');");
        }

        private static void RemoveDatabaseDistributedLock(IMigrationProcessor processor)
        {
            //AsyncHelper.RunSync(() =>

                //removeDistributedLockPolicy.Policy.ExecuteAsync(() =>
                //{
                    processor.Execute($"DELETE FROM {metadata.TableName} WHERE {metadata.ColumnName} = 0;");
                    //return Task.FromResult(0);
                //})
            //);
        }
    }
}
