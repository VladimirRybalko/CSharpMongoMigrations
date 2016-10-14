namespace CSharpMongoMigrations
{
    /// <summary>
    /// Mongo migration
    /// </summary>
    public interface IMigration
    {
        /// <summary>
        /// Execute migration up
        /// </summary>
        void Up();

        /// <summary>
        /// Execute migration down
        /// </summary>
        void Down();
    }
}
