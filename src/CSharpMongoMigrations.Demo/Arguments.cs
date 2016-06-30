using System;

namespace CSharpMongoMigrations.Demo
{
    public class Arguments
    {
        public string Server { get; }
        public string Database { get; }
        public string MigrationAssembly { get; }

        public Arguments(string[] args)
        {
            if (args.Length < 3)
                throw new ArgumentException("Migration runner required three arguments at least");

            Server = args[0];
            Database = args[1];
            MigrationAssembly = args[2];
        }
    }
}
