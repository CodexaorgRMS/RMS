using Customers.Application.Features.Customers.Commands.CreateCustomer;
using Customers.Application.Features.Debts.Commands.AddManualCustomerDebt;
using Customers.Application.Features.Payments.Commands.AddCustomerPayment;
using Customers.Presentation.Requests;
using Riok.Mapperly.Abstractions;

namespace Customers.Presentation.Mapping
{
	[Mapper]
	public partial class CustomerMapper
	{
		public partial CreateCustomerCommand MapToCommand(CreateCustomerRequest request);

		public partial AddCustomerPaymentCommand MapToCommand(AddCustomerPaymentRequest request, Guid customerId);

		public partial AddManualCustomerDebtCommand MapToCommand(AddManualCustomerDebtRequest request, Guid customerId);
	}
}
