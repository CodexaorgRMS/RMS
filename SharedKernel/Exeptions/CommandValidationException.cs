using FluentValidation.Results;

namespace SharedKernel.Exeptions
{
	public sealed class CommandValidationException : Exception
	{
		public IReadOnlyList<ValidationFailure> Failures { get; }

		public CommandValidationException(IReadOnlyList<ValidationFailure> failures)
			: base("Validation failed for one or more command properties.")
		{
			Failures = failures;
		}
	}
}
