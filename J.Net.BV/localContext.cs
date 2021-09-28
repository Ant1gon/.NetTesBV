using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace J.Net.BV
{
    public partial class localContext : DbContext
    {
        public localContext()
        {
        }


        public localContext(DbContextOptions<localContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<DocumentsStatuses> DocumentsStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=local;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Documents>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).IsUnicode(false);
            });
            modelBuilder.Entity<Documents>().HasIndex(d => d.Id);

            modelBuilder.Entity<DocumentsStatuses>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");
            });
            modelBuilder.Entity<DocumentsStatuses>().HasIndex(d => d.Id);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
