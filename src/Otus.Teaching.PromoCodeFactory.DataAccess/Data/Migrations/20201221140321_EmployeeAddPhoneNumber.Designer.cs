﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20201221140321_EmployeeAddPhoneNumber")]
    partial class EmployeeAddPhoneNumber
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10");

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.Administration.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("AppliedPromocodesCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.Administration.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.CustomerPreference", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PreferenceId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.HasKey("CustomerId", "PreferenceId");

                    b.HasIndex("PreferenceId");

                    b.ToTable("CustomerPreference");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Preference", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Preferences");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.PromoCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("BeginDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PartnerManagerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("PartnerName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<Guid?>("PreferenceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ServiceInfo")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("PartnerManagerId");

                    b.HasIndex("PreferenceId");

                    b.ToTable("PromoCodes");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.Administration.Employee", b =>
                {
                    b.HasOne("Otus.Teaching.PromoCodeFactory.Core.Domain.Administration.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.CustomerPreference", b =>
                {
                    b.HasOne("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Customer", "Customer")
                        .WithMany("CustomerPreferences")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Preference", "Preference")
                        .WithMany("CustomerPreferences")
                        .HasForeignKey("PreferenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.PromoCode", b =>
                {
                    b.HasOne("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Customer", "Customer")
                        .WithMany("PromoCodes")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Otus.Teaching.PromoCodeFactory.Core.Domain.Administration.Employee", "PartnerManager")
                        .WithMany()
                        .HasForeignKey("PartnerManagerId");

                    b.HasOne("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Preference", "Preference")
                        .WithMany()
                        .HasForeignKey("PreferenceId");
                });
#pragma warning restore 612, 618
        }
    }
}
