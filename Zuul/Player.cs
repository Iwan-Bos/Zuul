using System;
using System.Collections.Generic;
using System.Text;

namespace Zuul
{
    public class Player
    {
        private int health;
		public Inventory Backpack { get; }
		public Room CurrentRoom { get; set; }
        public Player()
        {
            CurrentRoom = null;
            health = 100;
            Backpack = new Inventory(25);
        }
        public void Damage(int amount) // Method for reducing health of the player.
        {
            this.health -= amount;
        }
        public void Heal(int amount) // Method for increasing health of the player.
        {
            this.health += amount;
        }
        public bool IsAlive() // Method for checking if the player is alive, returns boolean.
        {
            if (this.health <= 0)
            {
                return false;
            }
            return true;
        }
        public bool TakeFromChest(string itemName) // Takes Item from Chest, includes weight check, returns boolean.
        {
			Item item = CurrentRoom.Chest.Get(itemName);
			if (item != null)
			{
				if (Backpack.WeightCheck() >= item.Weight)
				{
					Backpack.Put(itemName, item);
					Console.WriteLine($"Took {itemName} from the chest");
					return true;
				}
				CurrentRoom.Chest.Put(itemName, item);
				Console.WriteLine($"{itemName} Doesn't fit");
			}
			return false;
		}
        public bool DropToChest(string itemName) // Takes Item from backpack, includes weight check, returns boolean.
		{
			Item item = Backpack.Get(itemName);
			if (item != null)
			{
				if (CurrentRoom.Chest.WeightCheck() >= item.Weight)
				{
					CurrentRoom.Chest.Put(itemName, item);
					Console.WriteLine($"Dropped {itemName} into the chest");
					return true;
				}
				Backpack.Put(itemName, item);
				Console.WriteLine($"{itemName} doesn't fit");
			}
			return false;
		}
		public string GetInvDescription()
		{
			int bpackMaxWeight = 25;
			int chestMaxWeight = 500;
			int bpackStoredWeight = bpackMaxWeight - Backpack.WeightCheck();
			int chestStoredWeight = chestMaxWeight - CurrentRoom.Chest.WeightCheck();

			string str = "\n";
			str += $"you check the room's chest: {chestStoredWeight}/{chestMaxWeight} kg\n";
			str += CurrentRoom.Chest.ListItems();
			str += "\n";
			str += $"You check your backpack: {bpackStoredWeight}/{bpackMaxWeight} kg\n";
			str += Backpack.ListItems();
			return str;
		}
	}
}