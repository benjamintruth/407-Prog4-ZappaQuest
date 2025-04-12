using System.Threading.Tasks.Dataflow;

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
			foreach (var item in items)
			{
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

				// DEV: 
				Console.WriteLine($"Building room {i}");

				string roomName = $"Room {i + 1}";
				string roomDescription = $"This is room number {i + 1}.";

				Boolean isSideRoom = (i % 3 == 0);
				Boolean isFirstRoom = (i == 0);
				Boolean isLastRoom = (i + 1 == maxRooms);

				// special name and description for side rooms (every third room)
				if (isSideRoom && !isFirstRoom)
				{
					roomName = $"Side Room for {rooms[i - 1].Name}";
					roomDescription = $"This is a side room connected to {rooms[i - 1].Name}. It is also room {i + 1}.";
				}
				// isLastRoom passed into isDungeonExit so that lastRoom is exit
				// 4 possible exits: North, East, South, West. 
				// The indices in the exits array do not correspond to specific cardinal directions, as rooms can be 'rotated' 
				rooms[i] = new Room(roomName, roomDescription, isLastRoom, new Room[4]);
			}

			// add exits
			for (int i = 0; i < maxRooms; i++)
			{

				// DEV
				Console.WriteLine($"Adding exits to room {i}");

				Boolean isThisFirstRoom = i == 0;
				Boolean isThisSecondRoom = i == 1;
				// first room cannot be a side room bc a side room must be ahead of the room it connects to
				Boolean isSideRoom = (i) % 3 == 0 && !isThisFirstRoom;
				Boolean isThisLastRoom = i + 1 == maxRooms;
				Boolean isNextRoomSideRoom = (i + 1) % 3 == 0;
				// next in spot in the array plus one for the floor 0 conversion 
				Boolean isPreviousRoomSideRoom = (i - 1) % 3 == 0 && !(isThisFirstRoom || isThisSecondRoom);
				// two ahead plus one additional to convert from the floor being 0
				Boolean RoomAfterNextRoomExists = !(i + 3 > maxRooms);

				// main path rooms 
				if (!isSideRoom)
				{
					// connect to next room (North) if available
					if (!isThisLastRoom)
					{
						// add first room in array ( could be main path room or side room, add either way)
						rooms[i].Exits[0] = rooms[i + 1];

						// if next room is a side room, add the room ahead of the side room to function as the north room for that 'branch'
						if (isNextRoomSideRoom)
						{
							// ensure that the last room isn't a side room, that there is a room after the upcoming side room
							if (RoomAfterNextRoomExists)
							{
								rooms[i].Exits[1] = rooms[i + 2];
							}
						}
					}

					// connect to previous room (South)
					if (!isThisFirstRoom)
					{
						// if previous room is a side room, go back two
						if (isPreviousRoomSideRoom)
						{
							rooms[i].Exits[3] = rooms[i - 2];
						}
						else
						{
							rooms[i].Exits[3] = rooms[i - 1];
						}
					}
				}
				// if side room, connect back to the main path (West)
				else
				{
					rooms[i].Exits[0] = rooms[i - 1];
				}
			}

			return rooms;
		}

		// Items List 
		private List<Item> GenerateItems()
		{
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