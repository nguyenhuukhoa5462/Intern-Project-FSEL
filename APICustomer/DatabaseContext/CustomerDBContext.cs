using APICustomer.Configuration;
using APICustomer.Models;
using Microsoft.EntityFrameworkCore;

namespace APICustomer.DatabaseContext
{
    public class CustomerDBContext : DbContext
    {
        public CustomerDBContext()
        {

        }
        public CustomerDBContext(DbContextOptions<CustomerDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder.UseSqlServer("Server=DESKTOP-T4L1DE8\\SQLEXPRESS;Database=Fsel-Customer;Trusted_Connection=True;"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new CustomerConfig());
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
