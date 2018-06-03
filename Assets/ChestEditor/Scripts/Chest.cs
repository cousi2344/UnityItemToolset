using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles chest inventory */
public class Chest : MonoBehaviour {

    [SerializeField]
    private List<InventoryItem> items; // items in the chest

    // removes all items from chest
    public void ClearChest()
    {
        items.Clear();
    }

    // adds an item to the chest
	public void AddToChest(InventoryItem item)
    {
        items.Add(item);
    }

    // handle clicking on the chest
    public void OnMouseDown()
    {
        // print each item in the chest
        foreach( InventoryItem item in items)
        {
            item.PrintItem();
        }
    }
}
