using System;
namespace ZappaQuest {


public class Item {
    // Type and Description are String
    public string Description {get; set;}
    // Have a boolean to check if item is magical or not
    public bool IsMagical {get; set;}

    public Item(string description, bool isMagical=false) {
        Description = description;
        IsMagical = isMagical;
    } 

    public virtual string Information() {
        return $"Item: {Description}, Magical: {IsMagical}";
    }
      
} // End Item Class

// Subclasses: Weapon, Armor, Treasure, Consumable(Food), Magic Items
// -----------------------------------------------------------------------
// Weapon Subclass
public class Weapon : Item {
    public int NumAttacksPerTurn {get; set;}
    public int MaxDamage {get; set;}
    public bool IsRemovable{get; set;}

    public Weapon(string description, bool isMagical, int attacksPerTurn, int maxDamage, bool isRemovable)
    : base(description, isMagical) {
        NumAttacksPerTurn = attacksPerTurn;
        MaxDamage = maxDamage;
        IsRemovable = isRemovable;
    }

    public override string Information() {
        return $"Weapon: {Description}, Attacks: {NumAttacksPerTurn}, Max damage: {MaxDamage}";
    }

    public int CompareTo(Weapon other) {
        if(other == null) return 1;
        return (this.MaxDamage*NumAttacksPerTurn).CompareTo(other.MaxDamage*other.NumAttacksPerTurn);
    }
} // End Wepapon Subclass

// Armor Subclass
public class Armor : Item {
    // Armor : Has a protection value and whether it is removable
    public int ProtectValue {get; set;}
    public bool IsRemovable {get; set;}

    public Armor(string description, bool isMagical, int protectValue, bool isRemovable) 
    : base(description, isMagical) {
        ProtectValue = protectValue;
        IsRemovable = isRemovable;
    }

    public override string Information() {
        return $"Armor: {Description}, Protection Available: {ProtectValue}, Removable? {IsRemovable}";
    }

    public int CompareTo(Armor other)
    {
        if (other == null) return 1;
        return this.ProtectValue.CompareTo(other.ProtectValue);
    }
} // End Armor Subclass 

// Treasure Subclass
public class Treasure : Item {
    // Treasure : Contians int value 
    public int Value {get; set;}

    public Treasure(string description, bool isMagical, int value) 
    : base(description, isMagical) {
        Value = value;
    }

    public override string Information() {
        return $"Treasure: {Description}, Value: {Value}";
    }

} // End Treasure Subclass 

// Food Subclass
public class Food : Item {
    // Food: Has maximum value so when eaten, will heal User at random amount up to that value
    public int Consumable {get; set;}

    public Food(string description, bool isMagical, int consumable)
    : base(description, isMagical) {
        Consumable = consumable;
    }

    public override string Information() {
        return $"Food/Consumable: {Description}, Maxmimum Healing: {Consumable}";
    }

} // End Food Subclass 

public class MagicItem : Item {
    public int JazzPower {get; set;}

    public MagicItem(string description, int jazzPower)
    : base(description, true) {
        JazzPower = jazzPower;
    }

    public override string Information() {
        return $"Magic Item: {Description}, Power: {JazzPower}";
    }

} // End MagicItem Subclass

} // end ZappaQuest