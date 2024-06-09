﻿// <auto-generated />
using System;
using GreenMileSharing.TripApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GreenMileSharing.TripApi.Persistence.Migrations
{
    [DbContext(typeof(TripDbContext))]
    partial class TripDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GreenMileSharing.Domain.Car", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CarBrand")
                        .IsRequired()
                        .HasMaxLength(125)
                        .IsUnicode(false)
                        .HasColumnType("character varying(125)");

                    b.Property<int>("CurrentMileage")
                        .IsUnicode(false)
                        .HasColumnType("integer");

                    b.Property<int>("EndOfLifeMileage")
                        .IsUnicode(false)
                        .HasColumnType("integer");

                    b.Property<string>("LicensePlateNumber")
                        .IsRequired()
                        .HasMaxLength(125)
                        .IsUnicode(false)
                        .HasColumnType("character varying(125)");

                    b.Property<int>("MaintenanceInterval")
                        .HasColumnType("integer");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(125)
                        .IsUnicode(false)
                        .HasColumnType("character varying(125)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("LicensePlateNumber")
                        .IsUnique();

                    b.ToTable("Cars", (string)null);
                });

            modelBuilder.Entity("GreenMileSharing.Domain.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("GreenMileSharing.Domain.Trip", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CarId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<int>("EndMileage")
                        .IsUnicode(false)
                        .HasColumnType("integer");

                    b.Property<int>("StartMileage")
                        .IsUnicode(false)
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Trips", (string)null);
                });

            modelBuilder.Entity("GreenMileSharing.Domain.Trip", b =>
                {
                    b.HasOne("GreenMileSharing.Domain.Car", "Car")
                        .WithMany("Trips")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GreenMileSharing.Domain.Employee", "Employee")
                        .WithMany("Trips")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("GreenMileSharing.Domain.Car", b =>
                {
                    b.Navigation("Trips");
                });

            modelBuilder.Entity("GreenMileSharing.Domain.Employee", b =>
                {
                    b.Navigation("Trips");
                });
#pragma warning restore 612, 618
        }
    }
}
