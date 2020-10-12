/**
 * Class that represents an attribute that an inventory item can have.
 * 
 * Written by: Jacob Cousineau
 * 
 * Class structure adapted from Unity Forums user TonyLi's comments here: https://forum.unity.com/threads/rpg-inventory-scriptableobject-list-or-list-of-scriptableobjects.371686/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Abstract class for an attribute on an item.
 * 
 * Attributes can be things like damage, armor, consumable effects, etc.
 */
public abstract class ItemAttribute : ScriptableObject
{
    // abstract function -- must be implemented in child classes
    // prints information about the attribute
    public abstract string PrintAttribute();
}
