using System.Collections.Generic;

namespace ZappaQuest
{
	public class Room
	{
		// DEV
		public string Name { get; }
		public string Description { get; }
		public Room[] Exits { get; }

		public bool IsDungeonExit { get; }

		public bool IsBonusRoom { get; }

		public int RoomIndex { get; set; }

		// List of Items for each room Created
		public List<Item> ItemsRoom { get; set; }

		private struct RoomDescription
		{
			public string RoomName { get; set; }
			public string RoomDescriptionText { get; set; }
		}

		// New room types for adventure game
		private static int RoomDescUsedCounter = 0;
		private static readonly RoomDescription[] RoomNamesAndDescriptions = new RoomDescription[]
		{
			new RoomDescription {
				RoomName = "Hot Rats Lair",
				RoomDescriptionText = "A sweltering chamber illuminated by glowing orange fungi. Massive rat skeletons hang from the ceiling, their bones fused into intricate jazz-like patterns. Strange instruments made of bone lie scattered about."
			},
			new RoomDescription {
				RoomName = "The Grand Wazoo's Throne Room",
				RoomDescriptionText = "An opulent circular chamber dominated by an absurdly large throne made of brass instruments. The walls are lined with arcane musical notation that seems to shift when not directly observed. Ghostly horns can be heard playing complex harmonies."
			},
			new RoomDescription {
				RoomName = "Apostrophe Apostasy",
				RoomDescriptionText = "A room where gravity works sideways. Yellow snow drifts cover one wall. A frozen-over hot tub contains what appears to be a small eskimo trapped in ice, eternally reaching for a nearby frozen sandwich."
			},
			new RoomDescription {
				RoomName = "Cosmik Debris Chamber",
				RoomDescriptionText = "A planetarium-like room where swirling cosmic dust forms bizarre constellations. Strange potions and mystical trinkets float in mid-air, defying gravity. A mysterious robed figure flickers in and out of existence in the corner."
			},
			new RoomDescription {
				RoomName = "Joe's Garage",
				RoomDescriptionText = "A mechanical workshop filled with bizarre contraptions. Steam-powered automatons mindlessly play broken instruments. In the center sits a sentient automotive engine that seems to speak through exhaust fumes and spark plugs."
			},
			new RoomDescription {
				RoomName = "St. Alfonzo's Pancake Breakfast",
				RoomDescriptionText = "A twisted parody of a chapel. The pews face a kitchen altar where spectral monks flip pancakes that transform into bizarre creatures when they land. The stained glass windows depict syrup bottles in religious poses."
			},
			new RoomDescription {
				RoomName = "Montana Banana Plantation",
				RoomDescriptionText = "A surreal agricultural chamber where tiny horses plow fields of dental floss. Bizarre banana trees grow upside-down from the ceiling, their fruit resembling tiny human hands that wave at visitors."
			},
			new RoomDescription {
				RoomName = "Zoot Allures Wardrobe",
				RoomDescriptionText = "A claustrophobic dressing room packed with flamboyant, sentient costumes that whisper critiques of your appearance. Mirrors on all sides show different versions of yourself wearing increasingly outlandish outfits."
			},
			new RoomDescription {
				RoomName = "The Torture Never Stops Dungeon",
				RoomDescriptionText = "A classic dungeon turned absurd. Torture devices play atonal jazz when activated. The walls are lined with cages containing philosophers engaged in eternal debates. The air smells of pumpkins and industrial waste."
			},
			new RoomDescription {
				RoomName = "Don't Eat The Yellow Snow Cave",
				RoomDescriptionText = "A frosty cavern with pristine white snowdrifts, except for one suspiciously yellow patch. Husky dogs carved from ice guard a central igloo where a spectral Nanook figure hovers, brandishing a spear made of frozen huskies."
			},
			new RoomDescription {
				RoomName = "Muffin Man's Bakery",
				RoomDescriptionText = "A nightmarish kitchen where gigantic muffins develop faces as they bake. Sentient utensils dance around mixing bowls filled with glowing batter. The baker himself is nowhere to be seen, but his laughter echoes from inside the stone oven."
			},
			new RoomDescription {
				RoomName = "Valley Girl Mallscape",
				RoomDescriptionText = "A twisted 1980s shopping mall frozen in time. Mannequins with valley girl expressions gossip amongst themselves and fall silent when noticed. Storefronts sell impossible merchandise that changes form when touched."
			},
			new RoomDescription {
				RoomName = "Watermelon in Easter Hay Greenhouse",
				RoomDescriptionText = "A serene glass chamber filled with impossible plants. Watermelons grow from crucifixes planted in hay-covered soil. The glass ceiling reveals an eternal sunset, and the faint sound of a mournful guitar solo echoes through the humid air."
			},
			new RoomDescription {
				RoomName = "Sheik Yerbouti's Harem",
				RoomDescriptionText = "An ornate chamber with silk-draped walls and incense-filled air. Animated throw rugs slither across the floor. Cushions shaped like desert creatures arrange and rearrange themselves around a hookah that bubbles with multicolored smoke."
			},
			new RoomDescription {
				RoomName = "Bobby Brown's Bathroom",
				RoomDescriptionText = "A gaudy 1970s bathroom with gold fixtures and disco balls. The mirrors show reflections that move independently. Sentient toiletries sing crude songs when touched, and the bathtub appears to be a portal to a subterranean nightclub."
			},
			new RoomDescription {
				RoomName = "Catholic Girls Chapel",
				RoomDescriptionText = "A twisted school chapel where the pews face in different directions. Ghostly schoolgirls giggle from behind pillars but vanish when approached. The confessional booth whispers scandalous secrets about people you've never met."
			},
			new RoomDescription {
				RoomName = "Camarillo Brillo Salon",
				RoomDescriptionText = "A psychedelic beauty salon where mirrors reflect impossible hairstyles. Electric combs and scissors hover in the air, occasionally snipping at nothing. The shampoo sinks contain swirling vortexes of fluorescent liquid that emits cryptic prophecies."
			},
			new RoomDescription {
				RoomName = "Central Scrutinizer's Office",
				RoomDescriptionText = "A bureaucratic nightmare of a room filled with filing cabinets that stretch beyond sight. A massive megaphone hangs from the ceiling, occasionally broadcasting absurd laws. Every surface is covered with regulations written in impossible scripts."
			},
			new RoomDescription {
				RoomName = "Willie the Pimp's Bordello",
				RoomDescriptionText = "A velvet-draped chamber of questionable taste. The furniture is shaped like bizarre anatomical features. A ghostly violin plays in the corner while a hat rack wearing a purple suit and feathered fedora slowly rotates by itself."
			},
			new RoomDescription {
				RoomName = "Inca Roads Observatory",
				RoomDescriptionText = "A prehistoric astronomical chamber with stone instruments pointed at the ceiling, which displays an alien sky. Star charts on the walls depict constellations in the shape of ancient spacecraft. The stone floor is engraved with mathematical formulas too advanced to comprehend."
			},
			new RoomDescription {
				RoomName = "Zombie Woof Kennel",
				RoomDescriptionText = "A dilapidated dog pound where the kennels contain shadowy canine figures that stand upright. Chew toys made of human bone litter the floor. A spectral dog catcher patrols with a net made of moonbeams, forever hunting something that howls from just beyond the walls."
			},
			new RoomDescription {
				RoomName = "Peaches En Regalia Ballroom",
				RoomDescriptionText = "An elegant dance hall where phantom orchestras play impossible time signatures. The dance floor is made of shifting fruit-colored tiles that change pattern with the music. Ornate regalia hanging on the walls occasionally joins the dance without wearers."
			},
			new RoomDescription {
				RoomName = "Project/Object Control Room",
				RoomDescriptionText = "A retrofuturistic command center with blinking consoles and mysterious screens showing scenes from throughout the dungeon. Everything seems interconnected by tubes and wires that pulse with conceptual energy. The centerpiece is a three-dimensional model of what might be the entire universe, or just a very ambitious electrical circuit."
			}
		};

