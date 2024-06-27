﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyBackend.DbContext;

#nullable disable

namespace MyBackend.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20240424121857_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("MyBackend.Entities.Gm", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Gms");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Name = "Gm1"
                        },
                        new
                        {
                            Id = "2",
                            Name = "Gm2"
                        });
                });

            modelBuilder.Entity("MyBackend.Entities.Player", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Players");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Name = "Player1"
                        },
                        new
                        {
                            Id = "2",
                            Name = "Player2"
                        },
                        new
                        {
                            Id = "3",
                            Name = "Player3"
                        },
                        new
                        {
                            Id = "4",
                            Name = "Player4"
                        });
                });

            modelBuilder.Entity("MyBackend.Entities.Session", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("GmId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxNumberOfPlayers")
                        .HasColumnType("INTEGER");

                    b.Property<string>("System")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GmId");

                    b.ToTable("Sessions");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Description = "Description",
                            GmId = "1",
                            MaxNumberOfPlayers = 4,
                            System = "d&d",
                            Tags = "tag1, tag2, tag3",
                            Title = "title1"
                        },
                        new
                        {
                            Id = "2",
                            Description = "Description",
                            GmId = "1",
                            MaxNumberOfPlayers = 4,
                            System = "d&d",
                            Tags = "tag1, tag2, tag3",
                            Title = "title2"
                        },
                        new
                        {
                            Id = "3",
                            Description = "Description",
                            GmId = "2",
                            MaxNumberOfPlayers = 4,
                            System = "d&d",
                            Tags = "tag1, tag2, tag3",
                            Title = "title3"
                        },
                        new
                        {
                            Id = "4",
                            Description = "Description",
                            GmId = "2",
                            MaxNumberOfPlayers = 6,
                            System = "Veasen",
                            Tags = "tag2, tag3",
                            Title = "title4"
                        });
                });

            modelBuilder.Entity("PlayerSession", b =>
                {
                    b.Property<string>("PlayersId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SessionsId")
                        .HasColumnType("TEXT");

                    b.HasKey("PlayersId", "SessionsId");

                    b.HasIndex("SessionsId");

                    b.ToTable("PlayerSession");

                    b.HasData(
                        new
                        {
                            PlayersId = "1",
                            SessionsId = "1"
                        },
                        new
                        {
                            PlayersId = "2",
                            SessionsId = "1"
                        },
                        new
                        {
                            PlayersId = "2",
                            SessionsId = "2"
                        },
                        new
                        {
                            PlayersId = "3",
                            SessionsId = "2"
                        },
                        new
                        {
                            PlayersId = "3",
                            SessionsId = "3"
                        },
                        new
                        {
                            PlayersId = "4",
                            SessionsId = "3"
                        },
                        new
                        {
                            PlayersId = "1",
                            SessionsId = "4"
                        },
                        new
                        {
                            PlayersId = "2",
                            SessionsId = "4"
                        },
                        new
                        {
                            PlayersId = "3",
                            SessionsId = "4"
                        },
                        new
                        {
                            PlayersId = "4",
                            SessionsId = "4"
                        });
                });

            modelBuilder.Entity("MyBackend.Entities.Session", b =>
                {
                    b.HasOne("MyBackend.Entities.Gm", "Gm")
                        .WithMany("Sessions")
                        .HasForeignKey("GmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gm");
                });

            modelBuilder.Entity("PlayerSession", b =>
                {
                    b.HasOne("MyBackend.Entities.Player", null)
                        .WithMany()
                        .HasForeignKey("PlayersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBackend.Entities.Session", null)
                        .WithMany()
                        .HasForeignKey("SessionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyBackend.Entities.Gm", b =>
                {
                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}