using System.ComponentModel.DataAnnotations;

namespace WebApi.Support
{
    public class ModelValidator
    {
        public List<ValidationResult> ValidateModel(object obj)
        {
            ValidationContext context = new(obj, null, null);
            List<ValidationResult> results = new();
            Validator.TryValidateObject(obj, context, results);
            return results;
        }
    }
}
