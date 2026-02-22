using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Models;

namespace ToyotaWeb.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            context.Database.EnsureCreated();

            // ===================== CARS =====================
            if (!context.Cars.Any())
            {
                var cars = new List<Car>
                {
                    new Car { Name="Vios", Category="Sedan", BodyType="Sedan", Seats=5, FuelType="Xăng", Origin="Việt Nam", Description="Sedan hạng B", IsActive=true },
                    new Car { Name="Camry", Category="Sedan", BodyType="Sedan", Seats=5, FuelType="Hybrid", Origin="Thái Lan", Description="Sedan cao cấp", IsActive=true },
                    new Car { Name="Corolla Altis", Category="Sedan", BodyType="Sedan", Seats=5, FuelType="Hybrid", Origin="Thái Lan", Description="Sedan hạng C", IsActive=true },
                    new Car { Name="Corolla Cross", Category="SUV", BodyType="SUV", Seats=5, FuelType="Hybrid", Origin="Thái Lan", Description="SUV đô thị", IsActive=true },
                    new Car { Name="Fortuner", Category="SUV", BodyType="SUV", Seats=7, FuelType="Dầu", Origin="Indonesia", Description="SUV 7 chỗ", IsActive=true },
                    new Car { Name="Land Cruiser", Category="SUV", BodyType="SUV", Seats=7, FuelType="Dầu", Origin="Nhật Bản", Description="SUV cao cấp", IsActive=true },
                    new Car { Name="Alphard", Category="MPV", BodyType="MPV", Seats=7, FuelType="Hybrid", Origin="Nhật Bản", Description="MPV hạng sang", IsActive=true }
                };

                context.Cars.AddRange(cars);
                context.SaveChanges();
            }

            // ===================== VARIANTS =====================
            if (!context.CarVariants.Any())
            {
                var vios = context.Cars.First(c => c.Name == "Vios");
                var camry = context.Cars.First(c => c.Name == "Camry");
                var landCruiser = context.Cars.First(c => c.Name == "Land Cruiser");
                var alphard = context.Cars.First(c => c.Name == "Alphard");

                var variants = new List<CarVariant>
                {
                    new CarVariant { CarId=vios.CarId, VariantName="1.5E MT", Engine="1.5L", Transmission="MT", DriveType="FWD", Price=458000000 },
                    new CarVariant { CarId=vios.CarId, VariantName="1.5G CVT", Engine="1.5L", Transmission="CVT", DriveType="FWD", Price=545000000 },

                    new CarVariant { CarId=camry.CarId, VariantName="2.0Q", Engine="2.0L", Transmission="AT", DriveType="FWD", Price=1220000000 },
                    new CarVariant { CarId=landCruiser.CarId, VariantName="LC300", Engine="3.5 V6", Transmission="AT", DriveType="4WD", Price=4580000000 },

                    new CarVariant { CarId=alphard.CarId, VariantName="Luxury", Engine="Hybrid 2.5L", Transmission="AT", DriveType="FWD", Price=4370000000 }
                };

                context.CarVariants.AddRange(variants);
                context.SaveChanges();
            }

            // ===================== IMAGES =====================
            if (!context.CarImages.Any())
            {
                var variants = context.CarVariants.ToList();

                var images = new List<CarImage>
                {
                    new CarImage { VariantId = variants.First(v => v.VariantName=="1.5E MT").VariantId, ImageUrl="/images/cars/vios/vios1.jpg" },
                    new CarImage { VariantId = variants.First(v => v.VariantName=="1.5E MT").VariantId, ImageUrl="/images/cars/vios/vios2.jpg" },

                    new CarImage { VariantId = variants.First(v => v.VariantName=="2.0Q").VariantId, ImageUrl="/images/cars/camry/camry1.jpg" },
                    new CarImage { VariantId = variants.First(v => v.VariantName=="2.0Q").VariantId, ImageUrl="/images/cars/camry/camry2.jpg" },

                    new CarImage { VariantId = variants.First(v => v.VariantName=="LC300").VariantId, ImageUrl="/images/cars/lc300/lc1.jpg" },

                    new CarImage { VariantId = variants.First(v => v.VariantName=="Luxury").VariantId, ImageUrl="/images/cars/alphard/alpha1.jpg" }
                };

                context.CarImages.AddRange(images);
                context.SaveChanges();
            }
        }
    }
}