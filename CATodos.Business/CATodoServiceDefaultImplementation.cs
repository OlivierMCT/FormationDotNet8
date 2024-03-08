
using CATodos.BusinessModels;
using CATodos.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CATodos.Business {
    public class CATodoServiceDefaultImplementation(CATodoDbContext context) : ICATodoService {
        
        public IEnumerable<Todo> GetAllTodos() {
            return context.Todos
                .Include(t => t.Categories)
                .Select(t => t.ToTodo())
                .ToList();
        }

        public IEnumerable<Todo> SearchTodos(string title) {
            return context.Todos
                .Include(t => t.Categories)
                .Where(t => t.Title.Contains(title))
                .Select(t => t.ToTodo())
                .ToList();
        }

        public IEnumerable<Todo> GetTodosByCategory(int categoryId) {
            return context.Categories
                .Include(c => c.Todos)
                .FirstOrDefault(c => c.Id == categoryId)
                ?.Todos.Select(t => t.ToTodo()) 
                ?? throw new CATodoException(102, $"la catégorie n°{categoryId} n'existe pas");
        }

        public Todo GetTodo(int id) {
            return context.Todos
                .Include(t => t.Categories)
                .FirstOrDefault(t => t.Id == id)
                ?.ToTodo()
                ?? throw new CATodoException(101, $"la tâche n°{id} n'existe pas");
        }

        public IEnumerable<Category> GetAllCategories() {
            return context.Categories
                .Include(c => c.Todos)
                .Select(c => c.ToCategory())
                .ToList();
        }

        public Category GetCategory(int id) {
            return context.Categories
                .FirstOrDefault(c => c.Id == id)
                ?.ToCategory()
                ?? throw new CATodoException(102, $"la catégorie n°{id} n'existe pas");
        }

        public void RemoveTodo(int id) {
            var todo = context.Todos.Find(id) ?? throw new CATodoException(101, $"la tâche n°{id} n'existe pas");
            if(!todo.IsDeletable()) throw new CATodoException(103, $"la tâche n°{id} n'est pas supprimable");
            context.Remove(todo);
            context.SaveChanges();
        }

        public Todo ToggleTodo(int id) {
            var todo = context.Todos.Find(id) ?? throw new CATodoException(101, $"la tâche n°{id} n'existe pas");
            todo.IsDone = !todo.IsDone;
            context.SaveChanges();
            return todo.ToTodo();
        }

        public Todo CreateTodo(TodoCreate infos) {
            // Vérifier les infos
            // Créer TodoEntity avec les infos
            // .Add && SaveChanges
            // transformer en Todo le TodoEntity que l'on vient d'insérer

            throw new NotImplementedException();
        }

        public Todo UpdateTodo(TodoUpdate infos) {
            throw new NotImplementedException();
        }

        public Todo UpdateCategoriesForTodo(IEnumerable<int> categoryIds) {
            throw new NotImplementedException();
        }

        #region Async Methods

        public Task<IEnumerable<Todo>> GetAllTodosAsync() => Task.FromResult(GetAllTodos());

        public Task<IEnumerable<Todo>> SearchTodosAsync(string title) => Task.FromResult(SearchTodos(title));

        public Task<IEnumerable<Todo>> GetTodosByCategoryAsync(int categoryId) => Task.FromResult(GetTodosByCategory(categoryId));

        public Task<Todo> GetTodoAsync(int id) => Task.FromResult(GetTodo(id));

        public Task<IEnumerable<Category>> GetAllCategoriesAsync() => Task.FromResult(GetAllCategories());

        public Task<Category> GetCategoryAsync(int id) => Task.FromResult(GetCategory(id));

        public Task RemoveTodoAsync(int id) => Task.Run(() => RemoveTodo(id));

        public Task<Todo> ToggleTodoAsync(int id) => Task.FromResult(ToggleTodo(id));

        public Task<Todo> CreateTodoAsync(TodoCreate infos) => Task.FromResult(CreateTodo(infos));

        public Task<Todo> UpdateTodoAsync(TodoUpdate infos) => Task.FromResult(UpdateTodo(infos));

        public Task<Todo> UpdateCategoriesForTodoAsync(IEnumerable<int> categoryIds) => Task.FromResult(UpdateCategoriesForTodo(categoryIds));

        #endregion
    }
}
