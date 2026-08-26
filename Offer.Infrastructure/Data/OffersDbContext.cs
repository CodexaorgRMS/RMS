using Microsoft.EntityFrameworkCore;
using Offers.Application.Abstractions;
using Offers.Domain.Entities;

namespace Offers.Infrastructure.Data;

public class OffersDbContext : DbContext, IOffersDataContext //OffersDbContext : DbContext, IOffersDbContext
{
    public OffersDbContext(DbContextOptions<OffersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Offer> Offers { get; set; }
    public DbSet<OfferTarget> OfferTargets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("Offers");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OffersDbContext).Assembly);
    }
}
