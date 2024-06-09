using GreenMileSharing.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenMileSharing.TripApi.Persistence.Configurations;

internal sealed class CarEntityTypeConfiguration : IEntityTypeConfiguration<Car>
{

    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars")
            .HasKey(car => car.Id);

        builder.Property(car => car.CarBrand)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(125);
        
        builder.Property(car => car.Model)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(125);
        
        builder.Property(car => car.LicensePlateNumber)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(125);

        builder.Property(car => car.CurrentMileage)
            .IsRequired()
            .IsUnicode(false);
        
        builder.Property(car => car.EndOfLifeMileage)
            .IsRequired()
            .IsUnicode(false);

        builder.HasMany(car => car.Trips)
            .WithOne()
            .HasForeignKey(trip => trip.CarId);
        
        builder.HasIndex(car => car.Id)
            .IsUnique();

        builder.HasIndex(car => car.LicensePlateNumber)
            .IsUnique();
    }
}