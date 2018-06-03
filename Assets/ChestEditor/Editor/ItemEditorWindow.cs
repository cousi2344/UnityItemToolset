using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

/**
 * Custom Editor window for creating and/or editing items.
 */
public class ItemEditorWindow : ItemSystemEditorBase {

    /**
     * Member vars for general editor window control
     */

    // Integer codes for each tab
    const int CREATE_ITEM_TAB = 0;
    const int EDIT_ITEM_TAB = 1;

    // integer code representing current tab of the editor window we are in
    private int currentTab;

    /**
     *  Member vars used for creation of new items.
     */

    // default name for created item
    const string DEFAULT_ITEM_NAME = "DefaultItem";

    // string containing name input for a new item
    private string newItemName = DEFAULT_ITEM_NAME;

    /**
     * Member vars used for editing existing items.
     */

    // item that is currently selected for editing
    private InventoryItem selectedItem;


    // list of flags corresponding to all atrributes that can be added to an item
    // true if the attrib has been selected and will be added
    List<bool> attribSelectionChecklist = new List<bool>();

    [MenuItem("Window/Item Editor")]
    public static void Init()
    {
        GetWindow<ItemEditorWindow>("Item Editor");
    }

    /*
     * scans for attribute types and handles re-populating attribute checklist.
     */
    private void Scan()
    {
        ScanForAttribTypes();

        attribSelectionChecklist.Clear();

        for(int i = 0; i < attribSubclassTypes.Count; i++)
        {
            attribSelectionChecklist.Add(false);
        }
    }

    /*
     * Create an Inventory item with a given set of attributes and create
     * necessary .asset files to store them in.
     */
    private void CreateNewItem()
    {
        // create the inventory item itself and name it properly
        InventoryItem newItem = ScriptableObject.CreateInstance<InventoryItem>();
        newItem.itemName = newItemName;

        // lists to hold attributes and their names for each attrib we will add
        List<ItemAttribute> attribsToAdd = new List<ItemAttribute>();
        List<string> attribsToAddNames = new List<string>();

        // loop through our attrib selection menu
        for (int j = 0; j < attribSelectionChecklist.Count; j++)
        {
            // if attrib checked, we will create an instance of that attrib
            if (attribSelectionChecklist[j])
            {
                // get the type of the attribute
                System.Type attribType = attribSubclassTypes[j];

                // use reflection to make a version of ScriptableObject.GetMethod that is specific to the type of the attrib
                // we need this because these types are not known at runtime, so we can't use generics directly
                // credit to Tim Robinson: https://stackoverflow.com/questions/3555056/how-should-i-call-the-generic-function-without-knowing-the-type-at-compile-time?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
                MethodInfo methodDefinition = typeof(ScriptableObject).GetMethod("CreateInstance", new System.Type[] { });
                MethodInfo method = methodDefinition.MakeGenericMethod(attribType);

                // add type and its name to a list
                attribsToAdd.Add((ItemAttribute)method.Invoke(null, null));
                attribsToAddNames.Add(attribType.ToString());
            }
        }

        // tell the inventory item about it's attributes
        newItem.attributes = attribsToAdd;

        // create a folder to put it in if it doesn't exist
        if (!AssetDatabase.IsValidFolder("Assets/" + newItemName))
        {
            AssetDatabase.CreateFolder("Assets", newItemName);
        }

        // create the acutal asset and put it in the folder
        AssetDatabase.CreateAsset(newItem, "Assets/" + newItemName + "/" + newItemName + ".asset");

        // create the assets for each attribute in our list
        for (int x = 0; x < attribsToAdd.Count; x++)
        {
            AssetDatabase.CreateAsset(attribsToAdd[x], "Assets/" + newItemName + "/" + newItemName + attribsToAddNames[x] + ".asset");
        }

        AssetDatabase.SaveAssets();
    }

    /*
     * update user interface for the window.
     */
    private void OnGUI()
    {
        // if we haven't done a scan yet, do one
        if (attribSubclassTypes.Count == 0)
        {
            Scan();
        }

        currentTab = GUILayout.Toolbar(currentTab, new string[] { "Create Items", "Edit Items" });

        switch(currentTab)
        {
            case CREATE_ITEM_TAB:
                // create items tab
                newItemName = EditorGUILayout.TextField("Item Name", newItemName);

                // add each item in possibleItems as a toggle
                for (int i = 0; i < attribNames.Count; i++)
                {
                    attribSelectionChecklist[i] = EditorGUILayout.Toggle(attribNames[i], attribSelectionChecklist[i]);
                }

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                // button used to actually create the asset
                if (GUILayout.Button("Create Item"))
                {
                    CreateNewItem();
                    currentTab = EDIT_ITEM_TAB;
                }

                GUILayout.EndHorizontal();
                break;

            case EDIT_ITEM_TAB:
                // edit items tab

                foreach (Object obj in Selection.objects)
                {
                    InventoryItem asItem = obj as InventoryItem;
                    if (asItem != null)
                    {
                        // we have an InventoryItem!

                        EditorGUILayout.LabelField(asItem.itemName, EditorStyles.boldLabel); // name of item

                        EditorGUI.indentLevel++; // indent

                        // for each attribute this item has, list all the attribute's properties
                        foreach (ItemAttribute attrib in asItem.attributes)
                        {
                            
                            EditorGUILayout.LabelField(attrib.GetType().Name); // name of attrib

                            SerializedObject serializedAttrib = new SerializedObject(attrib);

                            SerializedProperty serializedProp = serializedAttrib.GetIterator(); // iterator over all properties of attrib

                            EditorGUI.indentLevel++;

                            // look through all properties and display them
                            while (serializedProp.NextVisible(true))
                            {
                                if (serializedProp.name != "m_Script")
                                    EditorGUILayout.PropertyField(serializedProp, new GUIContent(UnityEditor.ObjectNames.NicifyVariableName(serializedProp.name)), GUILayout.MinWidth(100));
                            }

                            // allow modification of properties
                            serializedAttrib.ApplyModifiedProperties();

                            EditorGUI.indentLevel--;
                        }

                        EditorGUI.indentLevel--;
                    }
                }
                break;

        }
    }
}
