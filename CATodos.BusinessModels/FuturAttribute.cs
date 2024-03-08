
using System.ComponentModel.DataAnnotations;

namespace CATodos.BusinessModels {
    public class FuturAttribute : ValidationAttribute {
        public bool IncludeToday { get; set; }

        public override bool IsValid(object? value) {
            if(value != null && value is DateOnly dateValue) {
                var today = DateOnly.FromDateTime(DateTime.Today);
                return dateValue > today || ( IncludeToday && dateValue >= today );
            }
            else return true;
        }
    }
}