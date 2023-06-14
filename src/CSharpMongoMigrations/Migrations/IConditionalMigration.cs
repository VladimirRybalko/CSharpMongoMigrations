using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpMongoMigrations.Migrations
{
    /// <summary>
    /// Define condition checks
    /// </summary>
    public interface IConditionalMigration
    {
        /// <summary>
        /// Evaluate any custom condition to apply migration.
        /// </summary>
        bool ShouldUp();

        /// <summary>
        /// Evaluate any custom condition to revert migration.
        /// </summary>
        bool ShouldDown();
    }
}
