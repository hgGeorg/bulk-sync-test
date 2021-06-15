﻿// <auto-generated />
using System;
using BulkSyncTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BulkSyncTest.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20210615140255_ComplexTypes")]
    partial class ComplexTypes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BulkSyncTest.Base", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SomeBaseProperty")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Inheritance");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Base");
                });

            modelBuilder.Entity("BulkSyncTest.Complex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("ProfileTemplates");
                });

            modelBuilder.Entity("BulkSyncTest.EntityWithMultipleDotsInTableName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("Entity.With.Multiple.Dots.In.Table.Name");
                });

            modelBuilder.Entity("BulkSyncTest.Referenced", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("Referenced");
                });

            modelBuilder.Entity("BulkSyncTest.InheritA", b =>
                {
                    b.HasBaseType("BulkSyncTest.Base");

                    b.Property<string>("SomeOtherPropA")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("InheritA");
                });

            modelBuilder.Entity("BulkSyncTest.InheritB", b =>
                {
                    b.HasBaseType("BulkSyncTest.Base");

                    b.Property<string>("SomeOtherPropB")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("InheritB");
                });

            modelBuilder.Entity("BulkSyncTest.InheritEmpty", b =>
                {
                    b.HasBaseType("BulkSyncTest.Base");

                    b.HasDiscriminator().HasValue("InheritEmpty");
                });

            modelBuilder.Entity("BulkSyncTest.Complex", b =>
                {
                    b.OwnsOne("BulkSyncTest.OwnedA", "OwnedA", b1 =>
                        {
                            b1.Property<int>("ComplexId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int?>("ReferencedId")
                                .HasColumnType("int");

                            b1.HasKey("ComplexId");

                            b1.HasIndex("ReferencedId");

                            b1.ToTable("ProfileTemplates");

                            b1.WithOwner()
                                .HasForeignKey("ComplexId");

                            b1.HasOne("BulkSyncTest.Referenced", "Referenced")
                                .WithMany()
                                .HasForeignKey("ReferencedId")
                                .OnDelete(DeleteBehavior.Restrict);

                            b1.Navigation("Referenced");
                        });

                    b.OwnsOne("BulkSyncTest.OwnedB", "OwnedB", b1 =>
                        {
                            b1.Property<int>("ComplexId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Content")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ComplexId");

                            b1.ToTable("ProfileTemplates");

                            b1.WithOwner()
                                .HasForeignKey("ComplexId");
                        });

                    b.Navigation("OwnedA")
                        .IsRequired();

                    b.Navigation("OwnedB")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
