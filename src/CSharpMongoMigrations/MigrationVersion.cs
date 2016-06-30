using System;

namespace CSharpMongoMigrations
{
    public class MigrationVersion
    {
        public Guid Id { get; } = Guid.NewGuid();

        public long Version { get; private set; }

        public string Description { get; private set;}
        
        public MigrationVersion()
            :this(long.MinValue) { }

        public MigrationVersion(long version)
            : this(version, null) { }

        public MigrationVersion(long version, string description)
        {
            Version = version; ;
            Description = description;
        }

        public override string ToString()
        {
            return $"[{Version}] {Description}";
        }
    }
}