		public Room(int index, bool isSideRoom, bool isDungeonExit, Room[] exits)
		{
			// build room name and description
			bool usedAllRoomDescriptions = RoomDescUsedCounter >= RoomNamesAndDescriptions.Length;
			if (!usedAllRoomDescriptions)
			{
				RoomDescription currentRoomDescription = RoomNamesAndDescriptions[RoomDescUsedCounter];
				RoomDescUsedCounter++;
				Name = currentRoomDescription.RoomName;
				Description = currentRoomDescription.RoomDescriptionText;
				if (isSideRoom)
				{
					Name += " (BONUS ROOM)";
					Description = "\n This is a BONUS ROOM!";
				}
			}
			else
			{
				// use generic description
				Name = "Bathroom";
				Description = "It's a big, stinky bathroom. There are many toilets and showers, in a wide variety of sizes, from comically small to worrying large.";
			}

			RoomIndex = index;
			Exits = exits;
			IsDungeonExit = isDungeonExit;
			// side rooms are bonus rooms... terminology differs based on if we are concerned about room position
			IsBonusRoom = isSideRoom;
			ItemsRoom = new List<Item>();
		}

		// print room description:
		public void PrintRoomDescription()
		{
			Console.WriteLine($"You are in the {Name}.");
			Console.WriteLine(Description);

			// TODO: print creatures
			PrintExits();
			PrintRoomItems();


			if (IsDungeonExit)
			{
				Console.WriteLine("\nThis is the Dungeon Exit.");
			}
		}

