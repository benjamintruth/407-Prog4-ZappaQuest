using System.Collections.Generic;

namespace ZappaQuest
{
	public class Room
	{
		// DEV
		public string Name { get; set; }
		public string Description { get; set; }
		public Room[] Exits;

		// List of Items for each room Created
		public List<Item> ItemsRoom{ get; set; }

		public Room(string name, string description, Room[] exits)
		{
			Name = name;
			Description = description;
			Exits = exits;
			ItemsRoom = new List<Item>();
		}


		// print room description:
		public void PrintRoomDescription()
		{
			Console.WriteLine($"You are in the {Name}.");
			Console.WriteLine(Description);
			Console.WriteLine("Exits: ");
			foreach (var exit in Exits)
			{
				if (exit != null)
				{
					Console.WriteLine($"		{exit.Name}.");
				}
			}

			Console.WriteLine("Current Items in room:");
			foreach (var items in ItemsRoom) {
				Console.WriteLine($"		{items.Information()}");
			}

		}

	}
}