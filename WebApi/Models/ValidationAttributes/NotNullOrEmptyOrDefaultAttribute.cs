namespace WebApi.Models.ValidationAttributes
{
    public class NotNullOrEmptyOrDefault : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is string str)
            {
                return !string.IsNullOrEmpty(str) && !string.Equals(str, "string", StringComparison.OrdinalIgnoreCase);
            }

            return true;
        }
    }
}