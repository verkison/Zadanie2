using System.Data.Entity;
using Models.RefModels;

namespace Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("name=DBContext") { }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerStatus> CustomerStatus { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
    }
}