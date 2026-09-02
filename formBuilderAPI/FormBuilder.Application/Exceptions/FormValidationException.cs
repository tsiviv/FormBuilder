namespace FormBuilder.Application.Exceptions;

public class FormValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public FormValidationException(IEnumerable<string> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors.ToList();
    }
}
