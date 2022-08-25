using FluentMigrator.Runner.VersionTableInfo;

namespace FluentMigrator.Infra.Metadata
{
    public class FluentMigratorCustomMetadata : IVersionTableMetaData
    {
        public FluentMigratorCustomMetadata()
        {
            ApplicationContext = "GPRO";
        }

        public object ApplicationContext { get; set; }
        public bool OwnsSchema => true;
        public string SchemaName => "public";
        public string TableName => "migrations";
        public string ColumnName => "version";
        public string DescriptionColumnName => "description";
        public string UniqueIndexName => "uc_version";
        public string AppliedOnColumnName => "applied_on";

        public string TablePath => $"{SchemaName}.{TableName}";
    }
}
