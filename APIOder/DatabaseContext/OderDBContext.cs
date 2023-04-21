using APIOder.Configuration;
using APIOder.Models;
using Microsoft.EntityFrameworkCore;

namespace APIOder.DatabaseContext
{
    public class OderDBContext : DbContext
    {
        public OderDBContext()
        {

        }
        public OderDBContext(DbContextOptions<OderDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder.UseSqlServer("Server=DESKTOP-T4L1DE8\\SQLEXPRESS;Database=Fsel-Oder;Trusted_Connection=True;"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new OderConfig());
            modelBuilder.ApplyConfiguration(new OderDetailConfig());
        }
        public DbSet<Oder> Oders { get; set; }
        public DbSet<OderDetail> OderDetails { get; set; }
    }
}
