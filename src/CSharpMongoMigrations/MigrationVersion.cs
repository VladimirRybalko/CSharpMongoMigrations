using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Migration version
    /// </summary>
    public class MigrationVersion
    {
        /// <summary>
        /// Get a minimum migration version for the specified collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static MigrationVersion Min(string collection) => new MigrationVersion(collection, -1);

        /// <summary>
        /// Get a maximum migration version for the specified collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static MigrationVersion Max(string collection) => new MigrationVersion(collection, long.MaxValue);

        /// <summary>
        /// The unique identifier of the current migration.
        /// </summary>
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Sequence number of the current migration.
        /// </summary>
        public long Version { get; private set; }

        /// <summary>
        /// Text description of the current migration.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The collection name the current migration applies for.
        /// </summary>
        public string Collection { get; private set; }

        public MigrationVersion(long version)
            : this(null, version) { }

        public MigrationVersion(string collection, long version)
            : this(collection, version, null) { }

        public MigrationVersion(string collection, long version, string description)
        {
            Collection = collection;
            Version = version;
            Description = description;
        }

        public override string ToString()
        {
            return $"[{Collection}: {Version}] {Description}";
        }
    }
}