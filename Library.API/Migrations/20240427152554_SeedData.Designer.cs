﻿// <auto-generated />
using System;
using Library.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Library.API.Migrations
{
    [DbContext(typeof(LibraryDbContext))]
    [Migration("20240427152554_SeedData")]
    partial class SeedData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Library.API.Entities.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("BirthDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("BirthPlace")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = new Guid("72d5b5f5-3008-49b7-b0d6-cc337f1a3330"),
                            BirthDate = new DateTimeOffset(new DateTime(1960, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Beijing",
                            Email = "author1@xxx.com",
                            Name = "Author 1"
                        },
                        new
                        {
                            Id = new Guid("7d04a48e-be4e-468e-8ce2-3ac0a0c79549"),
                            BirthDate = new DateTimeOffset(new DateTime(1976, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Hubei",
                            Email = "author2@xxx.com",
                            Name = "Author 2"
                        },
                        new
                        {
                            Id = new Guid("8406b13e-a793-4b12-84cb-7fe2a694b9aa"),
                            BirthDate = new DateTimeOffset(new DateTime(1973, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Hubei",
                            Email = "author3@xxx.com",
                            Name = "Author 3"
                        },
                        new
                        {
                            Id = new Guid("74556abd-1a6c-4d20-a8a7-271dd4393b2e"),
                            BirthDate = new DateTimeOffset(new DateTime(1978, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Shandong",
                            Email = "author4@xxx.com",
                            Name = "Author 4"
                        },
                        new
                        {
                            Id = new Guid("1029db57-c15c-4c0c-80a0-c811b7995cb4"),
                            BirthDate = new DateTimeOffset(new DateTime(1973, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Beijing",
                            Email = "author5@xxx.com",
                            Name = "Author 5"
                        },
                        new
                        {
                            Id = new Guid("0f978cf6-df6d-47a9-8ef2-d2723cc29cc8"),
                            BirthDate = new DateTimeOffset(new DateTime(1981, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Beijing",
                            Email = "author6@xxx.com",
                            Name = "Author 6"
                        },
                        new
                        {
                            Id = new Guid("10ee3976-d672-4411-ae1c-3267baa940eb"),
                            BirthDate = new DateTimeOffset(new DateTime(1954, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Shandong",
                            Email = "author7@xxx.com",
                            Name = "Author 7"
                        },
                        new
                        {
                            Id = new Guid("2633a79c-9f4a-48d5-ae5a-70945fb8583c"),
                            BirthDate = new DateTimeOffset(new DateTime(1981, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Shandong",
                            Email = "author8@xxx.com",
                            Name = "Author 8"
                        });
                });

            modelBuilder.Entity("Library.API.Entities.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Pages")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7d8ebda9-2634-4c0f-9469-0695d6132153"),
                            AuthorId = new Guid("72d5b5f5-3008-49b7-b0d6-cc337f1a3330"),
                            Description = "Description of Book 1",
                            Pages = 281,
                            Title = "Book 1"
                        },
                        new
                        {
                            Id = new Guid("1ed47697-aa7d-48c2-aa39-305d0e13b3aa"),
                            AuthorId = new Guid("72d5b5f5-3008-49b7-b0d6-cc337f1a3330"),
                            Description = "Description of Book 2",
                            Pages = 370,
                            Title = "Book 2"
                        },
                        new
                        {
                            Id = new Guid("5f82c852-375d-4926-a3b7-84b63fc1bfae"),
                            AuthorId = new Guid("7d04a48e-be4e-468e-8ce2-3ac0a0c79549"),
                            Description = "Description of Book 3",
                            Pages = 229,
                            Title = "Book 3"
                        },
                        new
                        {
                            Id = new Guid("418a5b20-460b-4604-be17-2b0809e19acd"),
                            AuthorId = new Guid("7d04a48e-be4e-468e-8ce2-3ac0a0c79549"),
                            Description = "Description of Book 4",
                            Pages = 440,
                            Title = "Book 4"
                        });
                });

            modelBuilder.Entity("Library.API.Entities.Book", b =>
                {
                    b.HasOne("Library.API.Entities.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Library.API.Entities.Author", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
