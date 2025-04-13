using System.Threading.Tasks.Dataflow;

namespace ZappaQuest
{
	public class GameInstance
	{


		public Boolean GAME_OVER = false;
		public String[] PlayerData = new String[2];
        //whether it's already displayed "(press any key to stop)"
        public bool LearnedDice = false;

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

			// generate loot and add as drops
			AddLootToDungeon(Dungeon);

			// DEV
			// loop through dungeon, printing exits, description, etc
			// foreach (var room in Dungeon)
			// {
			// 	room.PrintRoomDescription();
			// }


			// create player creature (Frank)
			Frank thePlayer = new Frank();

			// build room array
			while (!GAME_OVER)
			{
				// main game loop ( LOOP = TURN )
				Room currentRoom = Dungeon[thePlayer.CurrentRoomIndex];

				// print room description
				currentRoom.PrintRoomDescription();

				// create loop for actions in this room until we leave
				Boolean inSameRoom = true;

				while (inSameRoom)
				{

					// if room has creature(s), run combat loop before advancing:
					// combat loop

					// query action:
					Console.WriteLine("These are your options: \n1. TAKE EXIT");
					if (currentRoom.ItemsRoom.Count > 0)
					{
						Console.WriteLine("2. TAKE ITEM");
					}
					String initialTurnChoice = Console.ReadLine();

					if (initialTurnChoice == "1")
					{
						// call navigate on frank
						currentRoom.Navigate(thePlayer);
						// we moved on
						inSameRoom = false;
					}
					else if (initialTurnChoice == "2")
					{
						// DEV
						// TODO: item pickup
						Console.WriteLine("Woopsie, can't do that yet.");

						// room.PickupItem ( print options with in for each, accept int, move item from room list to player list)
					}
					else
					{
						Console.WriteLine("TRY AGAIN");
					}
				}
				// GAME_OVER = true;
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
			Console.WriteLine("\nIt's time to be Frank. Let's play ZAPPA QUEST!\n");
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
				Boolean isFirstRoom = (i == 0);
				Boolean isSideRoom = (i % 3 == 0) && !isFirstRoom;
				Boolean isLastRoom = (i + 1 == maxRooms);

				// isLastRoom passed into isDungeonExit so that lastRoom is exit
				rooms[i] = new Room(i, isSideRoom, isLastRoom, new Room[4]);
				// 4 possible exits: North, East, South, West. 
				// The indices in the exits array do not correspond to specific cardinal directions, as rooms can be 'rotated' 
			}

			// add exits
			for (int i = 0; i < maxRooms; i++)
			{
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
		// Add to Loot Section!!!!!!!!!!!!!

		private void AddLootToDungeon(Room[] Dungeon)
		{
			List<Item> items = GenerateItems();
			Random gen = new Random();

			// loop through rooms with a random chance to add an item
			foreach (var Room in Dungeon)
			{

				// bonus rooms are guaranteed to have one item
				if (Room.IsBonusRoom)
				{
					Room.ItemsRoom.Add(items[gen.Next(items.Count)]);
					// double normal drop rate chance for a bonus item
					if (gen.NextDouble() < 0.6)
					{
						Room.ItemsRoom.Add(items[gen.Next(items.Count)]);
					}
				}
				else
				{
					// 30% base chance to add item
					if (gen.NextDouble() < 0.3)
					{
						Room.ItemsRoom.Add(items[gen.Next(items.Count)]);
					}
				}

			}
		}

        //roll a "die" and have the user time the outcome
        public int DiceRoll(string Prompt, int Max = 20)
        {
            //remove any pending keys
            while (Console.KeyAvailable)
                Console.ReadKey(intercept: true);

            //create a random order of numbers
            Random Random = new Random();
            Range.EndAt(20);
            int[] RandomWheel = Enumerable.Range(1, Max).ToArray();
            Random.Shuffle(RandomWheel);
            int RandomPos = 0;

            //say what it's rolling for
            if (LearnedDice)
            {
                Console.WriteLine($"Rolling for {Prompt}: \n");
            }
            else
            {
                Console.WriteLine($"Rolling for {Prompt}: (press any key to stop) \n");
                LearnedDice = true;
            }

            //cycle until the user presses a key
            while (!Console.KeyAvailable)
            {
                RandomPos = (RandomPos + 1) % 20;
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.WriteLine($"> {RandomWheel[RandomPos],-2} <");
                Task.Delay(150).Wait();
            }
            Console.ReadKey(intercept: true);

            return RandomWheel[RandomPos];
        }

        // end class
    }
}