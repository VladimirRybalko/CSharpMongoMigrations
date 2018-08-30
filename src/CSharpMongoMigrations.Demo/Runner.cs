using System.Reflection;

namespace CSharpMongoMigrations.Demo
{
    public class Runner
    {
        public static void Main(string[] args)
        {            
            var storesRunner = new MigrationRunner("mongodb://localhost:27017", "TestMigrations", "Stores", Assembly.GetExecutingAssembly().GetName().Name);
            storesRunner.Up();

            var personsRunner = new MigrationRunner("mongodb://localhost:27017/TestMigrations", "Persons", Assembly.GetExecutingAssembly().GetName().Name);
            personsRunner.Up();
        }
    }
}
