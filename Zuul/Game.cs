using System;

namespace Zuul
{
	public class Game
	{
		private readonly Parser parser;
		private readonly Player player;
		public Room WinRoom { get; set; }
		public Room PennyRoom { get; set; }
		public Game()
		{
			parser = new Parser();
			player = new Player();
			CreateRooms();
		}
		private void CreateRooms()
		{
			// create the rooms
			Room gate = new Room("past the gate that kept you inside", true);
			Room field = new Room("in the field, behind the gates of the garden", false);
			Room grove = new Room("in a grove filled with trees", false);
			Room fountain = new Room("in front of a wishing fountain.", false);
			Room river = new Room("near a shallow river.", false);
			Room cave = new Room("at an old overgrown cave", false);
			Room entrance = new Room("down at entrance of the depths of the cave, it goes down 3 meters...", false);
			Room tunnel = new Room("inside a narrow hall with a ladder leading down", false);
			Room treasureroom = new Room("in a room with the greatest treasures you've ever seen! or at least that is what you hoped...", false);

			// initialise room exits
			field.AddExit("south", gate);
			field.AddExit("east", grove);
			field.AddExit("north", fountain);

			grove.AddExit("west", field);
			grove.AddExit("north", river);
			
			river.AddExit("south", grove);
			river.AddExit("west", fountain);
			
			fountain.AddExit("east", river);
			fountain.AddExit("west", cave);
			fountain.AddExit("south", field);

			cave.AddExit("east", fountain);
			cave.AddExit("down", entrance);
			
			entrance.AddExit("up", cave);
			entrance.AddExit("east", tunnel);
			
			tunnel.AddExit("west", entrance);
			tunnel.AddExit("down", treasureroom);
			
			treasureroom.AddExit("up", tunnel);

			PennyRoom = fountain; // Room where the penny is useable
			WinRoom = gate; // win when gate is entered
			player.CurrentRoom = field;  // start game in field


			// create the items
			Item bandage = new Item(1, "a soggy worn out bandage...\n| with magical healing powers!");
			Item rock = new Item(5, "dang that's a heavy rock,\n| you might want to drop this.");
			Item key = new Item(2, "an antique looking key,\n| useful for unlocking things.");
			Item penny = new Item(2, "a large penny found on the floor,\n| what mysteries could it hold?");
			Item apple = new Item(3, "a shiny red apple found lying on the ground,\n| would almost be a waste to eat.");


			// add items to desired items Collection
			player.Backpack.Put("rock", rock);

///			field.Chest.Put("", );
			
			grove.Chest.Put("apple", apple);
			
			river.Chest.Put("bandage", bandage);

			fountain.Chest.Put("penny", penny);
			
///			cave.Chest.Put("", );
			
///			entrance.Chest.Put("", );
			
///			tunnel.Chest.Put("", );
			
			treasureroom.Chest.Put("key", key);
		}


