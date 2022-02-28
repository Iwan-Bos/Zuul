using System;
namespace Zuul
{
	public class Player
	{
		public int health { get; set; }
		private int maxhealth;
		public Inventory Backpack { get; }
		public Room CurrentRoom { get; set; }
		public Player()
		{
			CurrentRoom = null;
			health = 100;
			maxhealth = 100;
			Backpack = new Inventory(5);
		}
		public void Damage(int amount) // Method for reducing health of the player.
		{
			this.health -= amount;
		}
		public string Heal(int amount) // Method for healing the player.
		{
			int healedAmount;
			string confirmation;
			if (health + amount < maxhealth) // If the heal doesn't overflow maxhealth, heal
			{
				health += amount;
				Backpack.Get("bandage"); // !!! "bandage" has to be changed to a variable if I wanna add more healing items !!!
				confirmation = $"Applied the bandage and healed {amount}hp!";
				return confirmation;
			}
			if (health == maxhealth) // if player is dumb and wants to heal, ask for confirmation
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Your hp is already max");
				Console.WriteLine("Are you sure you wanna use this? y/n");
				Console.ResetColor();
				Console.Write("> ");
				string answer = Console.ReadLine();
				while (answer != "y" && answer != "n") // while loop to ask until the user has given a proccessable response
				{
					Console.WriteLine("You can only answer using y or n");
					Console.Write("> ");
					answer = Console.ReadLine();
				}
				if (answer == "y") // code based on decision made
				{
					Backpack.Get("bandage"); // !!! "bandage" has to be changed to a variable if I wanna add more healing items !!!
					confirmation = "Applied the bandage and healed 0hp!";
					return confirmation;
				}
				if (answer == "n")
				{
					confirmation = "You chose not to use the bandage";
					return confirmation;
				}
			}
			// Some math bad thing to calculate healedAmount
			healedAmount = amount - (amount + (health - maxhealth));
			Backpack.Get("bandage"); // !!! "medkit" has to be changed to a variable if I wanna add more healing items !!!
			confirmation = $"Applied the bandage and healed {healedAmount}hp!";
			health = maxhealth; // set the health to the limit after the calculation
			return confirmation;
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
					Console.WriteLine($"You took {itemName}");
					return true;
				}
				CurrentRoom.Chest.Put(itemName, item);
				Console.WriteLine($"{itemName} doesn't fit");
				return false;
			}
			Console.WriteLine("This room does not have this item");
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
					Console.WriteLine($"You dropped {itemName}");
					return true;
				}
				Backpack.Put(itemName, item);
				Console.WriteLine($"{itemName} doesn't fit");
				return false;
			}
			Console.WriteLine("You don't have that item");
			return false;
		}
		public string GetInvDescription() // Prints 2 lists with the player and room Inventory
		{
			int bpackMaxWeight = 5;
			int chestMaxWeight = 10;
			int bpackStoredWeight = bpackMaxWeight - Backpack.WeightCheck();
			int chestStoredWeight = chestMaxWeight - CurrentRoom.Chest.WeightCheck();

			string str = "\n";
			str += $"you check the room: {chestStoredWeight}/{chestMaxWeight} kg\n";
			str += $"{CurrentRoom.Chest.ListItems()}\n";
			str += $"You check your backpack: {bpackStoredWeight}/{bpackMaxWeight} kg\n";
			str += Backpack.ListItems();
			return str;
		}
	}
}