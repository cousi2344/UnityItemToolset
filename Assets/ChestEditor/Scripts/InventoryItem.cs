/**
 * Class that represents an item in an inventory with one or more attributes.
 * 
 * Written by: Jacob Cousineau
 * 
 * Class structure adapted from Unity Forums user TonyLi's comments here: https://forum.unity.com/threads/rpg-inventory-scriptableobject-list-or-list-of-scriptableobjects.371686/
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item", fileName = "Item.asset")]
/* Represents a single Item that can be in an inventory */
public class InventoryItem : ScriptableObject {

    public const string STRING_INDENT = "     ";

    public string itemName;

    public List<ItemAttribute> attributes; // all attributes that this item has

    // print info about item
    public void PrintItem()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine(itemName);
        builder.AppendLine(new string('=', itemName.Length));
        builder.AppendLine();

        // go through each attribute and add its info to our string builder
        foreach (ItemAttribute attrib in attributes)
        {
            builder.AppendLine(attrib.PrintAttribute());
        }

        Debug.Log(builder.ToString());
    }
}
