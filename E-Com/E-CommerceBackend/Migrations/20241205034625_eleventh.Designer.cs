﻿// <auto-generated />
using System;
using E_CommerceBackend.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace E_CommerceBackend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241205034625_eleventh")]
    partial class eleventh
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("E_CommerceBackend.Entities.Cardtable", b =>
                {
                    b.Property<int>("CardtableId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CardtableId"));

                    b.Property<int>("CVV")
                        .HasMaxLength(3)
                        .HasColumnType("int");

                    b.Property<long>("CardNumber")
                        .HasMaxLength(16)
                        .HasColumnType("bigint");

                    b.Property<string>("ExpirtDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CardtableId");

                    b.ToTable("Cardtables");
                });

            modelBuilder.Entity("E_CommerceBackend.Entities.CartMastertable", b =>
                {
                    b.Property<int>("CartMasterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartMasterId"));

                    b.Property<int>("UsertableId")
                        .HasColumnType("int");

                    b.HasKey("CartMasterId");

                    b.ToTable("CartMastertables");
                });

            modelBuilder.Entity("E_CommerceBackend.Entities.Cartdetailtable", b =>
                {
                    b.Property<int>("CartdetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartdetailId"));

                    b.Property<int>("CartMasterId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("CartdetailId");

                    b.ToTable("Cartdetailtables");
                });

            modelBuilder.Entity("E_CommerceBackend.Entities.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountryId"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("phonecode")
                        .HasColumnType("int");

                    b.Property<string>("shortname")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("CountryId");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("E_CommerceBackend.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PurchaseDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("PurchasePrice")
                        .HasColumnType("real");

                    b.Property<float>("SellingPrice")
                        .HasColumnType("real");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("E_CommerceBackend.Entities.SalesDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<float>("SellingPrice")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("SalesDetails");
                });

            modelBuilder.Entity("E_CommerceBackend.Entities.SalesMaster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DeliveryAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeliveryCountryId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeliveryStateId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeliveryZipcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("TotalPrice")
                        .HasColumnType("real");

                    b.Property<int>("UsertableId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SalesMasters");
                });

            modelBuilder.Entity("E_CommerceBackend.Entities.State", b =>
                {
                    b.Property<int>("StateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StateId"));

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("statename")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StateId");

                    b.HasIndex("CountryId");

                    b.ToTable("State");
                });

            modelBuilder.Entity("E_CommerceBackend.Entities.StoreOtp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("StoreOtp");
                });

            modelBuilder.Entity("E_CommerceBackend.Entities.Usertable", b =>
                {
                    b.Property<int>("UsertableId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsertableId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("CountryId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateOfBirth")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Profileimage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StateId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsertypeId")
                        .HasColumnType("int");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.HasKey("UsertableId");

                    b.ToTable("Usertable");
                });

            modelBuilder.Entity("E_CommerceBackend.Entities.Usertype", b =>
                {
                    b.Property<int>("typeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("typeId"));

                    b.Property<string>("usertype")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("typeId");

                    b.ToTable("Usertype");
                });

            modelBuilder.Entity("E_CommerceBackend.Entities.State", b =>
                {
                    b.HasOne("E_CommerceBackend.Entities.Country", "Country")
                        .WithMany("States")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("E_CommerceBackend.Entities.Country", b =>
                {
                    b.Navigation("States");
                });
#pragma warning restore 612, 618
        }
    }
}
