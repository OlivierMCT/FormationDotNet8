using CATodos.Entities;
using CATodos.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CATodos.Tests {
    public class CATodoDbContextUnitTests {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void CreateDatabaseSchema() {
            DbContextOptionsBuilder<CATodoDbContext> builder = new();
            builder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CATodosTestCreateDatabaseSchema");

            CATodoDbContext context = new(builder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public void UpdateBaseEntityDateTime() {
            // Arrange
            DateTime avant = DateTime.Now;
            TodoEntity todo = new TodoEntity() { Title = "",  };
            DbContextOptionsBuilder<CATodoDbContext> builder = new();
            builder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CATodosTestUpdateBaseEntityDateTime");
            CATodoDbContext context = new(builder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Add(todo);

            // Act
            context.SaveChanges();

            // Assert
            context = new(builder.Options);
            DateTime apres = context.Todos.Find(todo.Id)!.CreatedDate;
            Assert.That(apres, Is.GreaterThanOrEqualTo(avant));
        }
    }
}