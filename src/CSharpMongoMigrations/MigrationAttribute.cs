using System;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Mark specified class as migration. Use for discovering migrations in assembly.
    /// </summary>
    /// <remarks>You must set attribute for each migration class because it uses to discover migrations in assembly</remarks>
    public class MigrationAttribute : Attribute
    {
        public MigrationAttribute(long version, string description)
        {
            Version = version;
            Description = description;
        }

        /// <summary>
        /// Migration version
        /// </summary>
        public long Version { get; private set; }

        /// <summary>
        /// Migration description
        /// </summary>
        public string Description { get; private set; }
    }
}
