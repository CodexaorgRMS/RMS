using Offer.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Offers.Domain.Entities
{
	public class Offer
	{
		public Guid OfferId { get; set; }

		public string Name { get; set; } = string.Empty;

		public string? Description { get; set; }

		public OfferType Type { get; set; }

		public decimal Value { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public bool IsSmart { get; set; }

		public ICollection<OfferTarget> Targets { get; set; }
			= new List<OfferTarget>();
	}
}
