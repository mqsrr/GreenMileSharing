using GreenMileSharing.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenMileSharing.TripApi.Persistence.Configurations;

internal sealed class TripEntityConfiguration : IEntityTypeConfiguration<Trip>
{

    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.ToTable("Trips")
            .HasKey(trip => trip.Id);
        
        builder.Property(trip => trip.StartMileage)
            .IsRequired()
            .IsUnicode(false);
        
        builder.Property(trip => trip.EndMileage)
            .IsRequired()
            .IsUnicode(false);

        builder.HasIndex(trip => trip.Id)
            .IsUnique();
    }
}