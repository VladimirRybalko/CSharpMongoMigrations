using CSharpMongoMigrations.Demo.Migrations;
using System;

namespace CSharpMongoMigrations.Demo
{
    public class CustomMigrationFactory : IMigrationFactory
    {
        // Define condition.
        private bool _stopMigration = true;

        public IMigration Create(Type type)
        {
            // Put custom condition as per the requirement
            if(type == typeof(ConditionalMigrations) && _stopMigration)
            {
                return default;
            }

            return (IMigration)Activator.CreateInstance(type);
        }
    }
}