using System.Drawing;

namespace CATodos.BusinessModels {
    public record Category {
        public int Id { get; init; }
        public string Label { get; init; } = null!;
        public Color Color { get; init; }
        public bool IsTopCategory { get; init; }
    }
}
