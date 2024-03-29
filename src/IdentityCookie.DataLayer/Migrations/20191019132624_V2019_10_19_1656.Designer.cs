﻿// <auto-generated />
using System;
using IdentityCookie.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IdentityCookie.DataLayer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20191019132624_V2019_10_19_1656")]
    partial class V2019_10_19_1656
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IdentityCookie.DomainClasses.AppLogItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByBrowserName")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("CreatedByIp")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("LogLevel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logger")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedByBrowserName")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("ModifiedByIp")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("StateJson")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppLogItems");
                });

            modelBuilder.Entity("IdentityCookie.DomainClasses.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByBrowserName")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("CreatedByIp")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedByBrowserName")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("ModifiedByIp")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("IdentityCookie.DomainClasses.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByBrowserName")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("CreatedByIp")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LastLoggedIn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedByBrowserName")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("ModifiedByIp")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("IdentityCookie.DomainClasses.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByBrowserName")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("CreatedByIp")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedByBrowserName")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("ModifiedByIp")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("IdentityCookie.DomainClasses.UserRole", b =>
                {
                    b.HasOne("IdentityCookie.DomainClasses.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IdentityCookie.DomainClasses.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
