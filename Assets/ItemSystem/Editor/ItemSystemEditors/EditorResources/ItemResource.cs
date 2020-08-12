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

    private ItemAttributeResource itemAttributeResource;

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

    public ItemResource()
    {
        itemAttributeResource = MyEditorConfiguration.Instance.GetResource<ItemAttributeResource>();
    }

    /**
     * Get a random InventoryItem that has the given type of ItemAttribute
     */
    public InventoryItem GetRandomMatchingItem(System.Type attributeType)
    {
        // make a list to hold all the InventoryItem objects we find that match our criteria
        List<InventoryItem> matchingItems = new List<InventoryItem>();

        // look through every inventory item we know about
        foreach (InventoryItem item in Items)
        {
            foreach (ItemAttribute attrib in item.attributes)
            {
                if (attrib != null && attributeType.Equals(attrib.GetType()))
                {
                    matchingItems.Add(item);
                }
            }
        }

        return matchingItems[Random.Range(0, matchingItems.Count)];
    }

    /**
     * Scan the asset database for InventoryItems
     */
    private void ScanForItems()
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
