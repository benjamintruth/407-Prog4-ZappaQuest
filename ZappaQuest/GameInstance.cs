namespace ZappaQuest
{
	public class GameInstance
	{
		// whether you've either won or died
		public bool GAME_OVER = false;
		// the player's name and difficulty level
		// name is used during the ending, difficulty determines number of enemies and rooms
		public String[] PlayerData = new String[2];
		// whether you've seen the key prompt on dice rolls before
		public bool LearnedDice = false;
		// the list of rooms in the game dungeon
		public Room[] InstanceDungeon;

		//start the game
		public void Initialize()
		{
			Console.WriteLine("WELCOME 2 ZAPPA QUEST!");
			PlayerData = GreetPlayer();
			RunGame();
		}

		// the main game script
		private void RunGame()
		{

			// parse difficulty level
			int DifficultyLevel = Int32.Parse(PlayerData[1]);

			// create a number of rooms based on difficulty level
			Room[] Dungeon = BuildRooms(DifficultyLevel * 2);
			// attach dungeon to game instance
			InstanceDungeon = Dungeon;

			// generate loot and add as drops
			AddLootToDungeon(Dungeon);

			// Add enemies to dungeons
			AddCreaturesToDungeon(Dungeon);

			// create player creature (Frank)
			Frank thePlayer = new Frank(this);

			// main loop until the game is over
			while (!GAME_OVER)
			{
				// main game loop ( LOOP = TURN )
				Room currentRoom = Dungeon[thePlayer.CurrentRoomIndex];

				// print room description
				currentRoom.PrintRoomDescription();
				Console.WriteLine("\n");

				// create loop for actions in this room until we leave
				bool inSameRoom = true;
				while (inSameRoom)
				{
					// if room has aggressive enemy, auto-fight enemy
					if (currentRoom.EnemiesRoom.Count > 0)
					{
						Enemy roomEnemy = currentRoom.EnemiesRoom[0];
						if (roomEnemy.Aggressive)
						{
							thePlayer.fight(roomEnemy);
						}
					}

					// check if frank is dead
					if (!thePlayer.isAlive())
					{
						GAME_OVER = true;
						// have to end the 'in same room' loop to end the game
						inSameRoom = false;
						Console.WriteLine("You Lose! You were not good at being Frank! Frank is dead! Not Groovy! GG!");
					}
					else if (currentRoom.IsDungeonExit)
					{
						GAME_OVER = true;
						inSameRoom = false;
						Console.WriteLine($"Congratulations {PlayerData[0]}! You've reached the end of your journey through ZappaQuest. Your adventure has come to a successful conclusion. You win!");
					}
					// decide what to do in the room 
					else
					{
						// Set option to 1st option 
						int listOption = 1;
						Dictionary<int, string> option = new Dictionary<int, string>();

						// generate list of options for user to select conditonally
						Console.WriteLine("These are your options:");
						option[listOption++] = "TAKE EXIT";
						if (currentRoom.ItemsRoom.Count > 0)
						{
							option[listOption++] = "TAKE ITEM";
						}
						if (thePlayer.Inventory.Count > 0)
						{
							option[listOption++] = "DROP ITEM";
						}
						if (thePlayer.Inventory.OfType<Food>().Any())
						{
							option[listOption++] = "EAT FOOD";
						}
						if (currentRoom.EnemiesRoom.Count > 0)
						{
							option[listOption++] = "FIGHT ENEMY";
						}
						option[listOption++] = "REST";
						option[listOption++] = "VIEW INVENTORY";

						//print each option and each key
						foreach (var entry in option)
						{
							Console.WriteLine($"{entry.Key}. {entry.Value}");
						}

						//take a key for the action
						int selectOption = TakeInput(listOption);
						switch (option[selectOption])
						{
							//leave the room
							case "TAKE EXIT":
								currentRoom.Navigate(thePlayer);
								inSameRoom = false;
								break;
							//collect an item
							case "TAKE ITEM":
								if (currentRoom.EnemiesRoom.Count > 0)
								{
									Console.WriteLine($"You try to take the item, but the {currentRoom.EnemiesRoom[0].Name} stops you!");
									thePlayer.fight(currentRoom.EnemiesRoom[0]);
								}
								else
								{
									currentRoom.PickUpItem(thePlayer);
								}
								break;
							//leave an item
							case "DROP ITEM":
								currentRoom.DropItem(thePlayer);
								break;
							//eat food
							case "EAT FOOD":
								currentRoom.EatFood(thePlayer);
								break;
							//battle any nonaggressive enemies
							case "FIGHT ENEMY":
								thePlayer.fight(currentRoom.EnemiesRoom[0]);
								break;
							//take a break
							case "REST":
								thePlayer.RestPlayer();
								break;
							//observe your wares
							case "VIEW INVENTORY":
								thePlayer.ViewInventory();
								break;
						}
					}
				}
			}
		}

		//start the game by asking some info
		private String[] GreetPlayer()
		{
			String[] PlayerData = new String[2];

			Console.WriteLine("What is your name?");
			PlayerData[0] = Console.ReadLine();
			Console.WriteLine($"Nice to meet you, {PlayerData[0]}. Are you up for a challenge? (yes/no)");
			String PlayerResponse = Console.ReadLine();
			// Create the difficulty based on their response
			switch (PlayerResponse)
			{
				case "yes":
				case "y":
				case "ok":
				case "alright":
				case "sure":
				case "absolutely":
					PlayerData[1] = "20";
					break;
				case "no":
				case "n":
					PlayerData[1] = "5";
					break;
				case "maybe":
				case "m":
				case "perhaps":
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

		//populate the dungeon with rooms
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

		// Returns a prebuilt list of every item
		private List<Item> GenerateItems()
		{
			return new List<Item> {
				new Weapon("Stink Foot's Heavy Guitar", false, 2, 25, true),
				new Armor("Goblin Girl Suit", false, 12, true),
				new Treasure("Black Napkin", false, 50),
				new Food("Peaches En Regalia Smoothie", false, 24),
				new MagicItem("Joe's Garage Glitter Amulet", 6),
				new Weapon("Valley Girl Microphone", false, 3, 13, true),
				new Armor("Cosmic Debris Chest Armor", false, 8, true),
				new Treasure("Apostrophe' Shiny Vinyl", false, 70),
				new Food("Muffin from Reasearch Laboratory", false, 20),
				new MagicItem("Yellow Frozen Snow Cone", 5),
				new Food("Easter Hay Watermelon", false, 50),
				new Weapon("Zomby Woof Fangs", false, 4, 10, true),
				new Armor("I'm The Slime Shield", false, 15, true),
				new Treasure("FREAK OUT! Deluxe LP", true, 150),
				new Food("Pojama People Pudding", true, 20),
				new MagicItem("Uncle Remus Jazz Cube", 7),
				new Weapon("Inca Roads Laser Beam", true, 1, 60, false),
				new Armor("Plastic People Chestplate", false, 7, true),
				new Treasure("We're Only in it For The Money Coins!", false, 90),
				new Food("Call Any Vegetable Salad", false, 18)
			};
		}

		// returns a prebuilt list of every enemy in the game
		private List<Enemy> GenerateCreatures()
		{
			return new List<Enemy> {
				new Enemy(
					name: "Cosmic Debris",
					description: "A swirling mass of psychedelic space junk that dances erratically",
					health: 50,
					equippedWeapon: new Weapon("Rock Hands", true, 1, 20, false),
					equippedArmor: new Armor("Rock Armor", false, 12, false),
					currentGame: this,
					aggressive: true
				),
				new Enemy(
					name: "Yellow Snow Leopard",
					description: "A growling husky with suspiciously yellow-tinted fur",
					health: 35,
					equippedWeapon: new Weapon("Teeth", true, 1, 20, false),
					equippedArmor: new Armor("Fur", false, 3, false),
					currentGame: this,
					aggressive: true
				),
				new Enemy(
					name: "Bobby Brown",
					description: "A preppy antagonist with an inflated ego and questionable morals",
					health: 45,
					equippedWeapon: new Weapon("hands", false, 1, 2, false),
					equippedArmor: new Armor("skin", false, 3, false),
					currentGame: this,
					aggressive: true
				),
				new Enemy(
					name: "Muffin Man",
					description: "A deranged baker covered in flour with sinister intentions",
					health: 40,
					equippedWeapon: new Weapon("hands", false, 1, 3, false),
					equippedArmor: new Armor("muffin flesh", false, 3, false),
					currentGame: this,
					aggressive: true
				),
				new Enemy(
					name: "Camarillo Brillo",
					description: "A witch with hair that glows in the dark and mysterious powers",
					health: 55,
					equippedWeapon: new Weapon("Magic Missile Spell", true, 3, 3, true),
					equippedArmor: new Armor("Margic Robe", true, 3, true),
					currentGame: this,
					aggressive: false
				),
				new Enemy(
					name: "Hot Rat",
					description: "A fiery rodent with jazz-influenced attack patterns",
					health: 25,
					equippedWeapon: new Weapon("Teeth", true, 1, 5, false),
					equippedArmor: new Armor("skin", false, 3, false),
					currentGame: this,
					aggressive: true
				),
				new Enemy(
					name : "Farmer Banana",
					description: "A delusional agriculturist with a tiny horse and dental floss obsession",
					health: 30,
					equippedWeapon: new Weapon("Farmer Banana Scythe", true, 1, 20, true),
					equippedArmor: new Armor("skin", false, 3, false),
					currentGame: this,
					aggressive: false
				),
				new Enemy(
					name: "Duke Of Prunes",
					description: "idk.. he's like a prune guy... he's big",
					health: 60,
					equippedWeapon: new Weapon("Prune Hammer", true, 1, 20, true),
					equippedArmor: new Armor("skin", false, 3, false),
					currentGame: this,
					aggressive: true
				),
				new Enemy(
					name: "Central Scrutinizer",
					description: "A mechanical enforcer of social and musical conformity",
					health: 70,
					equippedWeapon: new Weapon("Robo Teeth", true, 1, 20, false),
					equippedArmor: new Armor("skin", false, 3, false),
					currentGame: this,
					aggressive: true
				),
				new Enemy(
					name: "Lumpy Gravy Beast",
					description: "An amorphous blob of sentient, experimental musical arrangements",
					health: 45,
					equippedWeapon: new Weapon("Gravy Teeth", true, 1, 15, false),
					equippedArmor: new Armor("skin", false, 3, false),
					currentGame: this,
					aggressive: false
				),
				new Enemy(
					name: "Zomby Woof",
					description: "A reanimated canine with an impressive vocal range",
					health: 50,
					equippedWeapon: new Weapon("Zombie Teeth", true, 1, 15, false),
					equippedArmor: new Armor("skin", false, 3, false),
					currentGame: this,
					aggressive: true
				),
				new Enemy(
					name: "Dirty Love Creature",
					description: "A pungent entity seeking questionable relations with household appliances",
					health: 35,
					equippedWeapon: new Weapon("Sticky Teeth", true, 1, 15, false),
					equippedArmor: new Armor("goo armor", false, 3, false),
					currentGame: this,
					aggressive: true
				),
				new Enemy(
					name: "Illinois Enema Bandit",
					description: "A notorious criminal with unconventional methods and a distinctive laugh",
					health: 45,
					equippedWeapon: new Weapon("Chicago Knife", true, 3, 15, true),
					equippedArmor: new Armor("skin", false, 3, false),
					currentGame: this,
					aggressive: true
				),
				new Enemy(
					name: "Dinah-Moe Humm",
					description: "A smug challenger who never loses her bizarre betting games",
					health: 40,
					equippedWeapon: new Weapon("Gambling... stick.. schileleigh", true, 2, 20, true),
					equippedArmor: new Armor("skin", false, 3, false),
					currentGame: this,
					aggressive: false
				),
				new Enemy(
					name: "G-Spot Tornado (MUSIC BASED)",
					description: "A whirling vortex of impossible rhythms and unplayable notation",
					health: 65,
					equippedWeapon: new Weapon("Gambling... stick.. schileleigh", true, 10, 3, false),
					equippedArmor: new Armor("skin", false, 3, false),
					currentGame: this,
					aggressive: true
				)
			};
		}

		//populate the rooms with loot
		private void AddLootToDungeon(Room[] Dungeon)
		{
			//get the item list
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

		//populate the dungeon with creatures
		private void AddCreaturesToDungeon(Room[] Dungeon)
		{
			// new random object
			Random gen = new Random();
			// number of creatures = difficulty score
			int DifficultyLevel = Int32.Parse(PlayerData[1]);

			// Create a creatures pool, lengthen it if there are more rooms than enemies
			List<Enemy> creaturesList = GenerateCreatures();
			int startcount = creaturesList.Count;
			int repeats = (DifficultyLevel / creaturesList.Count);
			for (int i = 0; i < repeats; i++)
			{
				creaturesList.AddRange(GenerateCreatures());
			}
			Enemy[] creatures = creaturesList.ToArray();
			//randomize the list
			gen.Shuffle(creatures);

			// generate a random set of rooms to put enemies in
			int[] RandomRooms = Enumerable.Range(1, Dungeon.Length - 1).ToArray();
			gen.Shuffle(RandomRooms);


			for (int i = 0; i < DifficultyLevel; i++)
			{
				//go through the first 50% of random rooms and add an enemy
				Dungeon[RandomRooms[i]].EnemiesRoom.Add(creatures[i]);
			}
		}

		//roll a "die" and have the user time the outcome
		public int DiceRoll(string Prompt, int Max = 20)
		{

			//create a random order of numbers
			Random Random = new Random();
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

			//cycle the random wheel until the user presses a key
			while (!Console.KeyAvailable)
			{
				RandomPos = (RandomPos + 1) % 20;
				Console.SetCursorPosition(0, Console.CursorTop - 1);
				Console.WriteLine($"> {RandomWheel[RandomPos],-2} <");
				Task.Delay(150).Wait();
			}
			Console.ReadKey(intercept: true);

			//output the resulting number
			return RandomWheel[RandomPos];
		}

		//take an integer input within a range
		public static int TakeInput(int max)
		{
			string input = Console.ReadLine();
			int value;
			//input must be o char, a number, and within range, otherwise ask again
			while (input.Length > 1 || !(Int32.TryParse(input, out value)) || (value <= 0 || value > max))
			{
				Console.WriteLine("Please enter a valid input");
				input = Console.ReadLine();
			}
			return value;
		}

	}
}