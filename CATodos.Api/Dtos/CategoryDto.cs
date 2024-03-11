using System.Drawing;

namespace CATodos.Api.Dtos {
    public record CategoryDto {
        public int Id { get; init; }
        public string Label { get; init; } = null!;
        public string Color { get; init; } = null!;
        public bool Top { get; init; }
    }
}
