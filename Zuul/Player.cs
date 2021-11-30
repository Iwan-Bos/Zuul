using System;
using System.Collections.Generic;
using System.Text;

namespace Zuul
{
    public class Player
    {
        public Room currentRoom;
        private int health;
        public Player()
        {
            health = 100;
            currentRoom = null;
        }
    }
}
