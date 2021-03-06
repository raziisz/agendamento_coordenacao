﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using agendamento_coordenacao.Data;

namespace agendamento_coordenacao.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200602154655_Initial2")]
    partial class Initial2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("backend.Models.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("varchar(1024)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Local")
                        .IsRequired()
                        .HasColumnName("local")
                        .HasColumnType("varchar(2028)");

                    b.Property<sbyte>("Reschedule")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("reschedule")
                        .HasColumnType("tinyint")
                        .HasDefaultValue((sbyte)0);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasColumnType("varchar(250)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("schedules");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Schedule");
                });

            modelBuilder.Entity("backend.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnName("lastname")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(100)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnName("password_hash")
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnName("password_salt")
                        .HasColumnType("longblob");

                    b.Property<string>("TypeUser")
                        .IsRequired()
                        .HasColumnName("type_user")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("backend.Models.Project", b =>
                {
                    b.HasBaseType("backend.Models.Schedule");

                    b.Property<DateTime>("EndProject")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartProject")
                        .HasColumnType("datetime(6)");

                    b.HasDiscriminator().HasValue("Project");
                });

            modelBuilder.Entity("backend.Models.Reunion", b =>
                {
                    b.HasBaseType("backend.Models.Schedule");

                    b.Property<DateTime>("DateReunion")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("HourEnd")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("HourStart")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasDiscriminator().HasValue("Reunion");
                });

            modelBuilder.Entity("backend.Models.Work", b =>
                {
                    b.HasBaseType("backend.Models.Schedule");

                    b.Property<DateTime>("DateWork")
                        .HasColumnType("datetime(6)");

                    b.HasDiscriminator().HasValue("Work");
                });

            modelBuilder.Entity("backend.Models.Schedule", b =>
                {
                    b.HasOne("backend.Models.User", "User")
                        .WithMany("Schedules")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
