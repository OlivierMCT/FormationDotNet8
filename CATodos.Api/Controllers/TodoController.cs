using CATodos.Api.Dtos;
using CATodos.Business;
using CATodos.BusinessModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CATodos.Api.Controllers {
    [ApiController, Route("todo")]
    public class TodoController : ControllerBase {
        private readonly ICATodoService _todoService;

        public TodoController(ICATodoService todoService) {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<TodosDto> GetAllAsync([FromQuery(Name = "q")] string? keyword = null) {
            return ToTodos(await ((!string.IsNullOrEmpty(keyword)) ? _todoService.SearchTodosAsync(keyword) : _todoService.GetAllTodosAsync()));
        }

        [HttpGet, Route("{id:int}", Name = "TodoController_GetOne")]
        public async Task<ActionResult<TodoDetailDto>> GetOneAsync(int id) {
            try {
                return ToDetailDto(await _todoService.GetTodoAsync(id));
            } catch (CATodoException ex) when (ex.Code == 101) {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete, Route("{id:int}")]
        public async Task<IActionResult> DeleteOneAsync(int id) {
            try {
                await _todoService.RemoveTodoAsync(id);
                return NoContent();
            } catch (CATodoException ex) when (ex.Code == 101) { return NotFound(ex.Message); } catch (CATodoException ex) when (ex.Code == 103) { return BadRequest(ex.Message); }
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> PostOneAsync(TodoPostDto dto) {
            try {
                var newTodo = await _todoService.CreateTodoAsync(new TodoCreate() {
                    DueDate = DateOnly.FromDateTime(dto.DueDate!.Value),
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    Title = dto.Title
                });
                return CreatedAtRoute("TodoController_GetOne", new { id = newTodo.Id }, ToDetailDto(newTodo));
            } catch (CATodoException ex) when (ex.Code == 104) { return BadRequest(ex.Message); }
        }

        [HttpPut, Route("{id}")]
        public async Task<ActionResult<TodoDetailDto>> PutOneAsync(int id, TodoPutDto dto) {
            try {
                await _todoService.UpdateCategoriesForTodoAsync(id, dto.Categories ?? new List<int>());
                var updatedTodo = await _todoService.UpdateTodoAsync(new TodoUpdate() {
                    DueDate = DateOnly.FromDateTime(dto.DueDate!.Value),
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    Title = dto.Title,
                    Id = id
                });
                return ToDetailDto(updatedTodo);
            } catch (CATodoException ex) when (ex.Code == 101) { return NotFound(ex.Message); } catch (CATodoException ex) when (ex.Code == 104) { return BadRequest(ex.Message); }
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
