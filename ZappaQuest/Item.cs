namespace ZappaQuest
{
	// ITEM CLASS ------------------------------------------------------------
	// This class is a base for all items in the adventure game, including:
	// Weapon, Armor, Treasure, Food(Consumable Items), and Magic items.
	// Each item has: Description(with stats), and computed Gold Value
	// All subclasses inherits from Item with specific properties and override Information()
	// to display its details. 

	// Item Class
	public abstract class Item
	{
		// Type and Description are String
		public string Description { get; set; }
		// Have a bool to check if item is magical or not
		public bool IsMagical { get; set; }

		// Gold Value: value is calculated base don subclasses attributes and increased by 30% if magical
		public int GoldValue { get; set; }

		// Item Constructor: Assign both item's description and if it is magical
		public Item(string description, bool isMagical = false)
		{
			Description = description;
			IsMagical = isMagical;
		} // end Item Constructor

		// Display basic information on items which can be overridden by subclasses
		public virtual string Information()
		{
			return $"Item: {Description}, Magical: {IsMagical}";
		}

		// Compare item to Gold Value 
		// used for determining to player which item is more valuable 
		public int CompareTo(Item other)
		{
			if (other == null) return 1;
			return (this.GoldValue).CompareTo(other.GoldValue);
		} // End CompareTo()

	} // End Item Class

	// Subclasses: Weapon, Armor, Treasure, Consumable(Food), Magic Items
	// -----------------------------------------------------------------------

	// WEAPON SUBCLASS 
	// Used in combat with Creatures. This subclass includes: attacks per turn,
	// maximum damage, and whether weapon is removable. 
	public class Weapon : Item
	{
		// Number of times weapon can be used in single turn
		public int NumAttacksPerTurn { get; set; }
		// Maximum number of damage weapon can perform on creature
		public int MaxDamage { get; set; }
		// Checks if weapon obtained is removable 
		public bool IsRemovable { get; set; }
		// Weapon Constructor: Assign each description values and calculates its gold value
		public Weapon(string description, bool isMagical, int attacksPerTurn, int maxDamage, bool isRemovable)
		: base(description, isMagical)
		{
			NumAttacksPerTurn = attacksPerTurn;
			MaxDamage = maxDamage;
			IsRemovable = isRemovable;
			// Calculation of Gold Value: (damage * attacks * 10); Value increased by 30% if magical. 
			GoldValue = maxDamage * attacksPerTurn * 10;
			if (this.IsMagical)
			{
				GoldValue = (int)Math.Floor(GoldValue * 1.30);
			}
		}

		public override string Information()
		{
			return $"Weapon: {Description}, Attacks: {NumAttacksPerTurn}, Max damage: {MaxDamage}";
		}
	} // End Weapon Subclass
	  // -------------------------------------
	  // ARMOR SUBCLASS
	public class Armor : Item
	{
		// Armor : Has a protection value and whether it is removable
		public int ProtectValue { get; set; }
		public bool IsRemovable { get; set; }

		public Armor(string description, bool isMagical, int protectValue, bool isRemovable)
		: base(description, isMagical)
		{
			ProtectValue = protectValue;
			IsRemovable = isRemovable;
			GoldValue = protectValue * 100;
			if (this.IsMagical)
			{
				GoldValue = (int)Math.Floor(GoldValue * 1.30);
			}
		}

		public override string Information()
		{
			return $"Armor: {Description}, Protection Available: {ProtectValue}, Removable? {IsRemovable}";
		}


	} // End Armor Subclass 
	  // -------------------------------------
	  // TREASURE SUBCLASS
	public class Treasure : Item
	{
		// Treasure : Contains int value 
		public int Value { get; set; }

		public Treasure(string description, bool isMagical, int value)
		: base(description, isMagical)
		{
			Value = value;
			GoldValue = Value * 100;
			if (this.IsMagical)
			{
				GoldValue = (int)Math.Floor(GoldValue * 1.30);
			}
		}

		public override string Information()
		{
			return $"Treasure: {Description}, Value: {Value}";
		}

	} // End Treasure Subclass 
	  // -------------------------------------
	  // FOOD SUBCLASS
	public class Food : Item
	{
		// Food: Has maximum value so when eaten, will heal User at random amount up to that value
		public int Consumable { get; set; }

		public Food(string description, bool isMagical, int consumable)
		: base(description, isMagical)
		{
			Consumable = consumable;
			GoldValue = consumable * 100;
			if (this.IsMagical)
			{
				GoldValue = (int)Math.Floor(GoldValue * 1.30);
			}

		}

		public override string Information()
		{
			return $"Food/Consumable: {Description}, Maximum Healing: {Consumable}";
		}

	} // End Food Subclass 
	  // -------------------------------------
	  // MAGIC ITEM SUBCLASS
	public class MagicItem : Item
	{
		public int JazzPower { get; set; }

		public MagicItem(string description, int jazzPower)
		: base(description, true)
		{
			JazzPower = jazzPower;
			GoldValue = jazzPower * 1000;
			if (this.IsMagical)
			{
				GoldValue = (int)Math.Floor(GoldValue * 1.30);
			}

		}
		// Display Information on Magic Items
		public override string Information()
		{
			return $"Magic Item: {Description}, Power: {JazzPower}";
		} // End Override Information()

	} // End MagicItem Subclass

} // end ZappaQuest