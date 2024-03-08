namespace CATodos.Entities {
    public class TodoEntity : BaseEntity {
        public string Title { get; set; } = null!;
        public DateOnly DueDate { get; set; }
        public bool IsDone { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public virtual IEnumerable<CategoryEntity> Categories { get; set; } = new HashSet<CategoryEntity>();
    }
}
