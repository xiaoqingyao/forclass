﻿// <auto-generated />
using System;
using CoursePlatform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoursePlatform.Data.Migrations
{
    [DbContext(typeof(CPDbContext))]
    [Migration("20201109094811_quoteds_name1")]
    partial class quoteds_name1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoursePlatform.Data.Entities.CourseEntity", b =>
                {
                    b.Property<int>("IndentityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CatalogId")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CatalogName")
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("CoverUrl")
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatorCode")
                        .HasColumnType("int");

                    b.Property<string>("CreatorName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Deleted")
                        .HasColumnType("int");

                    b.Property<string>("Goal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Intro")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("RegionCode")
                        .HasColumnType("int");

                    b.Property<string>("RegionName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SchoolCode")
                        .HasColumnType("int");

                    b.Property<string>("SchoolName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SignatureId")
                        .HasColumnType("int");

                    b.Property<string>("SignatureName")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("IndentityId");

                    b.HasIndex("Deleted");

                    b.HasIndex("ID");

                    b.ToTable("B_Course");
                });

            modelBuilder.Entity("CoursePlatform.Data.Entities.PlatformUserEntity", b =>
                {
                    b.Property<int>("IndentityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseShelves")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Deleted")
                        .HasColumnType("int");

                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SchoolId")
                        .HasColumnType("int");

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.Property<int>("StdJoined")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("IndentityId");

                    b.HasIndex("Deleted");

                    b.HasIndex("ID");

                    b.HasIndex("UserId");

                    b.ToTable("U_PlatformUser");
                });

            modelBuilder.Entity("CoursePlatform.Data.Entities.QuoteDsEntity", b =>
                {
                    b.Property<int>("IndentityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CatalogId")
                        .HasColumnType("int");

                    b.Property<string>("CourseId")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Deleted")
                        .HasColumnType("int");

                    b.Property<Guid>("DsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DsName")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<int>("OperatorId")
                        .HasColumnType("int");

                    b.Property<int>("SortVal")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("IndentityId");

                    b.HasIndex("CatalogId");

                    b.HasIndex("CourseId");

                    b.HasIndex("Deleted");

                    b.HasIndex("ID");

                    b.HasIndex("OperatorId");

                    b.HasIndex("SortVal");

                    b.ToTable("B_Course_DS");
                });

            modelBuilder.Entity("CoursePlatform.Data.Entities.TagsEntity", b =>
                {
                    b.Property<int>("IndentityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssetId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("CourseId")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Creator")
                        .HasColumnType("int");

                    b.Property<int>("Deleted")
                        .HasColumnType("int");

                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RegtionId")
                        .HasColumnType("int");

                    b.Property<int>("SchoolId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TypeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("IndentityId");

                    b.HasIndex("Deleted");

                    b.HasIndex("ID");

                    b.ToTable("B_Course_Tags");
                });
#pragma warning restore 612, 618
        }
    }
}