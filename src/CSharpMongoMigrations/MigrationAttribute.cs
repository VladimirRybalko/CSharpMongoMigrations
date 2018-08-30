using System;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Mark specified class as migration. Use for discovering migrations in assembly.
    /// </summary>
    /// <remarks>You must set attribute for each migration class because it uses to discover migrations in assembly</remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class MigrationAttribute : Attribute
    {
        public MigrationAttribute(long version, string description, string collectionName)
        {
            Version = version;
            Description = description;
            CollectionName = collectionName;
        }

        /// <summary>
        /// Migration version
        /// </summary>
        public long Version { get; private set; }

        /// <summary>
        /// Migration description
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Collection affected by migration
        /// </summary>
        public string CollectionName { get; private set; }
    }
}
