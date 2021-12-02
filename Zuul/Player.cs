using System;
using System.Collections.Generic;
using System.Text;

namespace Zuul
{
    public class Player
    {
        private int health;
        public Room CurrentRoom { get; set; }
        public Player()
        {
            CurrentRoom = null;
            health = 100;
        }
        //Methods
        public void Damage(int amount)
        {
            this.health -= amount;
        }
        public void Heal(int amount)
        {
            this.health += amount;
        }
        public bool IsAlive()
        {
            if (this.health <= 0)
            {
                return false;
            }
            return true;
        }
    }
}