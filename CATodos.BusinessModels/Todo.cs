using System.ComponentModel.DataAnnotations;

namespace CATodos.BusinessModels {
    public record Todo {
        public int Id { get; init; }
        public string Title { get; init; } = null!;
        public DateOnly DueDate { get; init; }
        public bool IsDone { get; init; }
        public TodoCoordinate? Coordinate { get; init; }
        public bool IsDeletable { get; init; }
        public TodoStatus Status { get; init; }

        public List<Category> Categories { get; init; } = new();
    }

    public record TodoCoordinate {
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }

    public enum TodoStatus {
        Completed = -1, InProgress = 1, Late = 9
    }

    public class TodoCreate {
        [Required(AllowEmptyStrings = false, ErrorMessage = "le titre est obligatoire")]
        public string? Title { get; set; } = null!;
        [Required(ErrorMessage = "l'échéance est obligatoire")]
        [Futur(IncludeToday = true, ErrorMessage = "l'échéance est invalide")]
        public DateOnly? DueDate { get; set; }
        [Range(39, 72, ErrorMessage = "la latitude est invalide")]
        public double? Latitude { get; set; }
        [Range(-12, 42, ErrorMessage = "la longitude est invalide")]
        public double? Longitude { get; set; }
    }

    public class TodoUpdate {
        [Required(ErrorMessage = "l'identifiant est obligatoire")]
        public int? Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "le titre est obligatoire")]
        public string? Title { get; set; } = null!;
        [Required(ErrorMessage = "l'échéance est obligatoire")]
        public DateOnly? DueDate { get; set; }
        [Range(39, 72, ErrorMessage = "la latitude est invalide")]
        public double? Latitude { get; set; }
        [Range(-12, 42, ErrorMessage = "la longitude est invalide")]
        public double? Longitude { get; set; }
    }
}
