using System.Reflection;

namespace CSharpMongoMigrations.Demo
{
    public class Runner
    {
        public static void Main(string[] args)
        {            
            var runner = new MigrationRunner("mongodb://localhost:27017/TestMigrations", Assembly.GetExecutingAssembly().FullName, new MigrationFactory());
            
            // Run all collection specific migrations
            runner.Up("Animals");

            // Run all existing migrations
            runner.Up();

            // Roll back all applied collection specific migrations
            runner.Down("Animals");

            // Roll back all applied migrations
            runner.Down();
        }
    }
}