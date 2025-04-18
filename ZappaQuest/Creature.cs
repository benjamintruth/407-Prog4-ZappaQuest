namespace ZappaQuest
{

	public abstract class Creature
	{
		// The name of the creature
		public string Name { get; }
		// A short description of the creature
		public string Description { get; }
		// Hit points until the creature dies
		public int Health { get; set; }
		// The room the creature is in
		public int CurrentRoomIndex { get; set; }
		// The game it's in
		protected GameInstance CurrentGame;

		// weapon and armor the creature is using
		public Weapon EquippedWeapon;
		public Armor EquippedArmor;

		public Creature(string name, string description, int health, Weapon equippedWeapon, Armor equippedArmor, GameInstance currentGame)
		{
			Name = name;
			Description = description;
			Health = health;
			EquippedWeapon = equippedWeapon;
			EquippedArmor = equippedArmor;
			CurrentGame = currentGame;
		}

		//Function to return if an enemy is dead yet
		public bool isAlive()
		{
			return Health > 0;
		}

		//Take damage and reduce health when hit
		public void takesDamage(int amount)
		{
			Health -= amount;
		}

		//attack, returns true if the opponent dies
		public bool attack(Creature opponent)
		{
			//strike a certain number of times based on the weapon
			for (int i = 0; i < EquippedWeapon.NumAttacksPerTurn; i++)
			{
				//roll a die for power
				int HitChance = CurrentGame.DiceRoll($"{Name}'s {EquippedWeapon.Description} attack");
				if (HitChance > opponent.EquippedArmor.ProtectValue)
				{
					//The damage dealt is max damage divided by a random number between 1 and 20/the number you roll
					Random HitPowerRandomizer = new Random();
					int HitPower = EquippedWeapon.MaxDamage / HitPowerRandomizer.Next(1, (int)(20 / HitChance));
					if (EquippedWeapon.IsMagical)
					{
						HitPower = (int)(HitPower * 1.3);
					}
					//deal the damage
					opponent.takesDamage(HitPower);
					Console.WriteLine($"{Name} hit {opponent.Name} and dealt {HitPower} Damage!");
				}
				else
				{
					//Attack is blocked by armor, deal no damage
					Console.WriteLine($"{opponent.Name}'s {opponent.EquippedArmor.Description} blocked it!");
				}

				//Check if the opponent is still alive every strike
				if (!opponent.isAlive())
				{
					Console.WriteLine($"{Name} defeated {opponent.Name}!");
					return true;
				}
			}
			//The opponent did not die
			return false;
		}
	}


	// special subclass for player character
	public class Frank : Creature
	{
		//all your non-weapon non-armor possessions
		public List<Item> Inventory { get; set; }

		//default Frank values
		public Frank(GameInstance currentGame) : base(
			name: "Frank Zappa",
			description: "the man himself",
			health: 100,
			equippedWeapon: new Weapon(
				description: "Modified SG Guitar with Custom Electronics",
				isMagical: true,
				attacksPerTurn: 2,
				maxDamage: 2,
				isRemovable: true
			),
			equippedArmor: new Armor("skin", false, 0, true),
			currentGame: currentGame
		)
		{
			CurrentRoomIndex = 0;
			Inventory = new List<Item>();
		}

		//fighting enemies
		public void fight(Enemy opponent)
		{
			//Display a random battle initiation message
			Random randomText = new Random();
			String[] battleMessages = {
				$"{opponent.Name} flies at you in a rage!",
				$"{opponent.Name} decides your time is up!",
				$"{opponent.Name} doesn't like your music!",
				$"{opponent.Name} thinks Frank is a dumb name!",
				$"{opponent.Name} called you weak! You'll show them!",
				$"{opponent.Name} smells bad and you've had enough!",
				$"{opponent.Name} is egging for a fight!",
				$"{opponent.Name} initiates a battle!",
				$"{opponent.Name} wants to end your career!",
				$"{opponent.Name} is suddenly consumed by rage!",
				$"{opponent.Name} disagrees with you on key social issues!",
				$"{opponent.Name} decided violence IS the answer!",
				$"{opponent.Name} charges at you!",
				$"{opponent.Name} decides the fist is mightier than the pen!",
				$"{opponent.Name} is insulted by your existence!",
				$"You've had enough of {opponent.Name}!",
			};
			int randomTextInt = randomText.Next(battleMessages.Length);
			Console.WriteLine(battleMessages[randomTextInt]);

			//loop until returning
			while (true)
			{
				//heal every turn from the power of jazz
				for (int i = 0; i < Inventory.Count; i++)
				{
					if (Inventory[i] is MagicItem)
					{
						Console.WriteLine($"{Name} healed {((MagicItem)Inventory[i]).JazzPower} from {Inventory[i].Description}");
						Health = Health + ((MagicItem)Inventory[i]).JazzPower;
					}
				}

				//attack the monster, attack() returns if they died
				if (attack(opponent))
				{
					// take their gear if possible
					stealFrom(opponent);
					// remove the opponent from the game
					CurrentGame.InstanceDungeon[CurrentRoomIndex].EnemiesRoom.Remove(opponent);
					return;
				}
				//monster attacks you
				else if (opponent.attack(this))
				{
					//you died, oof
					CurrentGame.GAME_OVER = true;
					return;
				}
			}
		}

		//critique your choices whenever you take a weapon or armor
		public void judgeStealing(Item choice, Item reject, Item choice2 = null, Item reject2 = null)
		{
			//judging two at once, compare two and then recurse
			if (choice2 is not null)
			{
				if (choice2.CompareTo(reject2) == -1)
				{
					judgeStealing(choice, reject);
				}
			}
			else
			{
				//compare each item to the other
				if (choice.CompareTo(reject) == -1)
					Console.WriteLine("Maybe not the best choice");
				else
					Console.WriteLine("Probably a good idea");
			}
		}

		//take an item and add it to your inventory, possibly
		public bool PickUpItem(Item item)
		{
			if (Inventory.Count >= 10)
			{
				return false;
			}
			Inventory.Add(item);
			return true;
		}

		//drop an item from your inventory onto the floor
		public void DropItem(Item item)
		{
			//you don't own anything
			if (item == null)
			{
				Console.WriteLine("There are no items to drop.");
				return;
			}
			if (Inventory.Contains(item))
			{
				//remove from inventory
				Inventory.Remove(item);
				Console.WriteLine($"You have dropped: {item.Information()} from Inventory.");
			}
		}

		//rest in a save room
		public void RestPlayer()
		{
			//randomly decide health
			int restHealing = new Random().Next(1, 6);

			Health += restHealing;
			// Maximum health from resting set to 100
			if (Health > 100)
			{
				Health = 100;
			}

			Console.WriteLine($"You are resting and restored {restHealing} HP. Your health is now: {Health}");
		}

		//view your posessions
		public void ViewInventory()
		{
			Console.WriteLine("---INVENTORY---");
			Console.WriteLine("Your Inventory Contains: ");
			if (Inventory.Count == 0)
			{
				Console.WriteLine("You are not carrying anything...");
			}
			else
			{
				for (int i = 0; i < Inventory.Count; i++)
				{
					Console.WriteLine($"{i + 1}. {Inventory[i].Description}");
				}
			}
			//see your weapon and armor too
			Console.WriteLine("Equipped Weapon: ");
			Console.WriteLine($"		{this.EquippedWeapon.Information()}");
			Console.WriteLine("Equipped Armor");
			Console.WriteLine($"		{this.EquippedArmor.Information()}");
			Console.WriteLine("----------------\n");
		}

		//loot a corpse
		private void stealFrom(Creature opponent)
		{
			//weapon and armor are both stealable
			if (opponent.EquippedWeapon.IsRemovable && opponent.EquippedArmor.IsRemovable)
			{
				Console.WriteLine($"You can take the {opponent.Name}'s:\n" +
					$"1. {opponent.EquippedWeapon.Description} with {opponent.EquippedWeapon.NumAttacksPerTurn} attacks, {opponent.EquippedWeapon.MaxDamage} strength) (1)\n" +
					$"2. {opponent.EquippedArmor.Description} with {opponent.EquippedArmor.ProtectValue} defense (2)\n" +
					"3. Don't take anything");

				// query user
				int choice = GameInstance.TakeInput(3);

				//take weapon
				if (choice == 1)
				{
					Console.WriteLine($"You took the {opponent.EquippedWeapon.Description}.");
					judgeStealing(opponent.EquippedWeapon, EquippedWeapon);
					EquippedWeapon = opponent.EquippedWeapon;
				}
				//take armor
				else if (choice == 2)
				{
					Console.WriteLine($"You took the {opponent.EquippedArmor.Description}.");
					judgeStealing(opponent.EquippedArmor, EquippedArmor);
					EquippedArmor = opponent.EquippedArmor;
				}
				//take neither
				else if (choice == 3)
				{
					judgeStealing(EquippedWeapon, opponent.EquippedWeapon, EquippedArmor, opponent.EquippedArmor);
				}

				//only weapon is stealable
			}
			else if (opponent.EquippedWeapon.IsRemovable)
			{
				Console.WriteLine($"You can take the {opponent.Name}'s:\n" +
				$"1. {opponent.EquippedWeapon.Description} with {opponent.EquippedWeapon.NumAttacksPerTurn} attacks, {opponent.EquippedWeapon.MaxDamage} strength)\n" +
				"2. Don't take anything");

				// query user
				int choice = GameInstance.TakeInput(2);

				//take weapon
				if (choice == 1)
				{
					Console.WriteLine($"You took the {opponent.EquippedWeapon.Description}.");
					judgeStealing(opponent.EquippedWeapon, EquippedWeapon);
					EquippedWeapon = opponent.EquippedWeapon;
				}
				//take neither
				else if (choice == 2)
				{
					judgeStealing(EquippedWeapon, opponent.EquippedWeapon);
				}
			}

			//only armor is stealable
			else if (opponent.EquippedArmor.IsRemovable)
			{
				Console.WriteLine($"You can take the {opponent.Name}'s:\n" +
				$"1. {opponent.EquippedArmor.Description} with {opponent.EquippedArmor.ProtectValue} defense\n" +
				"2. Don't take anything");

				// query user
				int choice = GameInstance.TakeInput(2);

				//take armor
				if (choice == 1)
				{
					Console.WriteLine($"You took the {opponent.EquippedArmor.Description}.");
					judgeStealing(opponent.EquippedArmor, EquippedArmor);
					EquippedArmor = opponent.EquippedArmor;
				}
				//take neither
				else if (choice == 2)
				{
					judgeStealing(EquippedArmor, opponent.EquippedArmor);
				}
			}
			//can't take anything
			else
			{
				Console.WriteLine("There's nothing you were able to take");
			}
			return;
		}
	}

	//foes, which can be aggressive or not
	public class Enemy : Creature
	{
		public bool Aggressive;

		public Enemy(string name, string description, int health, Weapon equippedWeapon, Armor equippedArmor, GameInstance currentGame, bool aggressive)
		: base(name, description, health, equippedWeapon, equippedArmor, currentGame)
		{
			Aggressive = aggressive;
		}
	}

}