using System.Collections.Generic;
using System;

namespace Zuul
{
	public class Room
	{
		public Inventory Chest { get; }
		public bool isLocked;
		private string description;
		private Dictionary<string, Room> exits; // stores exits of this room.

		public Room(string desc, bool locked)
		{
			description = desc;
			isLocked = locked;
			exits = new Dictionary<string, Room>();
			Chest = new Inventory(10);
		}

		// Define an exit from this room.
		public void AddExit(string direction, Room neighbor)
		{
			exits.Add(direction, neighbor);
		}
		public bool IsLocked()
		{
			return isLocked;
		}
		public string GetLongDescription()
		{
			string str = "You are ";
			str += description;
			str += ".\n";
			Console.ResetColor();
			str += GetExitString();
			str += ".\n";
			return str;
		}
		public Room GetExit(string direction)
		{
			if (exits.ContainsKey(direction))
			{
				return exits[direction];
			}
			else
			{
				return null;
			}
		}
		public string GetExitString()
		{
			string str = "Exits:";

			// because `exits` is a Dictionary, we use a `foreach` loop
			int countcommas = 0;
			foreach (string key in exits.Keys)
			{
				if (countcommas != 0)
				{
					str += ",";
				}
				str += " " + key;
				countcommas++;
			}
			return str;
		}
	}
}
