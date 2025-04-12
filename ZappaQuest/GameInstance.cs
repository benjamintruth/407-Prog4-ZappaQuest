namespace ZappaQuest
{
	public class GameInstance
	{


		public Boolean GAME_OVER = false;
		public String[] PlayerData = new String[2];

		public void Initialize()
		{
			Console.WriteLine("WELCOME 2 ZAPPA QUEST!");
			PlayerData = GreetPlayer();


			RunGame();
		}

		private void RunGame()
		{
			// parse difficulty level
			int DifficultyLevel = Int32.Parse(PlayerData[1]);

			// create a number of rooms based on difficulty level
			Room[] Dungeon = BuildRooms(DifficultyLevel * 2);

			List<Item> items = GenerateItems();
			Random gen = new Random();

			// loop through items and place them randomly in rooms
			foreach (var item in items) {
				Room targetR = Dungeon[gen.Next(Dungeon.Length)];
				targetR.ItemsRoom.Add(item);
			}
			// loop through dungeon, printing exits, description, etc
			foreach (var room in Dungeon)
			{
				room.PrintRoomDescription();

			}





			// create player creature (Frank)



			// build room array
			while (!GAME_OVER)
			{

				// main game loop ( LOOP = TURN )






				GAME_OVER = true;
			}
		}



		private String[] GreetPlayer()
		{
			String[] PlayerData = new String[2];

			Console.WriteLine("What is your name?");
			PlayerData[0] = Console.ReadLine();
			Console.WriteLine($"Nice to meet you, {PlayerData[0]}. Are you up for a challenge?");
			String PlayerResponse = Console.ReadLine();
			// parse player respose... many other options removed for brevity
			switch (PlayerResponse)
			{
				case "yes":
				case "y":
					PlayerData[1] = "100";
					break;
				case "no":
				case "n":
					PlayerData[1] = "5";
					break;
				case "maybe":
				case "m":
					Console.WriteLine("YOU WILL BE PUNISHED FOR YOUR INDECISION.");
					PlayerData[1] = "500";
					break;
				default:
					PlayerData[1] = "10";
					break;
			}

			Console.WriteLine($"Your difficulty level is: {PlayerData[1]}. ");
			Console.WriteLine("It's time to be Frank. Let's play ZAPPA QUEST!.");
			return PlayerData;
		}



		public static Room[] BuildRooms(int maxRooms)
		{
			// should never happen, just here to prevent a crash maybe
			if (maxRooms <= 0)
			{
				return new Room[0];
			}

			Room[] rooms = new Room[maxRooms];

			// create all rooms with basic info
			for (int i = 0; i < maxRooms; i++)
			{
				string roomName = $"Room {i + 1}";
				string roomDescription = $"This is room number {i + 1}.";

				// special name and description for side rooms (every third room)
				if ((i + 1) % 3 == 0)
				{
					roomName = $"Side Room for room {(i)}";
					roomDescription = $"This is a side room connected to the main path. Side room for room {(i)}.";
				}

				rooms[i] = new Room(roomName, roomDescription, new Room[4]); // 4 possible exits: North, East, South, West
			}

			// add exits
			for (int i = 0; i < maxRooms; i++)
			{
				// main path rooms 
				if ((i + 1) % 3 != 0)
				{



					// connect to next room (North)
					if (i < maxRooms - 1)
					{

						// if next room is a side room, jump one ahead
						if (i + 1 < maxRooms && (i + 2) % 3 == 0)
						{
							rooms[i].Exits[0] = rooms[i + 2];
						}
						else
						{
							rooms[i].Exits[1] = rooms[i + 1];
						}
					}

					// connect to previous room (South)
					if (i > 0)
					{
						rooms[i].Exits[3] = rooms[i - 1];
					}

					// if this is the room before a side room, connect to side room (East)
					if (i + 1 < maxRooms && (i + 2) % 3 == 0)
					{
						rooms[i].Exits[0] = rooms[i + 1];
					}
				}
				// if side room
				else
				{
					// connect back to the main path (West)
					if (i > 0)
					{
						rooms[i].Exits[2] = rooms[i - 1];
					}
				}
			}

			return rooms;
		}

		// Items List 
		private List<Item> GenerateItems() {
			return new List<Item> {
				new Weapon("Stink Foot’s Heavy Guitar", false, 2, 25, true),
				new Armor("Goblin Girl Suit", false, 12, true),
				new Treasure("Black Napkin", false, 50),
				new Food("Peaches En Regalia Smoothie", false, 24),
				new MagicItem("Joe's Garage Glitter Amulet", 77),
				new Weapon("Valley Girl Microphone", false, 3, 13, true),
				new Armor("Cosmic Debris Chest Armor", false, 8, true),
				new Treasure("Apostrophe' Shiny Vinyl", false, 70),
				new Food("Muffin from Reasearch Laboratory", false, 20),
				new MagicItem("Yellow Frozen Snow Cone", 50),
				new Food("Easter Hay Watermelon", false, 50)
			};
		}


		// end class
	}
}