		public void PrintExits()
		{
			Console.WriteLine("There are these Exits: ");
			for (int i = 0; i < Exits.Length; i++)
			{

				if (Exits[i] != null)
				{
					String exitDisplayLine = "		";
					if (IsBonusRoom)
					{
						exitDisplayLine += " (EAST) ";
					}

					// add north and south for normal rooms
					// exits [1] is reserved for side / bonus rooms
					else if (Exits[1] == null)
					{
						if (i == 0 && Exits[0] != null)
						{
							exitDisplayLine += " (NORTH) ";
						}
						if (i == 3 && Exits[3] != null)
						{
							exitDisplayLine += " (SOUTH) ";
						}
					}

					// if we have three rooms ( attached bonus room case) bonus room will be the first in the exits array
					else
					{
						if (i == 0 && Exits[0] != null)
						{
							exitDisplayLine += " (WEST) ";
						}
						if (i == 1 && Exits[1] != null)
						{
							exitDisplayLine += " (NORTH) ";
						}
						if (i == 3 && Exits[3] != null)
						{
							exitDisplayLine += " (SOUTH) ";
						}
					}

					// add room name and print line
					exitDisplayLine += Exits[i].Name;
					Console.WriteLine(exitDisplayLine);
				}
			}

		}

		public void PrintRoomItems()
		{
			Console.WriteLine("Current Items in room:");
			foreach (var items in ItemsRoom)
			{
				Console.WriteLine($"		{items.Information()}");
			}
		}

		// Picking up Items in Room 
		public void PickUpItem(Frank player)
		{
			// Check if there are no items in room player is in 
			if (ItemsRoom.Count == 0)
			{
				Console.WriteLine("Unfortunately, there are no items in this room to pick up");
				return;
			}

			Console.WriteLine("Current Items in room:");
			for (int i = 0; i < ItemsRoom.Count; i++)
			{
				Console.WriteLine($"{i + 1}. {ItemsRoom[i].Information()}");
			}

			// Allow player to make choice, and accept int
			Console.WriteLine("Please enter which item you want to pick up: ");
			string choice = Console.ReadLine();

			if (int.TryParse(choice, out int selection))
			{
				if (selection >= 1 && selection <= ItemsRoom.Count)
				{
					Item selectItem = ItemsRoom[selection - 1];

					// Add item to player inventory
					bool addSuccess = player.PickUpItem(selectItem);
					if (addSuccess)
					{
						ItemsRoom.RemoveAt(selection - 1);
						Console.WriteLine($"You have successfully picked up: {selectItem.Information()}");
					}
					else
					{
						Console.WriteLine("inventory is full. You cannot pick up item.");
					}
				}
				else
				{
					Console.WriteLine("Invalid item number");
				}
			}
			else
			{
				Console.WriteLine("Sorry, I didn't get that. Please enter a number.");
			}
		} // end PickUpItem Method

