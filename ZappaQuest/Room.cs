using System.Collections.Generic;

namespace ZappaQuest
{
	public class Room
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public Room(string name, string description)
		{
			Name = name;
			Description = description;
		}


		// print room description:
		public void PrintRoomDescription()
		{
			Console.WriteLine($"You are in the {Name} room.");
			Console.WriteLine(Description);
		}

	}
}