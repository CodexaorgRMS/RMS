using Customers.Presentation.Dtos;
using HotChocolate;
using HotChocolate.Types;

namespace Customers.Presentation.Subscriptions
{
	[ExtendObjectType(typeof(SharedPresentation.GraphQL.Subscription))]
	public class CustomerSubscriptions
	{
		[Subscribe]
		[Topic]
		public CustomerDto OnCustomerCreated([EventMessage] CustomerDto customer) => customer;

	}
}
