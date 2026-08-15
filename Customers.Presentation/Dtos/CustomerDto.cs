using System;
using System.Collections.Generic;
using System.Text;

namespace Customers.Presentation.Dtos
{
	public record CustomerDto( 
		Guid Id,
		string Name,
		string Phone,
		decimal TotalDebt,
		DateTime CreatedAt
	);
}
