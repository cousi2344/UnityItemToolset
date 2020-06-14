﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

/**
 * Helper resource class for editors to get data related to the various ItemAttributes in the sytem.
 */
public class ItemAttributeResource : EditorResource
{

    private List<System.Type> itemAttributeTypes = new List<System.Type>();

    public List<System.Type> ItemAttributeTypes
    {
        get
        {
            if (itemAttributeTypes == null)
            {
                ScanForItemAttributeTypes();
            }

            return itemAttributeTypes;
        }
    }

    public List<string> ItemAttributeNames
    {
        get
        {
            return ItemAttributeTypes.Select(type => type.Name).ToList();
        }
    }

    public void ScanForItemAttributeTypes()
    {
        // clear out any previous finds, in case they no longer exist in the project
        itemAttributeTypes.Clear();

        // get all types relevant to the ItemAttribute class
        System.Type[] subclassTypeArray = Assembly.GetAssembly(typeof(ItemAttribute))
            .GetTypes();

        foreach (System.Type t in subclassTypeArray)
        {
            // make sure that the given type is a subclass of ItemAttribute
            if (t.IsSubclassOf(typeof(ItemAttribute)))
            {
                // if it is, add the type to one list and its name to the other
                itemAttributeTypes.Add(t);
            }
        }
    }
}