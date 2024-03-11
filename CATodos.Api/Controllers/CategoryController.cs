using CATodos.Api.Dtos;
using CATodos.Business;
using CATodos.BusinessModels;
using Microsoft.AspNetCore.Mvc;

namespace CATodos.Api.Controllers {
    [Route("categorie")]
    public class CategoryController : ControllerBase {
        private readonly ICATodoService _todoService;

        public CategoryController(ICATodoService todoService) {
            _todoService = todoService;
        }

        [HttpGet]
        public IEnumerable<CategoryDto> GetAll() {
            return _todoService.GetAllCategories().Select(ToDto);
        }

        [HttpGet, Route("{id:int}")]
        public ActionResult<CategoryDto> GetOne([FromRoute] int id) {
            try {
                return ToDto(_todoService.GetCategory(id));
            }catch(CATodoException ex) when (ex.Code == 102)  {
                return NotFound(ex.Message);
            }
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
