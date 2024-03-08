using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CATodos.Entities {
    public class CategoryEntity : BaseEntity {
        public string Label { get; set; } = null!;
        public int Color { get; set; }

        public virtual IEnumerable<TodoEntity> Todos { get; set; } = new HashSet<TodoEntity>();
    }
}
