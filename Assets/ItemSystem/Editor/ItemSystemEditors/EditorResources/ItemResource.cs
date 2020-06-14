using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/**
 * Helper resource class for editors to get data related to the various Items in the system.
 */
public class ItemResource : EditorResource
{
    private List<InventoryItem> __items;

    public List<InventoryItem> Items
    {
        get
        {
            if (__items == null)
            {
                __items = new List<InventoryItem>();
                ScanForItems();
            }

            return __items;
        }
    }

    public void ScanForItems()
    {
        // empty out previous items -- don't want duplicates or items that no longer exist
        Items.Clear();

        // find string identifiers for all Assets that are of type InventoryItem
        string[] itemGuids = AssetDatabase.FindAssets("t:InventoryItem");

        // use string identifiers to find the actual InventoryItem objects and add them to possibleItems
        foreach (string guid in itemGuids)
        {
            InventoryItem item = AssetDatabase.LoadAssetAtPath<InventoryItem>(AssetDatabase.GUIDToAssetPath(guid));
           Items.Add(item);
        }
    }
}
