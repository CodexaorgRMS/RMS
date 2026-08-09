using FluentValidation.Results;
using SharedKernel.Exeptions;
using Wolverine.FluentValidation;

namespace SharedInfrastructure.Validations
{
	public sealed class ResultFailureAction<T> : IFailureAction<T>
	{
		public void Throw(T message, IReadOnlyList<ValidationFailure> failures)
		{
			throw new CommandValidationException(failures);
		}
	}
}
