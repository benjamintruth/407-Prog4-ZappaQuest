using System;
using System.Collections.Generic;
namespace ZappaQuest



public class Item {
    // Type and Description are String
    public string Name {get; set;}
    public string Description {get; set;}
    // Have a boolean to check if item is magical or not
    public bool isMagical {get; set;}

    public Item(string n, string des, bool isMag) {
    Name = n;
    Description = des;
    isMagical = isMag;
    } 

    public override string ToString() {
    return $"{Name} - {Description}" + (isMagical ? " ~Magical~" : "");
    }
}


// Subclasses: Weapon, Armor, Treasure, Consumable(Food), Magic Items 
// Finish editing Weapon subclass...
public class Weapon : Item {
    // Weapon has number of attacks per turn, maximum damage per hit and whether the 
    // weapon is removable(but claws).
    public int NumberAttacks {get; set;}
    public int MaximumDamage {get; set;}
    public bool IsRemovable {get; set;} 

}

public class Armor : Item {
    // Armor: Has protection value and shows whether armor is removable
    public int ProtectVal {get; set;}
    public bool isRemovable {get; set}

    public Armor(string des, int pValue, isRemov) : base(des) {
        ProtectVal = pValue;
        isRemovable = isRemov;
    }
}

public class Treasure : Item {
    // Treasure
    public int value {get; set;}

    public Treasure(string des, bool isMag, val): base(des, isMag) {
        value = val;
    }

}

public class Cosumable : Item {

}

public class MagicItems : Item {

}