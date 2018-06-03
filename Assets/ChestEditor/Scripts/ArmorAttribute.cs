/**
 * ItemAttribute subclass that denotes an item as being armor and therefore having some defensive capability.
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

[CreateAssetMenu(menuName = "Attribute/Armor Attribute", fileName = "ArmorAttrib.asset")]
/* Attribute that specifies the type of armor that an item provides and other details */
public class ArmorAttribute : ItemAttribute
{

    public enum eArmorType
    {
        Light,
        Medium,
        Heavy
    }

    public enum eBodyPart
    {
        Head,
        Arms,
        Chest,
        Legs,
        Feet,
        Shield,
    }

    public eArmorType armorType; // is the item this attrib is attached to considered light, medium, or heavy armor?

    public eBodyPart bodyPart; // what body part does this piece of armor go on?

    public float defense; // amount of defense armor provides



    // print out attrib info
    public override string PrintAttribute()
    {
        StringBuilder builder = new StringBuilder();
        string title = "Armor Attribute";
        builder.AppendLine(title);
        builder.AppendLine(new string('-', title.Length));
        builder.AppendLine("Armor Type: " + armorType);
        builder.AppendLine("Body Part: " + bodyPart);
        builder.AppendLine("Defense : " + defense);


        return builder.ToString();
    }
}
