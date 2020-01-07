using System;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Mark specified class as a migration. Used to discover migrations in the target assembly.
    /// </summary>
    public class MigrationAttribute : Attribute
    {
        public MigrationAttribute(long version, string description = null)
            : this(null, version, description)
        {
        }

        public MigrationAttribute(string collection, long version, string description = null)
        {
            Collection = collection;
            Version = version;
            Description = description;
        }

        /// <summary>
        /// The migration version.
        /// </summary>
        public long Version { get; private set; }

        /// <summary>
        /// The migration description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The target collection for migration.
        /// </summary>
        public string Collection { get; private set; }
    }
}