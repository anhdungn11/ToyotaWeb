using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Models;

namespace ToyotaWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TestDrive> TestDrives { get; set; }
        public DbSet<Consult> Consults { get; set; }
        public DbSet<Contact> Contacts { get; set; }
                public DbSet<Car> Cars { get; set; }
        public DbSet<CarVariant> CarVariants { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CarVariant>()
                .Property(v => v.Price)
                .HasPrecision(18, 2); // chuẩn tiền tệ
        }
    }
}
