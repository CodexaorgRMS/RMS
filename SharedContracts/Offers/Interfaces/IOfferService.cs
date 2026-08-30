using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedContracts.Sales.Events;

namespace SharedContracts.Offers.Interfaces;

public interface IOfferService
{
    Task<decimal> CalculateDiscountAsync(IEnumerable<SoldItemDto> items, CancellationToken cancellationToken = default); 
}
