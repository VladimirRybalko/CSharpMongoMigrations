namespace CSharpMongoMigrations
{
    public interface IMigration
    {
        void Up();

        void Down();
    }
}