		public void DropItem(Frank player) {
			if (player.Inventory.Count == 0) {
				Console.WriteLine("You do not have any items to drop.");
				return;
			}
			// Display current items in player inventory to choose which to drop
			Console.WriteLine("Current Items:");
			for (int i=0; i < player.Inventory.Count; i++) {
				Console.WriteLine($"{i+1}. {player.Inventory[i].Information()}");
			}
			// Ask player which item to drop
			Console.WriteLine("Which item would you like to drop from inventory?");
			Console.WriteLine("Be wise...");
			string choice = Console.ReadLine();

			if (int.TryParse(choice, out int selection)) {
				if (selection >= 1 && selection <= player.Inventory.Count) {
					Item selectItem = player.Inventory[selection - 1];
					player.DropItem(selectItem);
					ItemsRoom.Add(selectItem);
				}
				else {
					Console.WriteLine("Invalid selection. Please try again.");
				}
			}
			else {
				Console.WriteLine("Sorry, I didn't get that. Please enter a valid number.");
			}
		}

		public void Navigate(Frank player)
		{

			bool newRoomSelected = false;
			while (!newRoomSelected)
			{

				Console.WriteLine("Enter a selection for each exit you would like to take: ");
				// slightly modified version of print exits
				for (int i = 0; i < Exits.Length; i++)
				{

					if (Exits[i] != null)
					{
						String exitDisplayLine = "		";
						if (IsBonusRoom)
						{
							exitDisplayLine += "1. (EAST) ";
						}

						// add north and south for normal rooms
						// exits [1] is reserved for side / bonus rooms
						else if (Exits[1] == null)
						{
							if (i == 0 && Exits[0] != null)
							{
								exitDisplayLine += "1. (NORTH) ";
							}
							if (i == 3 && Exits[3] != null)
							{
								exitDisplayLine += "2. (SOUTH) ";
							}
						}

						// if we have three rooms ( attached bonus room case) bonus room will be the first in the exits array
						else
						{
							if (i == 0 && Exits[0] != null)
							{
								exitDisplayLine += "1. (WEST) ";
							}
							if (i == 1 && Exits[1] != null)
							{
								exitDisplayLine += "2. (NORTH) ";
							}
							if (i == 3 && Exits[3] != null)
							{
								exitDisplayLine += "3. (SOUTH) ";
							}
						}

						// add room name and print line
						exitDisplayLine += Exits[i].Name;
						Console.WriteLine(exitDisplayLine);
					}
				}

				// add back line
				int BackSelector = 0;
				if (IsBonusRoom)
				{
					BackSelector = 2;
				}
				else if (Exits[1] == null)
				{
					BackSelector = 3;
				}
				else
				{
					BackSelector = 4;
				}
				Console.WriteLine($"		{BackSelector}. BACK");

				// query user
				int exitSelection = GameInstance.TakeInput(BackSelector);

				// handle navigation differently based on room type
				// bonus room, only can go back to main room or exit the move loop
				if (IsBonusRoom)
				{
					if (exitSelection == 1)
					{
						// move frank's room index to the index of the exit
						player.CurrentRoomIndex = Exits[0].RoomIndex;
						newRoomSelected = true;
					}
					else if (exitSelection == 2)
					{
						// break loop
						newRoomSelected = true;
					}
					else
					{
						Console.WriteLine("Sorry, didn't quite get that.");
					}
				}
				// main north/south room, no side room
				else if (Exits[1] == null)
				{
					if (exitSelection == 1)
					{
						// north travel
						player.CurrentRoomIndex = Exits[0].RoomIndex;
						newRoomSelected = true;
					}
					else if (exitSelection == 2)
					{
						// south travel
						player.CurrentRoomIndex = Exits[3].RoomIndex;
						newRoomSelected = true;
					}
					else if (exitSelection == 3)
					{
						// break loop
						newRoomSelected = true;
					}
					else
					{
						Console.WriteLine("Sorry, didn't quite get that.");
					}
				}
				// room that do have side rooms, three options
				else
				{
					if (exitSelection == 1)
					{
						// west travel
						player.CurrentRoomIndex = Exits[0].RoomIndex;
						newRoomSelected = true;
					}
					else if (exitSelection == 2)
					{
						// north travel
						player.CurrentRoomIndex = Exits[1].RoomIndex;
						newRoomSelected = true;
					}
					else if (exitSelection == 3)
					{
						// south travel
						player.CurrentRoomIndex = Exits[3].RoomIndex;
						newRoomSelected = true;
					}
					else if (exitSelection == 4)
					{
						// break loop
						newRoomSelected = true;
					}
					else
					{
						Console.WriteLine("Sorry, didn't quite get that.");
					}
				}


			}


		}

	}
}