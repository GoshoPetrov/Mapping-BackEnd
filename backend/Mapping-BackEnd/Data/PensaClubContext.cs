using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Mapping_BackEnd.Data;

public partial class PensaClubContext : DbContext
{
	public PensaClubContext()
	{
	}

	public PensaClubContext(DbContextOptions<PensaClubContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Place> Places { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (!string.IsNullOrEmpty(connectionString))
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        else
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PensaClub;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Place>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Places__3214EC071FE8A1FA");

			entity.Property(e => e.Id).ValueGeneratedOnAdd();
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
