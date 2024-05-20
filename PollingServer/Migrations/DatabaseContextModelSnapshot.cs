﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PollingServer.Models;

#nullable disable

namespace PollingServer.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PollingServer.Models.Image.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Bytes")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Answer.BaseAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<int?>("PollAnswersId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PollAnswersId");

                    b.ToTable("Answers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BaseAnswer");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Answer.PollAnswers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AnswerTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PollId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PollId");

                    b.HasIndex("UserId");

                    b.ToTable("PollsAnswers");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Poll", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Access")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int?>("ImageId")
                        .HasColumnType("int");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Polls");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.PollAllowedUsers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("PollId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PollId");

                    b.HasIndex("UserId");

                    b.ToTable("PollAllowedUsers");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Question.BaseQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("FieldName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("OrderRate")
                        .HasColumnType("int");

                    b.Property<int?>("PollId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PollId");

                    b.ToTable("Questions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BaseQuestion");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("PollingServer.Models.User.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<DateTime>("RegistrationTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PollingServer.Models.User.UserBan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndTime")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserBan");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Answer.SelectAnswer", b =>
                {
                    b.HasBaseType("PollingServer.Models.Poll.Answer.BaseAnswer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("SelectAnswer");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Answer.TextFieldAnswer", b =>
                {
                    b.HasBaseType("PollingServer.Models.Poll.Answer.BaseAnswer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Answers", t =>
                        {
                            t.Property("Text")
                                .HasColumnName("TextFieldAnswer_Text");
                        });

                    b.HasDiscriminator().HasValue("TextFieldAnswer");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Question.SelectQuestion", b =>
                {
                    b.HasBaseType("PollingServer.Models.Poll.Question.BaseQuestion");

                    b.Property<string>("DefaultValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Options")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("SelectQuestion");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Question.TextFieldQuestion", b =>
                {
                    b.HasBaseType("PollingServer.Models.Poll.Question.BaseQuestion");

                    b.Property<string>("FieldPlaceholder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("TextFieldQuestion");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Answer.BaseAnswer", b =>
                {
                    b.HasOne("PollingServer.Models.Poll.Answer.PollAnswers", null)
                        .WithMany("BaseAnswers")
                        .HasForeignKey("PollAnswersId");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Answer.PollAnswers", b =>
                {
                    b.HasOne("PollingServer.Models.Poll.Poll", null)
                        .WithMany("Answers")
                        .HasForeignKey("PollId");

                    b.HasOne("PollingServer.Models.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Poll", b =>
                {
                    b.HasOne("PollingServer.Models.Image.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.HasOne("PollingServer.Models.User.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.PollAllowedUsers", b =>
                {
                    b.HasOne("PollingServer.Models.Poll.Poll", null)
                        .WithMany("AllowedUsers")
                        .HasForeignKey("PollId");

                    b.HasOne("PollingServer.Models.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Question.BaseQuestion", b =>
                {
                    b.HasOne("PollingServer.Models.Poll.Poll", null)
                        .WithMany("Questions")
                        .HasForeignKey("PollId");
                });

            modelBuilder.Entity("PollingServer.Models.User.UserBan", b =>
                {
                    b.HasOne("PollingServer.Models.User.User", null)
                        .WithMany("Bans")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Answer.PollAnswers", b =>
                {
                    b.Navigation("BaseAnswers");
                });

            modelBuilder.Entity("PollingServer.Models.Poll.Poll", b =>
                {
                    b.Navigation("AllowedUsers");

                    b.Navigation("Answers");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("PollingServer.Models.User.User", b =>
                {
                    b.Navigation("Bans");
                });
#pragma warning restore 612, 618
        }
    }
}
