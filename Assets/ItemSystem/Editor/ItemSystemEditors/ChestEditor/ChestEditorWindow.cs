using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
    
/**
 * Custom Editor window for editing the contents of chests or other items that have
 * an inventory.
 */
public class ChestEditorWindow : BaseCustomEditorWindow {
    /**
     * Member vars for both methods of adding items
     */
    // current list of InventoryItems that the editor knows about
    List<InventoryItem> possibleItems = new List<InventoryItem>();

    // flag determining if chests will be cleared out before adding the selected items
    bool clearChestsOnAdd = false;

    ItemAttributeResource itemAttributeResource;

    /**
     * Member vars for selecting manually (picking each item one by one)
     */
    // list of flags corresponding to InventoryItems in possibleItems -- true if the item has been selected and will be added
    List<bool> itemSelectionChecklist = new List<bool>();

    // flag for submenu used to add specific items manually
    bool selectItemsManually = true;


    /**
     * Member vars used for selecting by criteria
     */

    // list containing the current attribute selections for each 
    List<int> attribSelections = new List<int>();

    // flag for submenu used to add random items, possibly based on their attributes
    bool selectItemsByAttrib = false;

    // if using by attribute submenu, how many items do we want to create
    int numItems = 0;

    [MenuItem("Window/Chest Editor")]
    public static void Init()
    {
        GetWindow<ChestEditorWindow>("Chest Editor");
    }

    protected override void ConfigureWindow()
    {
        scrollableControl.enabled = true;
    }

    protected override void ConstructInnerWindow()
    {
        itemAttributeResource = GetResource<ItemAttributeResource>();
    }

    // scans Assets folder for items that could be added to chests
    private void ScanForItems()
    {
        // empty out previous items -- don't want duplicates or items that no longer exist
        possibleItems.Clear();
        itemSelectionChecklist.Clear();

        // find string identifiers for all Assets that are of type InventoryItem
        string[] itemGuids = AssetDatabase.FindAssets("t:InventoryItem");

        // use string identifiers to find the actual InventoryItem objects and add them to possibleItems
        foreach (string guid in itemGuids)
        {
            InventoryItem item = AssetDatabase.LoadAssetAtPath<InventoryItem>(AssetDatabase.GUIDToAssetPath(guid));
            possibleItems.Add(item);
            itemSelectionChecklist.Add(false); // all objects are initially not selected to go into the chest
        }
    }

    // does both the scans outlined in ScanForItems and ScanForAttribTypes
    private void Scan()
    {
        ScanForItems();
        itemAttributeResource.ScanForItemAttributeTypes();
    }

    // add all selected items to a single chest
    private void AddSelectedToChest(Chest chest)
    {
        // if specified, clear out the chest before adding new items
        if (clearChestsOnAdd)
        {
            chest.ClearChest();
        }

        // if user chose to select items by hand, simply add them
        if (selectItemsManually)
        {
            // look at each item in possibleItems and check if it is selected
            for (int i = 0; i < possibleItems.Count; i++)
            {
                if (itemSelectionChecklist[i])
                {
                    // it's selected -- add it to the chest
                    chest.AddToChest(possibleItems[i]);
                }
            }
        }
        // if user chose to select by attribute, get items that fit those criteria
        else if (selectItemsByAttrib)
        {
            // loop for each item to be added
            for(int i = 0; i < numItems; i++)
            {
                // make a list to hold all the InventoryItem objects we find that match our criteria
                List<InventoryItem> matchingItems = new List<InventoryItem>();

                // look through every inventory item we know about
                foreach (InventoryItem item in possibleItems)
                {
                    // get that thing
                    System.Type selectedType = itemAttributeResource.ItemAttributeTypes[i];

                    foreach (ItemAttribute attrib in item.attributes)
                    {
                        if (attrib != null && selectedType.Equals(attrib.GetType()))
                        {
                            matchingItems.Add(item);
                        }
                    }
                }

                chest.AddToChest(matchingItems[Random.Range(0, matchingItems.Count)]);
            }
        }

        // tell the editor that the object has been changed
        // this prevents changes from disappearing when going into play mode
        // I believe there is a more current way to do this, but I haven't figured it out yet
        Undo.RecordObject(chest, "Chest Modify");
        EditorUtility.SetDirty(chest);

    }

