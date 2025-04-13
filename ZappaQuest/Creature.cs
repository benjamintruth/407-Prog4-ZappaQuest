//something

using System.Runtime.InteropServices.Swift;

namespace ZappaQuest
{

	public class Creature
	{
		// add all base creature attributes:
		public string Name { get; }
		public string Description { get; }
		public int Health { get; set; }
        public int CurrentRoomIndex { get; set; }
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

		public bool isAlive()
		{
			return Health > 0;
		}

		public void takesDamage(int amount)
		{
			Health -= amount;
		}
    }

	// special subclass for player character
	public class Frank : Creature
    {
        public List<Item> Inventory { get; set; }

		public Frank(GameInstance currentGame) : base(
			name: "Frank Zappa",
			description: "the man himself",
			health: 100,
			equippedWeapon: new Weapon("hands", false, 1, 2, false),
			equippedArmor: new Armor("skin", false, 0, false),
			currentGame: currentGame
        )
        {
            CurrentRoomIndex = 0;
			Inventory = new List<Item>();
		}

        //fighting (needs work)
        public void fight(Creature opponent)
        {
            //attack a certain number of times based on the weapon
            for (int i = 0; i < EquippedWeapon.NumAttacksPerTurn; i++)
            {
                //roll a die for power
                int HitChance = CurrentGame.DiceRoll($"{Name}'s {EquippedWeapon.Description} attack");
                if (HitChance > opponent.EquippedArmor.ProtectValue)
                {
                    Random HitPowerRandomizer = new Random();
                    int HitPower = EquippedWeapon.MaxDamage / HitPowerRandomizer.Next(1, (int)20 / HitChance);
                    opponent.takesDamage(HitPower);
                    Console.WriteLine($"{Name} hit {opponent.Name} and dealt ");
                }
                else
                {
                    Console.WriteLine($"{opponent.Name}'s {opponent.EquippedArmor.Description} blocked it!");
                }
            }
            if (!opponent.isAlive())
            {
                Console.WriteLine($"{Name} defeated {opponent.Name}!");
                stealFrom(opponent);
                return;
            }
        }

        private void stealFrom(Creature opponent)
		{
			//weapon and armor are both stealable
			if (opponent.EquippedWeapon.IsRemovable && opponent.EquippedArmor.IsRemovable)
			{
				Console.WriteLine($"You can take the {opponent.Name}'s:\n" +
					$"{opponent.EquippedWeapon.Description} with {opponent.EquippedWeapon.NumAttacksPerTurn} attacks, {opponent.EquippedWeapon.MaxDamage} strength) (1)" +
					$"{opponent.EquippedArmor.Description} with {opponent.EquippedArmor.ProtectValue} defense (2)" +
					"or don't (3)");

				// query user
				String choice = Console.ReadLine();

				//take weapon
				if (choice == "1")
				{
					EquippedWeapon = opponent.EquippedWeapon;
                    Console.WriteLine($"You took the {opponent.EquippedWeapon.Description}.");

					//todo: replace judging with a function probably
                    if (EquippedWeapon.CompareTo(opponent.EquippedWeapon) == 1)
                        Console.WriteLine("Probably a good idea");
                    else
                        Console.WriteLine("Maybe not the best choice");
                }
                //take armor
                else if (choice == "2")
				{
					EquippedArmor = opponent.EquippedArmor;
					Console.WriteLine($"You took the {opponent.EquippedArmor.Description}.");

                    //todo: replace judging with a function probably
                    if (EquippedArmor.CompareTo(opponent.EquippedArmor) == 1)
                        Console.WriteLine("Probably a good idea");
                    else
                        Console.WriteLine("Maybe not the best choice");
                }
                //take neither
                else if (choice == "3")
				{
                    //todo: replace judging with a function probably
                    if (EquippedWeapon.CompareTo(opponent.EquippedWeapon) < 1 && EquippedArmor.CompareTo(opponent.EquippedArmor) < 1)
						Console.WriteLine("Probably a good idea");
					else
						Console.WriteLine("Maybe not the best choice");
				}

			//only weapon is stealable
			}
			else if (opponent.EquippedWeapon.IsRemovable)
			{
				Console.WriteLine($"You can take the {opponent.Name}'s:\n" +
				$"{opponent.EquippedWeapon.Description} with {opponent.EquippedWeapon.NumAttacksPerTurn} attacks, {opponent.EquippedWeapon.MaxDamage} strength) (1) +" +
				"or don't (2)'");

				// query user
				String choice = Console.ReadLine();

				if (choice == "1")
				{
					EquippedWeapon = opponent.EquippedWeapon;
                    Console.WriteLine($"You took the {opponent.EquippedWeapon.Description}.");

                    if (EquippedWeapon.CompareTo(opponent.EquippedWeapon) == 1)
                        Console.WriteLine("Probably a good idea");
                    else
                        Console.WriteLine("Maybe not the best choice");
                }
				else if (choice == "2")
				{
					if (EquippedWeapon.CompareTo(opponent.EquippedWeapon) < 1)
						Console.WriteLine("Probably a good idea");
					else
						Console.WriteLine("Maybe not the best choice");
				}
			}

			//only armor is stealable
			else if (opponent.EquippedArmor.IsRemovable)
			{
				Console.WriteLine($"You can take the {opponent.Name}'s:\n" +
				$"{opponent.EquippedArmor.Description} with {opponent.EquippedArmor.ProtectValue} defense (1)" +
				"or don't (2)'");

				// query user
				String choice = Console.ReadLine();

				if (choice == "1")
				{
					EquippedArmor = opponent.EquippedArmor;
                    Console.WriteLine($"You took the {opponent.EquippedArmor.Description}.");

                    //todo: replace judging with a function probably
                    if (EquippedArmor.CompareTo(opponent.EquippedArmor) == 1)
                        Console.WriteLine("Probably a good idea");
                    else
                        Console.WriteLine("Maybe not the best choice");
                }
				else if (choice == "2")
				{
					if (EquippedArmor.CompareTo(opponent.EquippedArmor) < 1)
						Console.WriteLine("Probably a good idea");
					else
						Console.WriteLine("Maybe not the best choice");
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

}