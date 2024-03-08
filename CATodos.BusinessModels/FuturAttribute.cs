
using System.ComponentModel.DataAnnotations;

namespace CATodos.BusinessModels {
    public class FuturAttribute : ValidationAttribute {
        public bool IncludeToday { get; set; }

        public override bool IsValid(object? value) {
            if(value != null && value is DateOnly dateValue) {

            }
            else return true;
        }
    }S
}