    // clear all items from a single chest
    private void ClearChest(Chest chest)
    {
        chest.ClearChest();

        Undo.RecordObject(chest, "Chest Clear");
        EditorUtility.SetDirty(chest);
    }

    // update the user interface for the window
    protected override void DrawInnerWindow()
    {
        // if we haven't done a scan yet, do one
        if(possibleItems.Count == 0 || itemAttributeResource.ItemAttributeTypes.Count == 0)
        {
            Scan();
        }

        // put button in top right corner to scan for items
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("Rescan"))
        {
            Scan();
        }
        EditorGUILayout.EndHorizontal();

        // submenu for manually picking out items
        selectItemsManually = EditorGUILayout.BeginToggleGroup("Select Items Manually", selectItemsManually);
        if (selectItemsManually)
        {
            // add each item in possibleItems as a toggle
            for (int i = 0; i < possibleItems.Count; i++)
            {
                itemSelectionChecklist[i] = EditorGUILayout.Toggle(possibleItems[i].itemName, itemSelectionChecklist[i]);

            }

            // set other menu off
            selectItemsByAttrib = false;
        }
        EditorGUILayout.EndToggleGroup();

        // submenu for selecting random items by different criteria
        selectItemsByAttrib = EditorGUILayout.BeginToggleGroup("Select Items By Attribute", selectItemsByAttrib);
        if(selectItemsByAttrib)
        {
            // field for specifying number of items that you want to add
            EditorGUILayout.BeginHorizontal();
            numItems = EditorGUILayout.IntField("Number of Items", numItems);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            // if the number of items specified doesn't match the length of our list which keeps track of them, reset the list and set selections back to 0
            if(numItems != attribSelections.Count)
            {
                attribSelections.Clear();
                for(int i = 0; i < numItems; i++)
                {
                    attribSelections.Add(0);
                }
            }

            // for each item, give selection criteria options
            for (int i = 0; i < numItems; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Item " + (i+1) + ":");
                attribSelections[i] = GUILayout.SelectionGrid(attribSelections[i], itemAttributeResource.ItemAttributeNames.ToArray(), 5);
                EditorGUILayout.EndHorizontal();
            }

            // set other menu off
            selectItemsManually = false;
        }

        EditorGUILayout.EndToggleGroup();

        GUILayout.FlexibleSpace();

        // add toggle for clearing chests when adding items
        clearChestsOnAdd = EditorGUILayout.Toggle("Clear Chests On Add", clearChestsOnAdd);

        EditorGUILayout.BeginHorizontal();

        // button for adding to all selected gameObjects
        if(GUILayout.Button("Add To Selected"))
        {
            // look through all selected gameObjects
            foreach (GameObject go in Selection.gameObjects)
            {
                // check if this is a chest -- if so, add the selected items to it
                Chest c = go.GetComponent<Chest>();
                if (c != null)
                {
                    AddSelectedToChest(c);
                }
            }
        }

        // button for adding items to ALL chests, regardless of selection
        if (GUILayout.Button("Add to All Chests"))
        {
            // find all chests in scene
            Chest[] allChests = FindObjectsOfType<Chest>();

            // add items to each chest
            foreach (Chest c in allChests)
            {
                AddSelectedToChest(c);
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear Selected"))
        {
            // look through all selected gameObjects
            foreach (GameObject go in Selection.gameObjects)
            {
                // check if this is a chest -- if so, clear it
                Chest c = go.GetComponent<Chest>();
                if (c != null)
                {
                    ClearChest(c);
                }
            }
        }

        // button for clearing all chests
        if (GUILayout.Button("Clear All Chests"))
        {
            // find all chests in scene
            Chest[] allChests = FindObjectsOfType<Chest>();

            // add items to each chest
            foreach (Chest c in allChests)
            {
                ClearChest(c);
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
