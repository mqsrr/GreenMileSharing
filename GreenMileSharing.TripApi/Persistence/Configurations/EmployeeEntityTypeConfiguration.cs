using GreenMileSharing.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenMileSharing.TripApi.Persistence.Configurations;

internal sealed class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{

    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees")
            .HasKey(employee => employee.Id);

        builder.Property(employee => employee.Username)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(255);

        builder.HasIndex(employee => employee.Id)
            .IsUnique();

        builder.HasMany(employee => employee.Trips)
            .WithOne()
            .HasForeignKey(trip => trip.EmployeeId);
    }
}