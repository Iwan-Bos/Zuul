using System.Collections.Generic;

namespace Zuul
{
    public class Room
    {
		public Inventory Chest { get; }
		
		private string description;
        private Dictionary<string, Room> exits; // stores exits of this room.

		public Room(string desc)
        {
            description = desc;
            exits = new Dictionary<string, Room>();
            Chest = new Inventory(500);
        }
        
		// Define an exit from this room.
        public void AddExit(string direction, Room neighbor)
        {
            exits.Add(direction, neighbor);
        }

		/**
		 * Returns a long description of this room, in the form:
		 *     You are in the kitchen.
		 *     Exits: north, west
		 *     
		 *     In this room's chest lies: 
		 *     Item - Description
		 *     Item - Description
		 */
		public string GetLongDescription()
        {
            string str = "You are ";
            str += description;
            str += ".\n";
			str += GetExitString();
			str += ".\n";
            return str;
        }
		
        /**
		 * Return the room that is reached if we go from this room in direction
		 * "direction". If there is no room in that direction, return null.
		 */
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

        /**
		 * Return a string describing the room's exits, for example
		 * "Exits: north, west".
		 */
        private string GetExitString()
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
