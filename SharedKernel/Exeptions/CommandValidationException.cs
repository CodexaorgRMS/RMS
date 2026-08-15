using FluentValidation.Results;

namespace SharedKernel.Exeptions
{
	public class CommandValidationException : Exception
	{
		public IDictionary<string, string[]> Errors { get; }

		public CommandValidationException(IReadOnlyList<ValidationFailure> failures)
			: base("Validation failed for one or more command properties.")
		{
			Errors = failures
				.GroupBy(f => f.PropertyName, f => f.ErrorMessage)
				.ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
		}
	}


	public sealed class NotFoundException : Exception
	{
		public NotFoundException(string message) : base(message)
		{
		}

		public NotFoundException(string entityName, object key)
			: base($"'{entityName}' with key '{key}' was not found.")
		{
		}
	}


	public sealed class ConflictException : Exception
	{
		public ConflictException(string message) : base(message)
		{
		}
	}

	public sealed class ForbiddenException : Exception
	{
		public ForbiddenException(string message) : base(message)
		{
		}
	}
}
