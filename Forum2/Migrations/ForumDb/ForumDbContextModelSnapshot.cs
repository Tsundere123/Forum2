﻿// <auto-generated />
using System;
using Forum2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Forum2.Migrations.ForumDb
{
    [DbContext(typeof(ForumDbContext))]
    partial class ForumDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

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

            modelBuilder.Entity("Forum2.Models.ForumPost", b =>
                {
                    b.Property<int>("ForumPostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ForumPostContent")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ForumPostCreationTimeUnix")
                        .HasColumnType("TEXT");

                    b.Property<string>("ForumPostCreatorId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ForumThreadId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ForumPostId");

                    b.HasIndex("ForumThreadId");

                    b.ToTable("ForumPost");
                });

            modelBuilder.Entity("Forum2.Models.ForumThread", b =>
                {
                    b.Property<int>("ForumThreadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ForumCategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ForumThreadCreationTimeUnix")
                        .HasColumnType("TEXT");

                    b.Property<string>("ForumThreadCreatorId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("ForumThreadIsSoftDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ForumThreadTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ForumThreadId");

                    b.HasIndex("ForumCategoryId");

                    b.ToTable("ForumThread");
                });

            modelBuilder.Entity("Forum2.Models.ForumPost", b =>
                {
                    b.HasOne("Forum2.Models.ForumThread", "ForumThread")
                        .WithMany("ForumPosts")
                        .HasForeignKey("ForumThreadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ForumThread");
                });

            modelBuilder.Entity("Forum2.Models.ForumThread", b =>
                {
                    b.HasOne("Forum2.Models.ForumCategory", "ForumCategory")
                        .WithMany("ForumThreads")
                        .HasForeignKey("ForumCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ForumCategory");
                });

            modelBuilder.Entity("Forum2.Models.ForumCategory", b =>
                {
                    b.Navigation("ForumThreads");
                });

            modelBuilder.Entity("Forum2.Models.ForumThread", b =>
                {
                    b.Navigation("ForumPosts");
                });
#pragma warning restore 612, 618
        }
    }
}
