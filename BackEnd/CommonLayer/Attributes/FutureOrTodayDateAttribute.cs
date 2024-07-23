using System.ComponentModel.DataAnnotations;

namespace CommonLayer.Attributes
{
    public class FutureOrTodayDateAttribute : ValidationAttribute
    {
        public FutureOrTodayDateAttribute()
        {
            ErrorMessage = "The date must be today or in the future.";
        }

        public override bool IsValid(object? value)
        {
            if (value is DateOnly date)            
                return date >= DateOnly.FromDateTime(DateTime.Today);
            
            return value is null;
        }
    }
}
