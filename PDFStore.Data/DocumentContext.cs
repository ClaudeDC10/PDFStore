using Microsoft.EntityFrameworkCore;
using PDFStore.Core.Domain.Entities;

namespace PDFStore.Data
{
    public class DocumentContext : DbContext
    {
        public DbSet<DocumentItem> DocumentItems { get; set; }

        public DocumentContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentItem>()
                .HasIndex(i => i.ContentHash)
                .IsUnique();
        }
    }
}