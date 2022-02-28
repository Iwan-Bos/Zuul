namespace Zuul
{
	public class Item
	{
		public Room WinRoom { get; set; }
		public Room PennyRoom { get; set; }
		public string Description { get; }
		public int Weight { get; }
		public Item(int weight, string description)
		{
			Weight = weight;
			Description = description;
		}
		public virtual void Use(Command command)
		{
			// Base Use Method
		}
	}
}
