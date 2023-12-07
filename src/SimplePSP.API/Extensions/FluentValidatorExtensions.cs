using FluentValidation.Results;

namespace SimplePSP.API.Extensions
{
    public static class FluentValidatorExtensions
    {
        public static Dictionary<string, string[]> GetErrorsDictionary(this ValidationResult validation)
        {
            var errorsDict = new Dictionary<string, string[]>();
            foreach (var error in validation.Errors)
                errorsDict.Add(error.PropertyName, [error.ErrorMessage]);

            return errorsDict;
        }
    }
}