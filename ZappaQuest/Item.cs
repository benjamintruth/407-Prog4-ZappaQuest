using System;
namespace ZappaQuest
{
	// ITEM CLASS
	public abstract class Item
	{
		// Type and Description are String
		public string Description { get; set; }
		// Have a bool to check if item is magical or not
		public bool IsMagical { get; set; }

		// Gold value is calculated based off the attributes 
		public int GoldValue { get; set; }

		// Item Constructor: Assign both item's description and if it is magical
		public Item(string description, bool isMagical = false)
		{
			Description = description;
			IsMagical = isMagical;
		} // end Item Constructor

		// Display information on items which can be overridden by subclasses
		public virtual string Information()
		{
			return $"Item: {Description}, Magical: {IsMagical}";
		}

		// Compare item to Gold Value 
		public int CompareTo(Item other)
		{
			if (other == null) return 1;
			return (this.GoldValue).CompareTo(other.GoldValue);
		} // End CompareTo()

	} // End Item Class

	// Subclasses: Weapon, Armor, Treasure, Consumable(Food), Magic Items
	// -----------------------------------------------------------------------

	// Weapon Subclass
	public class Weapon : Item
	{
		public int NumAttacksPerTurn { get; set; }
		public int MaxDamage { get; set; }
		public bool IsRemovable { get; set; }

		public Weapon(string description, bool isMagical, int attacksPerTurn, int maxDamage, bool isRemovable)
		: base(description, isMagical)
		{
			NumAttacksPerTurn = attacksPerTurn;
			MaxDamage = maxDamage;
			IsRemovable = isRemovable;
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

		// DEV may no longer be needed
		// public int CompareTo(Weapon other)
		// {
		// 	if (other == null) return 1;
		// 	return (this.MaxDamage * NumAttacksPerTurn).CompareTo(other.MaxDamage * other.NumAttacksPerTurn);
		// }
	} // End Weapon Subclass

	// Armor Subclass
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

	// Treasure Subclass
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

	// Food Subclass
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
			return $"Food/Consumable: {Description}, Maximimum Healing: {Consumable}";
		}

	} // End Food Subclass 

	// Magic Item Subclass
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