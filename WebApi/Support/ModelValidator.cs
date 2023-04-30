﻿namespace WebApi.Support
{
    public class ModelValidator
    {
        private ModelValidator()
        {
        }

        public static List<ValidationResult> ValidateModel(object obj)
        {
            ValidationContext context = new(obj, null, null);
            List<ValidationResult> results = new();
            Validator.TryValidateObject(obj, context, results);
            return results;
        }
    }
}
