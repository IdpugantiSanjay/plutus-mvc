﻿// <auto-generated />
using System;
using MVC.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MVC.Migrations
{
    [DbContext(typeof(PlutusDbContext))]
    [Migration("20221004110620_AddNewCategory2")]
    partial class AddNewCategory2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MVC.Categories.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Salary"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Refund"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Food Delivery"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Online Shopping"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Restaurant"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Mutual Funds"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Groceries"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Interest"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Internet"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Prepaid Phone Plan"
                        });
                });

            modelBuilder.Entity("MVC.Transactions.Transaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("MVC.Transactions.Transaction", b =>
                {
                    b.HasOne("MVC.Categories.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}