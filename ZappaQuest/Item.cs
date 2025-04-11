using System;
namespace ZappaQuest {


public class Item {
    // Type and Description are String
    public string Name {get; set;}
    public string Description {get; set;}
    // Have a boolean to check if item is magical or not
    public bool IsMagical {get; set;}

    public Item(string name, string description, bool isMagical=false) {
        Name = name;
        Description = description;
        IsMagical = isMagical;
    } 

    public virtual void Use() {
        Console.WriteLine($"Using {Name}: {Description}");
    }
   
} // End Item Class

// Subclasses: Weapon, Armor, Treasure, Consumable(Food), Magic Items
// -----------------------------------------------------------------------
// Weapon Subclass
public class Weapon : Item {
    public int NumAttacksPerTurn {get; set;}
    public int MaxDamage {get; set;}
    public bool IsRemovable{get; set;}

    public Weapon(string name, string description, bool isMagical, int attacksPerTurn, int maxDamage, bool isRemovable)
    : base(name, description, isMagical) {
        NumAttacksPerTurn = attacksPerTurn;
        MaxDamage = maxDamage;
        IsRemovable = isRemovable;
    }

    public override void Use() {
        Random gen = new Random();
        int damage = gen.Next(1, MaxDamage + 1);
        Console.WriteLine($"{Name} attacks with {damage} damage!");
    }

    public int CompareTo(Weapon other) {
        if(other == null) return 1;
        return this.MaxDamage.CompareTo(other.MaxDamage);
    }
} // End Wepapon Subclass

// Armor Subclass
public class Armor : Item {
    // Armor : Has a protection value and whether it is removable
    public int ProtectValue {get; set;}
    public bool IsRemovable {get; set;}

    public Armor(string name, string description, bool isMagical, int protectValue, bool isRemovable) 
    : base(name, description, isMagical) {
        ProtectValue = protectValue;
        IsRemovable = isRemovable;
    }

    public override void Use() {
        Console.WriteLine($"{Name} has {ProtectValue} protection for creature.");
    }
} // End Armor Subclass 

// Treasure Subclass
public class Treasure : Item {
    // Treasure : Contians int value 
    public int Value {get; set;}

    public Treasure(string name, string description, bool isMagical, int value) 
    : base(name, description, isMagical) {
        Value = value;
    }

    public override void Use() {
        Console.WriteLine($"{Name} is a treasure worth {Value} Zappa tickets.");
        Console.WriteLine("Take it, don't worry about how it works (;");
    }
} // End Treasure Subclass 

// Food Subclass
public class Consumable : Item {
    // Food: Has maximum value so when eaten, will heal User at random amount up to that value
    public int Food {get; set;}

    public Consumable(string name, string description, bool isMagical, int food)
    : base(name, description, isMagical) {
        Food = food;
    }

    public override void Use() {
        Random gen = new Random();
        int healingAmt = gen.Next(1, Food + 1);
        Console.WriteLine($"{Name} heals you up to {healingAmt} points");
    }
} // End Consumable Subclass 

public class MagicItem : Item {
    public int JazzPower {get; set;}

    public MagicItem(string name, string description, int jazzPower)
    : base(name, description, true) {
        JazzPower = jazzPower;
    }
    public override void Use() {
        Random gen = new Random();
        int magicHealing = gen.Next(1, JazzPower + 1);
        Console.WriteLine($"You use {Name}, healing {magicHealing} of jazz power");
    }
} // End MagicItem Subclass

}