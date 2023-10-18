﻿// <auto-generated />
using Forum2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Forum2.Migrations.ForumDb
{
    [DbContext(typeof(ForumDbContext))]
    [Migration("20231006094636_ForumThreadCreatorId")]
    partial class ForumThreadCreatorId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("Forum2.Models.ForumCategory", b =>
                {
                    b.Property<int>("ForumCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ForumCategoryDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ForumCategoryName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ForumCategoryId");

                    b.ToTable("ForumCategory");
                });

            modelBuilder.Entity("Forum2.Models.ForumThread", b =>
                {
                    b.Property<int>("ForumThreadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ForumCategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ForumThreadCreatorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ForumThreadTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ForumThreadId");

                    b.HasIndex("ForumCategoryId");

                    b.ToTable("ForumThread");
                });

            modelBuilder.Entity("Forum2.Models.ForumThread", b =>
                {
                    b.HasOne("Forum2.Models.ForumCategory", "ForumCategory")
                        .WithOne("ForumThread")
                        .HasForeignKey("Forum2.Models.ForumThread", "ForumCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ForumCategory");
                });

            modelBuilder.Entity("Forum2.Models.ForumCategory", b =>
                {
                    b.Navigation("ForumThread");
                });
#pragma warning restore 612, 618
        }
    }
}