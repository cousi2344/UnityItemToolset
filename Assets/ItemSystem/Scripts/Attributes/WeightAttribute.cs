/**
 * ItemAttribute subclass that denotes an item as having weight in the inventory system.
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

[CreateAssetMenu(menuName = "Attribute/Weight Attribute", fileName = "WeightAttrib.asset")]
/* Attribute that specifies the amount of weight that an object has */
public class WeightAttribute : ItemAttribute
{

    public float weight;



    // print out attrib info
    public override string PrintAttribute()
    {
        StringBuilder builder = new StringBuilder();
        string title = "Weight Attribute";
        builder.AppendLine(title);
        builder.AppendLine(new string('-', title.Length));
        builder.AppendLine("Weight: " + weight);

        return builder.ToString();
    }
}
