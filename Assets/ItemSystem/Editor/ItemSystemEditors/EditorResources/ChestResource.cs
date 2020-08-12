using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


/**
 * Helper resource class for editors to get data related to the various Items in the system.
 */
public class ChestResource : EditorResource
{
    // various methods of selecting which chests to put items into
    public enum ChestSelectionMethod { Selected, All };

    private ItemResource itemResource;

    public ChestResource()
    {
        itemResource = MyEditorConfiguration.Instance.GetResource<ItemResource>();
    }

    /**
     * Add a list of items to a group of chests
     */
    public void AddToChests(ChestSelectionMethod method, bool clearOnAdd, List<InventoryItem> items)
    {
        List<Chest> chests = GetChests(method);

        if (chests != null)
        {
            foreach(Chest chest in chests)
            {
                AddToChest(chest, clearOnAdd, items);
            }
        }
    }

    /**
     * Add a list of items with the given item attributes to a group of chests
     */
    public void AddToChests(ChestSelectionMethod method, bool clearOnAdd, List<System.Type> itemAttributeTypes)
    {
        List<Chest> chests = GetChests(method);

        if (chests != null)
        {
            foreach (Chest chest in chests)
            {
                List<InventoryItem> items = itemAttributeTypes.Select(type => itemResource.GetRandomMatchingItem(type)).ToList();
                AddToChest(chest, clearOnAdd, items);
            }
        }
    }

    /**
     * Add a list of items to one chest
     */
    private void AddToChest(Chest chest, bool clearOnAdd, List<InventoryItem> items)
    {
        if (clearOnAdd)
        {
            chest.ClearChest();
        }

        foreach (InventoryItem item in items)
        {
            chest.AddToChest(item);
        }

        // tell the editor that the object has been changed
        // this prevents changes from disappearing when going into play mode
        // I believe there is a more current way to do this, but I haven't figured it out yet
        Undo.RecordObject(chest, "Chest Modify");
        EditorUtility.SetDirty(chest);
    }

    /**
     * Get a list of all chests that will be selected given the selection mode
     */
    private List<Chest> GetChests(ChestSelectionMethod method)
    {
        List<Chest> chests = new List<Chest>();

        switch (method)
        {
            case ChestSelectionMethod.All:
                chests = FindObjectsOfType<Chest>().ToList();
                break;
            case ChestSelectionMethod.Selected:
            default:
                chests = Selection.gameObjects
                    .Where(gameObject => gameObject.GetComponent<Chest>() != null)
                    .Select(gameObject => gameObject.GetComponent<Chest>())
                    .ToList();
                break;
        }

        return chests;
    }
}
