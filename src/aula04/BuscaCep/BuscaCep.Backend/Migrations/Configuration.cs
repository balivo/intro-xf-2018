using System.Data.Entity.Migrations;

namespace BuscaCep.Backend.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<BuscaCep.Backend.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BuscaCep.Backend.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
