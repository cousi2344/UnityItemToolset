/**
 * ItemAttribute subclass that denotes an item as being able to deal damage.
 * 
 * Written by: Jacob Cousineau
 * 
 * Structure of inventory system adapted from Unity Forums user TonyLi's comments here: https://forum.unity.com/threads/rpg-inventory-scriptableobject-list-or-list-of-scriptableobjects.371686/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Attribute/Damage Attribute", fileName = "DamageAttrib.asset")]
/* Attribute that specifies the type and quantity of damage that an item deals */
public class DamageAttribute : ItemAttribute {

    // special types of damage that an object can have
    [Flags]
    public enum eSpecialDamageTypes {
        Piercing = 1,
        Burning = 2,
        Freezing = 4,
        Poison = 8
    }

    public float damage; // amount of damage this object deals

    [EnumFlag]
    public eSpecialDamageTypes damageTypes; // which special damage types does the item this attrib is attached to have?

    // print out attrib info
    public override string PrintAttribute()
    {
        StringBuilder builder = new StringBuilder();
        string title = "Damage Attribute";
        builder.AppendLine(title);
        builder.AppendLine(new string('-', title.Length));
        builder.AppendLine("Damage: " + damage);
        builder.AppendLine("Special Damage Types: " + damageTypes.ToString());
        

        return builder.ToString();
    }
}
