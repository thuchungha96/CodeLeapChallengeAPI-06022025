﻿// <auto-generated />
using CodeLeapChallengeAPI_06022025.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CodeLeapChallengeAPI_06022025.Migrations
{
    [DbContext(typeof(CodeDBContext))]
    [Migration("20250206152524_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseSerialColumns(modelBuilder);

            modelBuilder.Entity("CodeLeapChallengeAPI_06022025.Data.Class.UserInfor", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.Property<int?>("AccountType")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserName");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
