﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;

namespace Octogram.Chats.Infrastructure.Migrations.Migrations
{
    [DbContext(typeof(RepositoryDbContext))]
    partial class RepositoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Messenger.Domain.Chats.Chat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnName("CreateDate")
                        .HasColumnType("timestamptz");

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.ToTable("Chats");

                    b.HasDiscriminator<string>("Type").HasValue("Chat");
                });

            modelBuilder.Entity("Messenger.Domain.Messages.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ChatId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnName("Content")
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.Property<DateTimeOffset>("SentDate")
                        .HasColumnName("SentDate")
                        .HasColumnType("timestamptz");

                    b.Property<string>("State")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("State")
                        .HasColumnType("text")
                        .HasDefaultValue("Sending");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Octogram.Chats.Domain.Members.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnName("Email")
                        .HasColumnType("character varying(320)")
                        .HasMaxLength(320);

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Username")
                        .HasColumnName("Username")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("UsernameId")
                        .HasColumnName("UsernameId")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Messenger.Domain.Chats.DirectChat", b =>
                {
                    b.HasBaseType("Messenger.Domain.Chats.Chat");

                    b.Property<Guid?>("MemberId")
                        .HasColumnType("uuid");

                    b.HasIndex("MemberId")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("DirectChat");
                });

            modelBuilder.Entity("Messenger.Domain.Chats.Chat", b =>
                {
                    b.HasOne("Octogram.Chats.Domain.Members.Account", "Owner")
                        .WithOne()
                        .HasForeignKey("Messenger.Domain.Chats.Chat", "OwnerId");
                });

            modelBuilder.Entity("Messenger.Domain.Messages.Message", b =>
                {
                    b.HasOne("Messenger.Domain.Chats.Chat", "Chat")
                        .WithMany()
                        .HasForeignKey("ChatId");
                });

            modelBuilder.Entity("Messenger.Domain.Chats.DirectChat", b =>
                {
                    b.HasOne("Octogram.Chats.Domain.Members.Account", "Member")
                        .WithOne()
                        .HasForeignKey("Messenger.Domain.Chats.DirectChat", "MemberId");
                });
#pragma warning restore 612, 618
        }
    }
}
