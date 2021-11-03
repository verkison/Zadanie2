using Models;

namespace Zadanie.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            context.CustomerStatus.AddOrUpdate(
                c => c.Status,
                new Models.RefModels.CustomerStatus { Status = "Potencjalny" },
                new Models.RefModels.CustomerStatus { Status = "Obecny" }
            ); context.SaveChanges();

            context.Customers.AddOrUpdate(
                new Customer { FirstName = "Jan", LastName = "Kowalski", Address = "Ulicowa 10/1", StatusID = 1 }
            ); context.SaveChanges();
        }
    }
}