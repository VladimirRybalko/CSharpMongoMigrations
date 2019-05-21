using System;
using System.IO;

namespace CSharpMongoMigrations.Tests
{
    public static class AutoFixture
    {       
        private static Random rand = new Random();

        public static string String()
        {
            return Path.GetRandomFileName();
        }

        public static int Int()
        {
            return rand.Next();
        }

        public static long Long()
        {
            return (long)rand.Next();
        }
    }
}
