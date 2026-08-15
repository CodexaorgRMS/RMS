using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContracts.Inventory.Dtos
{
	public record ProductDto(string Name, string Description, decimal SellingPrice, 
		Guid CategoryId,bool IsActive,string Barcode);

}
