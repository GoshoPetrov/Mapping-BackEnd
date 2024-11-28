﻿// <auto-generated />
using System;
using Mapping_BackEnd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Mapping_BackEnd.Migrations
{
    [DbContext(typeof(PensaClubContext))]
    [Migration("20241128011154_mssql.local_migration_104")]
    partial class mssqllocal_migration_104
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Mapping_BackEnd.Data.Place", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Caption")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Lat")
                        .HasColumnType("float");

                    b.Property<double?>("Long")
                        .HasColumnType("float");

                    b.Property<double?>("Radius")
                        .HasColumnType("float");

                    b.HasKey("Id")
                        .HasName("PK__Places__3214EC071FE8A1FA");

                    b.ToTable("Places");
                });
#pragma warning restore 612, 618
        }
    }
}
