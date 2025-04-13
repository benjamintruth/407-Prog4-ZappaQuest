//something

namespace ZappaQuest
{

	public class Creature
	{
		// add all base creature attributes:
		public string Name { get; }
		public string Description { get; }
		public List<Item> Inventory { get; set; }
		public int Health { get; }

		public Weapon EquippedWeapon;

		public Armor EquippedArmor;

		public Creature(string name, string description, List<Item> inventory, int health, Weapon equippedWeapon, Armor equippedArmor)
		{
			Name = name;
			Description = description;
			Inventory = inventory;
			Health = health;
			EquippedWeapon = equippedWeapon;
			EquippedArmor = equippedArmor;
		}
	}

	// special subclass for player character
	public class Frank : Creature
	{
		public int CurrentRoomIndex { get; set; }

		public Frank() : base(
			name: "Frank Zappa",
			description: "the man himself",
			inventory: new List<Item>(),
			health: 100,
			equippedWeapon: null,
			equippedArmor: null
		)
		{
			CurrentRoomIndex = 0;
		}
	}

}