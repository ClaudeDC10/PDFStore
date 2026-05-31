using Microsoft.EntityFrameworkCore;
using PDFStore.Core.Domain.Entities;

namespace PDFStore.Infrastructure
{
    public class DocumentContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<DocumentItem> DocumentItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentItem>()
                .HasIndex(i => i.ContentHash)
                .IsUnique();
        }
    }
}