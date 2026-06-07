using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Models;

namespace ToyotaWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SOSRequest> SOSRequests { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerCar> CustomerCars { get; set; }

        public DbSet<CustomerInteraction> CustomerInteractions { get; set; }

        public DbSet<SaleOrder> SaleOrders { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
        public DbSet<CompanyExpense> CompanyExpenses { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<SaleKPI> SaleKPIs { get; set; }
        public DbSet<Inventory> Inventories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // =========================
            // PRICE DECIMAL
            // =========================
            builder.Entity<CarVariant>()
                .Property(v => v.Price)
                .HasPrecision(18, 2);

            builder.Entity<Inventory>()
                .Property(i => i.ImportPrice)
                .HasPrecision(18, 2);

            builder.Entity<Inventory>()
                .Property(i => i.SalePrice)
                .HasPrecision(18, 2);

            // =========================
            // SALE ORDER RELATION
            // =========================
            builder.Entity<SaleOrder>()
                .HasOne(x => x.Sale)
                .WithMany()
                .HasForeignKey(x => x.SaleId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}