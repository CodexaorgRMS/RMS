using Microsoft.EntityFrameworkCore;
using Offers.Domain.Entities;

namespace Offers.Application.Abstractions;

public interface IOffersDataContext
{
    DbSet<Offer> Offers { get; }
    DbSet<OfferTarget> OfferTargets { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}