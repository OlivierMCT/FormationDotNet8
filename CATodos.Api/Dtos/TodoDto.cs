using CATodos.BusinessModels;
using System.ComponentModel.DataAnnotations;

namespace CATodos.Api.Dtos {

    public record TodosDto {
        public List<CategoryDto> Categories { get; init; } = null!;
        public List<TodoDto> Todos { get; init; } = null!;
    }

    public abstract record TodoBaseDto {
        public int Id { get; init; }
        public string Title { get; init; } = null!;
        public DateOnly DueDate { get; init; }
        public bool Done { get; init; }
    }

    public record TodoDto : TodoBaseDto {
        public List<int> Categories { get; init; } = null!;
    }

    public record TodoDetailDto : TodoBaseDto {
        public double? Latitude { get; init; }
        public double? Longitude { get; init; }
        public bool Deletable { get; init; }
        public string Status { get; init; } = null!;

        public List<CategoryDto> Categories { get; init; } = null!;
    }

    public record TodoPostDto {
        [Required(AllowEmptyStrings = false, ErrorMessage = "le titre est obligatoire")]
        public string? Title { get; set; } = null!;
        [Required(ErrorMessage = "l'échéance est obligatoire")]
        public DateTime? DueDate { get; set; }
        [Range(-90, 90, ErrorMessage = "la latitude est invalide")]
        public double? Latitude { get; set; }
        [Range(-180, 180, ErrorMessage = "la longitude est invalide")]
        public double? Longitude { get; set; }
    }

    public record TodoPutDto {
        [Required(AllowEmptyStrings = false, ErrorMessage = "le titre est obligatoire")]
        public string? Title { get; set; } = null!;
        [Required(ErrorMessage = "l'échéance est obligatoire")]
        public DateTime? DueDate { get; set; }
        [Range(-90, 90, ErrorMessage = "la latitude est invalide")]
        public double? Latitude { get; set; }
        [Range(-180, 180, ErrorMessage = "la longitude est invalide")]
        public double? Longitude { get; set; }

        public List<int>? Categories { get; set; } = new List<int>();
    }

    public record TodoPatchDto {
        [Required(ErrorMessage = "l'état est obligatoire")]
        public bool? Done { get; set; } = null!;
    }
}
