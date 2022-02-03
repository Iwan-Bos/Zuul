using System.Collections.Generic;

namespace Zuul
{
	public class Inventory
	{
		private int maxWeight;
		private Dictionary<string, Item> items;

		public Inventory(int maxWeight)
		{
			this.maxWeight = maxWeight;
			items = new Dictionary<string, Item>();
		}

		public int WeightCheck() // Method for checking if the Item exists and fits in the items Collection.
		{
			int allItemWeight = 0;
			int remainingWeight;

			// add up all item weight
			foreach (KeyValuePair<string, Item> pair in items)
			{
				allItemWeight += pair.Value.Weight;
			}
			// calculate the remaining weight that the inventory can take
			remainingWeight = maxWeight - allItemWeight;
			return remainingWeight;
		}
		public void Put(string itemName, Item item) { items.Add(itemName, item); } // Method for putting an Item to the items Collection.
		public Item Get(string itemName) // Method for removing an item from an inventory, and returning the Item.
		{
			Item item = null;

			// find Item in items Collection
			if (items.ContainsKey(itemName))
			{
				item = items[itemName];
				// remove Item from items Collection if found
				items.Remove(itemName);
			}
			return item;
		}
		public string ListItems() // Method for listing all items in the items Collection, returns string.
		{
			string itemlist = null;
			if (items.Count != 0)
			{
				itemlist += "---------------------------------\n";
				foreach (KeyValuePair<string, Item> item in items)
				{
					itemlist += $"|> {item.Key} {item.Value.Weight}kg\n";
					itemlist += $"| \'{item.Value.Description}\'\n";
				}
				itemlist += "---------------------------------\n";
			}
			else
			{
				itemlist += "Looks like it's empty";
			}
			return itemlist;
		}
		public bool CollectionContainsKey(string itemName)
		{
			if (items.ContainsKey(itemName))
			{
				return true;
			}
			return false;
		} // Method for using a method somewhere else.
	}
}