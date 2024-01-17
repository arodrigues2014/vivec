namespace Vrt.Vivec.Svc.Helpers
{
    public class ErrorResultHelper
    {
        public object? Input { get; set; }
        public IList<ValidationError>? ValidationErrors { get; set; }

        public ErrorResultHelper(object? input, IList<ValidationFailure>? validationErrors)
        {
            Input = input;

            ValidationErrors = validationErrors?.Select(error =>
                new ValidationError(error.PropertyName, error.ErrorMessage)).ToList();
        }

        public class ValidationError
        {
            public string PropertyName { get; set; }
            public string ErrorMessage { get; set; }

            public ValidationError(string propertyName, string errorMessage)
            {
                PropertyName = propertyName;
                ErrorMessage = errorMessage;
            }
        }
    }
}
