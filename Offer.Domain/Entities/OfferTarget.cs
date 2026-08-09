using Offer.Domain.Enums;
using System;

namespace Offers.Domain.Entities
{
	public class OfferTarget
	{
		public Guid OfferTargetId { get; set; }

		public Guid OfferId { get; set; }

		public OfferTargetType TargetType { get; set; }

		public Guid TargetId { get; set; }
	}
}
