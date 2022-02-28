using System.Collections.Generic;
using System;

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

		public int WeightCheck() 
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
		} // Method for checking if the Item exists and fits in the items Collection.
		public void Put(string itemName, Item item) { items.Add(itemName, item); } // Method for putting an Item to the items Collection.
		public Item Get(string itemName) 
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
		} // Method for removing an item from an inventory, and returning the Item.
		public string ListItems() 
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
		} // Method for listing all items in the items Collection, returns string.
		public bool CollectionContainsKey(string itemName)
		{
			if (items.ContainsKey(itemName))
			{
				return true;
			}
			return false;
		} // Method for using a method somewhere else.
	///	public void ListItems2() // Method for printing all items in the items Collection. returns nothing for use with console colors.
	///	{
	///		if (items.Count != 0)
	///		{
	///			Console.WriteLine("---------------------------------\n");
	///			foreach (KeyValuePair<string, Item> item in items)
	///			{
	///				Console.WriteLine($"|> {item.Key} {item.Value.Weight}kg\n");
	///				Console.WriteLine($"| \'{item.Value.Description}\'\n");
	///			}
	///			Console.WriteLine("---------------------------------\n");
	///		}
	///		else
	///		{
	///			Console.WriteLine("Looks like it's empty");
	///		}
	///	}
	}
}