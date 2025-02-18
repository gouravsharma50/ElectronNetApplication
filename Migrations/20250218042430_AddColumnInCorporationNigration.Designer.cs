﻿// <auto-generated />
using System;
using DesktopApplication.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DesktopApplication.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250218042430_AddColumnInCorporationNigration")]
    partial class AddColumnInCorporationNigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("DesktopApplication.Models.Branch", b =>
                {
                    b.Property<int>("BranchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("BranchCreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CorporationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSync")
                        .HasColumnType("INTEGER");

                    b.HasKey("BranchId");

                    b.HasIndex("CorporationId");

                    b.HasIndex("CreatedByUserId")
                        .IsUnique();

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("DesktopApplication.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BranchId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CorporationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsSync")
                        .HasColumnType("INTEGER");

                    b.HasKey("CategoryId");

                    b.HasIndex("BranchId");

                    b.HasIndex("CorporationId");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DesktopApplication.Models.Corporation", b =>
                {
                    b.Property<int>("CorporationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CorporationCreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("CorporationName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsSync")
                        .HasColumnType("INTEGER");

                    b.HasKey("CorporationId");

                    b.ToTable("Corporations");
                });

            modelBuilder.Entity("DesktopApplication.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BranchId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CorporationId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsSync")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.HasIndex("BranchId");

                    b.HasIndex("CorporationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DesktopApplication.Models.Branch", b =>
                {
                    b.HasOne("DesktopApplication.Models.Corporation", "Corporation")
                        .WithMany("Branches")
                        .HasForeignKey("CorporationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DesktopApplication.Models.User", "User")
                        .WithOne()
                        .HasForeignKey("DesktopApplication.Models.Branch", "CreatedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Corporation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DesktopApplication.Models.Category", b =>
                {
                    b.HasOne("DesktopApplication.Models.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DesktopApplication.Models.Corporation", "Corporation")
                        .WithMany()
                        .HasForeignKey("CorporationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DesktopApplication.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Corporation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DesktopApplication.Models.User", b =>
                {
                    b.HasOne("DesktopApplication.Models.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DesktopApplication.Models.Corporation", "Corporation")
                        .WithMany()
                        .HasForeignKey("CorporationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Corporation");
                });

            modelBuilder.Entity("DesktopApplication.Models.Corporation", b =>
                {
                    b.Navigation("Branches");
                });
#pragma warning restore 612, 618
        }
    }
}
