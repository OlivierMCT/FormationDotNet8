using CATodos.Api.Dtos;
using CATodos.Api.Filters;
using CATodos.Business;
using CATodos.BusinessModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CATodos.Api.Controllers {
    [Route("category")]
    public class CategoryController : ControllerBase {
        private readonly ICATodoService _todoService;

        public CategoryController(ICATodoService todoService) {
            _todoService = todoService;
        }

        [HttpGet, ExcelCsvResourceFilter, OutputCache(Duration = 20)]
        public IEnumerable<CategoryDto> GetAll([FromServices] ILogger<CategoryController> logger) {
            logger.LogInformation("{}> GetAll Categories Action", DateTime.Now.ToString("T"));
            return _todoService.GetAllCategories().Select(ToDto);
        }

        [HttpGet, Route("{id:int}"), CATodoExceptionFilter]
        public CategoryDto GetOne([FromRoute] int id) {
            return ToDto(_todoService.GetCategory(id));
        }

        internal static CategoryDto ToDto(Category c) {
            return new CategoryDto() {
                Id = c.Id,
                Top = c.IsTopCategory,
                Label = c.Label,
                Color = $"#{c.Color.R:x2}{c.Color.G:x2}{c.Color.B:x2}{c.Color.A:x2}"
            };
        }
    }
}