		// Main play routine.  Loops until end of play.
		public void Play()
		{
			PrintWelcome();

			// Enter the main command loop.  Here we repeatedly read commands and
			// execute them until the player wants to quit.
			bool finished = false;
			while (!finished)
			{
				if (InWinRoom())
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"You managed to escape the garden!");
					Console.ResetColor();
					finished = true;
				}
				else if (player.IsAlive() == true)
				{
					Command command = parser.GetCommand();
					finished = ProcessCommand(command);
				}
				else
				{
					Console.WriteLine();
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("You got pricked to death by the prickely grass...");
					Console.ResetColor();
					finished = true;
				}
			}
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Thank you for playing.");
			Console.ResetColor();
			Console.ReadLine();
		}

		/**
		 * Print out the opening message for the player.
		 */
		private void PrintWelcome()
		{
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Welcome to Zuul!");
			Console.WriteLine("Zuul is an edited, incredibly pointless adventure game.");
			Console.WriteLine("Type 'help' if you need help.");
			Console.WriteLine();
			Console.WriteLine(player.CurrentRoom.GetLongDescription());
			Console.ResetColor();
		}

		/**
		 * Given a command, process (that is: execute) the command.
		 * If this command ends the game, true is returned, otherwise false is
		 * returned.
		 */
		private bool ProcessCommand(Command command)
		{
			bool wantToQuit = false;

			if (command.IsUnknown())
			{
				Console.WriteLine("I don't know what you mean...");
				return false;
			}

			string commandWord = command.GetCommandWord();
			switch (commandWord)
			{
				case "help":
					PrintHelp();
					break;
				case "look":
					Look();
					break;
				case "inspect":
					Console.WriteLine(player.GetInvDescription());
					break;
				case "go":
					GoRoom(command);
					break;
				case "take":
					Take(command);
					break;
				case "drop":
					Drop(command);
					break;
				case "use":
					Use(command);
					break;
				case "quit":
					wantToQuit = true;
					break;
			}
			return wantToQuit;
		}

		// implementations of user commands:
		private void Look()
		{
			Console.WriteLine($"Your health: {player.health}hp");
			Console.WriteLine(player.CurrentRoom.GetLongDescription());
		}
		private void PrintHelp()
		{
			Console.WriteLine("You wake up inside a field,");
			Console.WriteLine("The only thing between you and freedom is the garden gate");
			Console.WriteLine();
			// let the parser print the commands
			parser.PrintValidCommands();
		}
		private void GoRoom(Command command)
		{
			if (!command.HasSecondWord())
			{
				Console.WriteLine("Go where?");
				return;
			}

			string direction = command.GetSecondWord();

			// Try to go to the next room
			Room nextRoom = player.CurrentRoom.GetExit(direction);

			if (nextRoom == null)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"There is no passage {direction}wards!");
				Console.ResetColor();
			}
			else if (nextRoom.IsLocked())
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("The gate is locked!");
				Console.ResetColor();
			}
			else
			{
				player.CurrentRoom = nextRoom;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("The garden's plants prickle you\n");
				Console.ResetColor();
				player.Damage(10);
				Console.WriteLine($"Your health: {player.health}hp");
				Console.WriteLine(player.CurrentRoom.GetLongDescription());
			}
		}
		private void Take(Command command)
		{
			if (!command.HasSecondWord())
			{
				Console.WriteLine("Take what?");
				return;
			}
			player.TakeFromChest(command.GetSecondWord());
		}
		private void Drop(Command command)
		{
			if (!command.HasSecondWord())
			{
				Console.WriteLine("Drop what?");
				return;
			}
			player.DropToChest(command.GetSecondWord());
		}
		private void Use(Command command)
		{
			string itemName = command.GetSecondWord();
			string direction = command.GetThirdWord();

			if (command.HasSecondWord())
			{
				if (player.Backpack.CollectionContainsKey(itemName))
				{
					// Polymorphism doesn't wanna, so I won't use inheritance for now
					switch (itemName)
					{
						case "key":
							if (command.HasThirdWord())
							{
								Room targetedRoom = player.CurrentRoom.GetExit(direction);
								// check if the direction actually has a Room
								if (targetedRoom != null)
								{
									// check if the room is even locked
									if (targetedRoom.isLocked)
									{
										targetedRoom.isLocked = false;
										player.Backpack.Get("key");
										Console.ForegroundColor = ConsoleColor.Green;
										Console.WriteLine($"The gate has been unlocked!");
										Console.ResetColor();
										return;
									}
									Console.ForegroundColor = ConsoleColor.Yellow;
									Console.WriteLine($"this was never locked");
									Console.ResetColor();
								}
							}
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.WriteLine("Use key where?");
							Console.WriteLine(player.CurrentRoom.GetExitString());
							Console.ResetColor();
							return;
						case "bandage":
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine(player.Heal(50));
							Console.ResetColor();
							return;
						case "penny":
							if (player.CurrentRoom != PennyRoom)
							{
								Console.WriteLine($"I don't think you can use a penny in this room.");
								return;
							}
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.Write("You throw the penny in the fountain");
							System.Threading.Thread.Sleep(1200);
							Console.Write("\n.");
							System.Threading.Thread.Sleep(500);
							Console.Write(".");
							System.Threading.Thread.Sleep(500);
							Console.Write(".");
							System.Threading.Thread.Sleep(1200);
							Console.ForegroundColor= ConsoleColor.Green;
							Console.WriteLine("\nIt did nothing :D");
							Console.ResetColor();
							player.Backpack.Get("penny");
							return;
						case "apple":
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.Write("You took a bite of the apple");
							System.Threading.Thread.Sleep(1200);
							Console.Write("\n.");
							System.Threading.Thread.Sleep(500);
							Console.Write(".");
							System.Threading.Thread.Sleep(500);
							Console.Write(".");
							System.Threading.Thread.Sleep(1200);
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("\nIt tastes disgusting,\n reduced hp by 10");
							player.Damage(10);
							Console.ResetColor();
							player.Backpack.Get("apple"); 
							return;
					}
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("I didn't add this item to the case switch -_-\"");
					Console.ResetColor();
					return;
				}
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"You don't have an item called: \"{itemName}\"");
				Console.ResetColor();
				return;
			}
			Console.WriteLine("Use what?");
			return;
		}
		public bool InWinRoom()
		{
			if (player.CurrentRoom == WinRoom)
			{
				return true;
			}
			return false;
		}
	}
}