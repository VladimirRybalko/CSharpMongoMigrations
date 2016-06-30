using System;

namespace CSharpMongoMigrations
{
    public class MigrationAttribute : Attribute
    {
        public MigrationAttribute(long version, string description)
        {
            Version = version;
            Description = description;
        }

        public long Version { get; private set; }
        public string Description { get; private set; }
    }
}
