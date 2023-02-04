﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ThreaditAPI.Database;

#nullable disable

namespace ThreaditAPI.Migrations
{
    [DbContext(typeof(PostgresDbContext))]
    partial class PostgresDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ModeratorProfileSpool", b =>
                {
                    b.Property<string>("CreatedSpoolsId")
                        .HasColumnType("text");

                    b.Property<string>("ModeratorsId")
                        .HasColumnType("text");

                    b.HasKey("CreatedSpoolsId", "ModeratorsId");

                    b.HasIndex("ModeratorsId");

                    b.ToTable("ModeratorProfileSpool");
                });

            modelBuilder.Entity("ThreaditAPI.Models.Comment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Edited")
                        .HasColumnType("boolean");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ThreadId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ThreadId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ThreaditAPI.Models.Spool", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Interests")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserSettingsId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("UserSettingsId");

                    b.ToTable("Spools");
                });

            modelBuilder.Entity("ThreaditAPI.Models.Thread", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Rips")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SpoolId")
                        .HasColumnType("text");

                    b.Property<string>("Stitches")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Topic")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("SpoolId");

                    b.ToTable("Threads");
                });

            modelBuilder.Entity("ThreaditAPI.Models.UserProfile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserProfiles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("UserProfile");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("ThreaditAPI.Models.UserSession", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateExpires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("ThreaditAPI.Models.UserSettings", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("DarkMode")
                        .HasColumnType("boolean");

                    b.Property<string>("Interests")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProfilePicture")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Spins")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("ThreaditAPI.Models.ModeratorProfile", b =>
                {
                    b.HasBaseType("ThreaditAPI.Models.UserProfile");

                    b.HasDiscriminator().HasValue("ModeratorProfile");
                });

            modelBuilder.Entity("ThreaditAPI.Models.User", b =>
                {
                    b.HasBaseType("ThreaditAPI.Models.UserProfile");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("User");
                });

            modelBuilder.Entity("ModeratorProfileSpool", b =>
                {
                    b.HasOne("ThreaditAPI.Models.Spool", null)
                        .WithMany()
                        .HasForeignKey("CreatedSpoolsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThreaditAPI.Models.ModeratorProfile", null)
                        .WithMany()
                        .HasForeignKey("ModeratorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ThreaditAPI.Models.Comment", b =>
                {
                    b.HasOne("ThreaditAPI.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThreaditAPI.Models.Thread", null)
                        .WithMany("Comments")
                        .HasForeignKey("ThreadId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ThreaditAPI.Models.Spool", b =>
                {
                    b.HasOne("ThreaditAPI.Models.User", "Owner")
                        .WithMany("CreatedSpools")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThreaditAPI.Models.UserSettings", null)
                        .WithMany("SpoolsJoined")
                        .HasForeignKey("UserSettingsId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ThreaditAPI.Models.Thread", b =>
                {
                    b.HasOne("ThreaditAPI.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThreaditAPI.Models.Spool", null)
                        .WithMany("Threads")
                        .HasForeignKey("SpoolId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ThreaditAPI.Models.Spool", b =>
                {
                    b.Navigation("Threads");
                });

            modelBuilder.Entity("ThreaditAPI.Models.Thread", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("ThreaditAPI.Models.UserSettings", b =>
                {
                    b.Navigation("SpoolsJoined");
                });

            modelBuilder.Entity("ThreaditAPI.Models.User", b =>
                {
                    b.Navigation("CreatedSpools");
                });
#pragma warning restore 612, 618
        }
    }
}
