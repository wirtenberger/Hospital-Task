﻿// <auto-generated />
using System;
using Hospital.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hospital.DataAccess.Migrations
{
    [DbContext(typeof(HospitalDbContext))]
    [Migration("20240903151802_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Hospital.Core.Model.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.ToTable("Districts", t =>
                        {
                            t.HasCheckConstraint("Districts_Number_NonNegative", "Number > 0");
                        });

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Number = 1
                        },
                        new
                        {
                            Id = 2,
                            Number = 2
                        },
                        new
                        {
                            Id = 3,
                            Number = 3
                        },
                        new
                        {
                            Id = 4,
                            Number = 4
                        },
                        new
                        {
                            Id = 5,
                            Number = 5
                        });
                });

            modelBuilder.Entity("Hospital.Core.Model.Doctor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("DistrictId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Patronymic")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<int>("SpecialityId")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.HasIndex("RoomId");

                    b.HasIndex("SpecialityId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("Hospital.Core.Model.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Patronymic")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Hospital.Core.Model.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.ToTable("Rooms", t =>
                        {
                            t.HasCheckConstraint("Rooms_Number_NonNegative", "Number > 0");
                        });

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Number = 1
                        },
                        new
                        {
                            Id = 2,
                            Number = 2
                        },
                        new
                        {
                            Id = 3,
                            Number = 3
                        },
                        new
                        {
                            Id = 4,
                            Number = 4
                        },
                        new
                        {
                            Id = 5,
                            Number = 5
                        });
                });

            modelBuilder.Entity("Hospital.Core.Model.Speciality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Specialities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Speciality 1"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Speciality 2"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Speciality 3"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Speciality 4"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Speciality 5"
                        });
                });

            modelBuilder.Entity("Hospital.Core.Model.Doctor", b =>
                {
                    b.HasOne("Hospital.Core.Model.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Hospital.Core.Model.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Hospital.Core.Model.Speciality", "Speciality")
                        .WithMany()
                        .HasForeignKey("SpecialityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("District");

                    b.Navigation("Room");

                    b.Navigation("Speciality");
                });

            modelBuilder.Entity("Hospital.Core.Model.Patient", b =>
                {
                    b.HasOne("Hospital.Core.Model.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("District");
                });
#pragma warning restore 612, 618
        }
    }
}
