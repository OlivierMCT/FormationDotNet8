
using CATodos.BusinessModels;

namespace CATodos.Business {
    public interface ICATodoService {
        IEnumerable<Todo> GetAllTodos();
        IEnumerable<Todo> SearchTodos(string title);
        IEnumerable<Todo> GetTodosByCategory(int categoryId);
        Todo GetTodo(int id);

        IEnumerable<Category> GetAllCategories();
        Category GetCategory(int id);

        void RemoveTodo(int id);
        Todo ToggleTodo(int id);
        Todo CreateTodo(TodoCreate infos);
        Todo UpdateTodo(TodoUpdate infos);

        Todo UpdateCategoriesForTodo(IEnumerable<int> categoryIds);



        Task<IEnumerable<Todo>> GetAllTodosAsync();
        Task<IEnumerable<Todo>> SearchTodosAsync(string title);
        Task<IEnumerable<Todo>> GetTodosByCategoryAsync(int categoryId);
        Task<Todo> GetTodoAsync(int id);

        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryAsync(int id);

        Task RemoveTodoAsync(int id);
        Task<Todo> ToggleTodoAsync(int id);
        Task<Todo> CreateTodoAsync(TodoCreate infos);
        Task<Todo> UpdateTodoAsync(TodoUpdate infos);

        Task<Todo> UpdateCategoriesForTodoAsync(IEnumerable<int> categoryIds);
    }
}
