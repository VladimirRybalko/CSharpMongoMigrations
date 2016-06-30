using System.Reflection;

namespace CSharpMongoMigrations.Demo
{
    public class Runner
    {
        public static void Main(string[] args)
        {            
            var runner = new MigrationRunner("localhost:27017", "TestMigrations", Assembly.GetExecutingAssembly().CodeBase);
            runner.Up();
        }
    }
}
