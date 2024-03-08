using CATodos.Entities;
using Microsoft.EntityFrameworkCore;

namespace CATodos.Persistance {
    public class CATodoDbContext : DbContext {
        public DbSet<TodoEntity> Todos { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }

        public CATodoDbContext(DbContextOptions<CATodoDbContext> options) : base(options) {
            SavingChanges += (s, e) => ManageBaseEntityDateTime();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<CategoryEntity>(e => {
                e.Property(c => c.Label).HasMaxLength(200);
                e.HasIndex(c => c.Label).IsUnique();
            });

            modelBuilder.Entity<TodoEntity>(e => {
                e.Property(t => t.Title).HasMaxLength(200);
                e
                    .HasMany(t => t.Categories)
                    .WithMany(c => c.Todos)
                    .UsingEntity("TodosCategories");
            });
        }

        private void ManageBaseEntityDateTime() {
            ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State is EntityState.Added or EntityState.Modified)
                .ToList()
                .ForEach(e => {
                    if(e.State is EntityState.Added) {
                        e.Entity.CreatedDate = DateTime.Now;
                    }
                    else if(e.State is EntityState.Modified) {
                        e.Entity.UpdatedDate = DateTime.Now;
                    }
                });
        }
    }
}
