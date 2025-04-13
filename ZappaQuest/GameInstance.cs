using System.Threading.Tasks.Dataflow;

namespace ZappaQuest
{
	public class GameInstance
	{


		public bool GAME_OVER = false;
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
			Frank thePlayer = new Frank(this);

			// build room array
			while (!GAME_OVER)
			{
				// main game loop ( LOOP = TURN )
				Room currentRoom = Dungeon[thePlayer.CurrentRoomIndex];

				// print room description
				currentRoom.PrintRoomDescription();

				// create loop for actions in this room until we leave
				bool inSameRoom = true;

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
					if (thePlayer.Inventory.Count > 0) {
						Console.WriteLine("3. DROP ITEM");
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
						//Console.WriteLine("Woopsie, can't do that yet.");
						currentRoom.PickUpItem(thePlayer);
						// room.PickupItem ( print options with in for each, accept int, move item from room list to player list)
					}
					else if (initialTurnChoice == "3") {
						currentRoom.DropItem(thePlayer);
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
				bool isFirstRoom = (i == 0);
				bool isSideRoom = (i % 3 == 0) && !isFirstRoom;
				bool isLastRoom = (i + 1 == maxRooms);

				// isLastRoom passed into isDungeonExit so that lastRoom is exit
				rooms[i] = new Room(i, isSideRoom, isLastRoom, new Room[4]);
				// 4 possible exits: North, East, South, West. 
				// The indices in the exits array do not correspond to specific cardinal directions, as rooms can be 'rotated' 
			}

			// add exits
			for (int i = 0; i < maxRooms; i++)
			{
				bool isThisFirstRoom = i == 0;
				bool isThisSecondRoom = i == 1;
				// first room cannot be a side room bc a side room must be ahead of the room it connects to
				bool isSideRoom = (i) % 3 == 0 && !isThisFirstRoom;
				bool isThisLastRoom = i + 1 == maxRooms;
				bool isNextRoomSideRoom = (i + 1) % 3 == 0;
				// next in spot in the array plus one for the floor 0 conversion 
				bool isPreviousRoomSideRoom = (i - 1) % 3 == 0 && !(isThisFirstRoom || isThisSecondRoom);
				// two ahead plus one additional to convert from the floor being 0
				bool RoomAfterNextRoomExists = !(i + 3 > maxRooms);

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
				new Weapon("Stink Foot's Heavy Guitar", false, 2, 25, true),
				new Armor("Goblin Girl Suit", false, 12, true),
				new Treasure("Black Napkin", false, 50),
				new Food("Peaches En Regalia Smoothie", false, 24),
				new MagicItem("Joe's Garage Glitter Amulet", 77),
				new Weapon("Valley Girl Microphone", false, 3, 13, true),
				new Armor("Cosmic Debris Chest Armor", false, 8, true),
				new Treasure("Apostrophe' Shiny Vinyl", false, 70),
				new Food("Muffin from Reasearch Laboratory", false, 20),
				new MagicItem("Yellow Frozen Snow Cone", 50),
				new Food("Easter Hay Watermelon", false, 50),
				new Weapon("Zomby Woof Fangs", false, 4, 10, true),
				new Armor("I'm The Slime Shield", false, 15, true),
				new Treasure("FREAK OUT! Deluxe LP", true, 150),
				new Food("Pojama People Pudding", true, 20),
				new MagicItem("Uncle Remus Jazz Cube", 70),
				new Weapon("Inca Roads Laser Beam", true, 1, 60, false),
				new Armor("Plastic People Chestplate", false, 7, true),
				new Treasure("We're Only in it For The Money Coins!", false, 90),
				new Food("Call Any Vegetable Salad", false, 18)
			};
		}

		private List<Creature> GenerateCreatures()
		{
			List<Creature> CreatureList = new List<Creature> {
		new Creature(
			name: "Cosmic Debris",
			description: "A swirling mass of psychedelic space junk that dances erratically",
			health: 50,
			equippedWeapon: new Weapon("Rock Hands", true, 1, 20, true),
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Yellow Snow Leopard",
			description: "A growling husky with suspiciously yellow-tinted fur",
			health: 35,
			equippedWeapon: new Weapon("Teeth", true, 1, 20, true),
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Bobby Brown",
			description: "A preppy antagonist with an inflated ego and questionable morals",
			health: 45,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Muffin Man",
			description: "A deranged baker covered in flour with sinister intentions",
			health: 40,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Camarillo Brillo",
			description: "A witch with hair that glows in the dark and mysterious powers",
			health: 55,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Hot Rat",
			description: "A fiery rodent with jazz-influenced attack patterns",
			health: 25,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Montana Banana Farmer",
			description: "A delusional agriculturist with a tiny horse and dental floss obsession",
			health: 30,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Duke Of Prunes",
			description: "idk.. he's like a prune guy... he's big",
			health: 60,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Central Scrutinizer",
			description: "A mechanical enforcer of social and musical conformity",
			health: 70,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Lumpy Gravy Beast",
			description: "An amorphous blob of sentient, experimental musical arrangements",
			health: 45,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Zomby Woof",
			description: "A reanimated canine with an impressive vocal range",
			health: 50,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Dirty Love Creature",
			description: "A pungent entity seeking questionable relations with household appliances",
			health: 35,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Illinois Enema Bandit",
			description: "A notorious criminal with unconventional methods and a distinctive laugh",
			health: 45,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "Dinah-Moe Humm",
			description: "A smug challenger who never loses her bizarre betting games",
			health: 40,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		),
		new Creature(
			name: "G-Spot Tornado",
			description: "A whirling vortex of impossible rhythms and unplayable notation",
			health: 65,
			equippedWeapon: null,
			equippedArmor: null,
			currentGame: this
		)
	};

			// add 3 random pieces of magical equipment to random monsters


			return CreatureList;
		}

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

		private void AddCreaturesToDungeon(Room[] Dungeon) { }

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

		public static int TakeInput(int max)
		{
			string input = Console.ReadLine();
			int value;
			while (input.Length > 1 || int.TryParse(input, out value) && (value <= 0 || value > max))
			{
				Console.Write("Please enter a valid input");
				input = Console.ReadLine();
			}
			return value;
		}

		// end class
	}
}