using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;

namespace Sales.Application.Abstractions
{
	public interface ISalesDataContext
	{
		DbSet<Order> Orders { get; set; }
		DbSet<OrderItem> OrderItems { get; set; }
	}
}
