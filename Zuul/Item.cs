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
		public virtual void Use(string itemName)
		{

		}
	}
}
