using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
     * Create an Inventory item with a given set of attributes and create
     * necessary .asset files to store them in.
     */
    public void CreateNewItem(string newItemName, List<System.Type> attributesToAdd)
    {
        // create the inventory item itself and name it properly
        InventoryItem newItem = ScriptableObject.CreateInstance<InventoryItem>();
        newItem.itemName = newItemName;

        // lists to hold attributes and their names for each attrib we will add
        List<ItemAttribute> addedAttributes = new List<ItemAttribute>();
        List<string> addedAttributeNames = new List<string>();

        // loop through our attrib selection menu
        for (int j = 0; j < attributesToAdd.Count; j++)
        {
            // get the type of the attribute
            System.Type attribType = attributesToAdd[j];

            // use reflection to make a version of ScriptableObject.CreateInstance that is specific to the type of the attrib
            // we need this because these types are not known at runtime, so we can't use generics directly
            // credit to Tim Robinson: https://stackoverflow.com/questions/3555056/how-should-i-call-the-generic-function-without-knowing-the-type-at-compile-time?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
            MethodInfo methodDefinition = typeof(ScriptableObject).GetMethod("CreateInstance", new System.Type[] { });
            MethodInfo method = methodDefinition.MakeGenericMethod(attribType);

            // add type and its name to a list
            addedAttributes.Add((ItemAttribute)method.Invoke(null, null));
            addedAttributeNames.Add(attribType.ToString());
        }

        // tell the inventory item about its attributes
        newItem.attributes = addedAttributes;

        // create a folder to put it in if it doesn't exist
        if (!AssetDatabase.IsValidFolder("Assets/" + newItemName))
        {
            AssetDatabase.CreateFolder("Assets", newItemName);
        }

        // create the acutal asset and put it in the folder
        AssetDatabase.CreateAsset(newItem, "Assets/" + newItemName + "/" + newItemName + ".asset");

        // create the assets for each attribute in our list
        for (int x = 0; x < addedAttributes.Count; x++)
        {
            AssetDatabase.CreateAsset(addedAttributes[x], "Assets/" + newItemName + "/" + newItemName + addedAttributeNames[x] + ".asset");
        }

        AssetDatabase.SaveAssets();
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
