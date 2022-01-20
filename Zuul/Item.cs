using System;
using System.Collections.Generic;
using System.Text;

namespace Zuul
{
    public class Item
    {
        public string Description { get; }
        public int Weight { get; }
        public Item(int weight, string description)
        {
            Weight = weight;
            Description = description;
        }
	}
}
