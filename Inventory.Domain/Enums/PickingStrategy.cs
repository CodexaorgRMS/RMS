namespace Inventory.Domain.Enums
{
	public enum PickingStrategy
	{
		FEFO = 1, // First Expired, First Out 
		FIFO = 2, // First In, First Out
		LIFO = 3, // Last In, First Out
		HIFO = 4  // Highest In, First Out
	}
}
