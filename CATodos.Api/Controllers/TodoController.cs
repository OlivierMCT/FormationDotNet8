using CATodos.Api.Dtos;
using CATodos.Api.Filters;
using CATodos.Business;
using CATodos.BusinessModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CATodos.Api.Controllers {
    [ApiController, Route("todo"), CATodoExceptionFilter]
    public class TodoController(ICATodoService todoService) : ControllerBase {
        [HttpGet, ChronoActionFilter]
        public async Task<TodosDto> GetAllAsync([FromQuery(Name = "q")] string? keyword = null) {
            return ToTodos(await ((!string.IsNullOrEmpty(keyword)) ? todoService.SearchTodosAsync(keyword) : todoService.GetAllTodosAsync()));
        }

        [HttpGet, Route("{id:int}", Name = "TodoController_GetOne")]
        public async Task<TodoDetailDto> GetOneAsync(int id) {
            return ToDetailDto(await todoService.GetTodoAsync(id));
        }

        [HttpDelete, Route("{id:int}"), CAAuthorizationFilter(Applications = "Sandra;OTIC")]
        public async Task<IActionResult> DeleteOneAsync(int id) {
            await todoService.RemoveTodoAsync(id);
            return NoContent();
        }

        [HttpPost, Route(""), CAAuthorizationFilter]
        [ProducesResponseType(201), ProducesResponseType(400)]
        public async Task<IActionResult> PostOneAsync(TodoPostDto dto) {
            var newTodo = await todoService.CreateTodoAsync(new TodoCreate() {
                DueDate = DateOnly.FromDateTime(dto.DueDate!.Value),
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Title = dto.Title
            });
            return CreatedAtRoute("TodoController_GetOne", new { id = newTodo.Id }, ToDetailDto(newTodo));
        }

        [HttpPut, Route("{id}"), CAAuthorizationFilter]
        public async Task<TodoDetailDto> PutOneAsync(int id, TodoPutDto dto) {
            await todoService.UpdateCategoriesForTodoAsync(id, dto.Categories ?? new List<int>());
            var updatedTodo = await todoService.UpdateTodoAsync(new TodoUpdate() {
                DueDate = DateOnly.FromDateTime(dto.DueDate!.Value),
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Title = dto.Title,
                Id = id
            });
            return ToDetailDto(updatedTodo);
        }

        [HttpPatch, Route("{id}"), CAAuthorizationFilter]
        public async Task<TodoDetailDto> PatchOneAsync(int id, TodoPatchDto dto) {
            var updatedTodo = await todoService.GetTodoAsync(id);
            if (updatedTodo.IsDone != dto.Done!.Value)
                updatedTodo = await todoService.ToggleTodoAsync(id);
            return ToDetailDto(updatedTodo);
        }

        #region Automapper
        internal static TodosDto ToTodos(IEnumerable<Todo> todos) {
            return new TodosDto() {
                Categories = todos.SelectMany(x => x.Categories).Distinct().Select(CategoryController.ToDto).ToList(),
                Todos = todos.Select(TodoController.ToDto).ToList()
            };
        }

        internal static TodoDto ToDto(Todo todo) {
            return new TodoDto() {
                Categories = todo.Categories.Select(x => x.Id).ToList(),
                Done = todo.IsDone,
                DueDate = todo.DueDate,
                Id = todo.Id,
                Title = todo.Title
            };
        }

        internal static TodoDetailDto ToDetailDto(Todo todo) {
            return new TodoDetailDto() {
                Categories = todo.Categories.Select(CategoryController.ToDto).ToList(),
                Done = todo.IsDone,
                DueDate = todo.DueDate,
                Id = todo.Id,
                Title = todo.Title,
                Deletable = todo.IsDeletable,
                Latitude = todo.Coordinate?.Latitude,
                Longitude = todo.Coordinate?.Longitude,
                Status = todo.Status.ToString()
            };
        }
        #endregion
    }
}
