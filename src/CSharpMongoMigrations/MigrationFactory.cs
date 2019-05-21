using System;

namespace CSharpMongoMigrations
{
    public sealed class MigrationFactory : IMigrationFactory
    {
        public IMigration Create(Type type)
        {
            return (IMigration)Activator.CreateInstance(type);
        }
    }

    public interface IMigrationFactory
    {
        IMigration Create(Type type);
    }
}
