using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * Custom Editor window for creating and/or editing items.
 */
public class ItemEditorWindow : BaseCustomEditorWindow {

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

    ItemResource itemResource;
    ItemAttributeResource itemAttributeResource;


    // list of flags corresponding to all atrributes that can be added to an item
    // true if the attrib has been selected and will be added
    List<bool> attribSelectionChecklist = new List<bool>();

    [MenuItem("Window/Item Editor")]
    public static void Init()
    {
        GetWindow<ItemEditorWindow>("Item Editor");
    }

    protected override void ConfigureWindow()
    {
        scrollableControl.enabled = true;
    }

    protected override void ConstructInnerWindow()
    {
        itemResource = GetResource<ItemResource>();
        itemAttributeResource = GetResource<ItemAttributeResource>();
    }

    // Clear out and remake the list of item attributes that can be selected
    private void RebuildAttribSelectionChecklist()
    {
        itemAttributeResource.ScanForItemAttributeTypes();

        attribSelectionChecklist.Clear();

        for(int i = 0; i < itemAttributeResource.ItemAttributeTypes.Count; i++)
        {
            attribSelectionChecklist.Add(false);
        }
    }

    /*
     * update user interface for the window.
     */
    protected override void DrawInnerWindow()
    {
        // if we haven't done a scan yet, do one
        if (attribSelectionChecklist.Count == 0)
        {
            RebuildAttribSelectionChecklist();
        }

        currentTab = GUILayout.Toolbar(currentTab, new string[] { "Create Items", "Edit Items" });

        switch(currentTab)
        {
            case CREATE_ITEM_TAB:
                newItemName = EditorGUILayout.TextField("Item Name", newItemName);

                // add each item in possibleItems as a toggle
                for (int i = 0; i < itemAttributeResource.ItemAttributeTypes.Count; i++)
                {
                    attribSelectionChecklist[i] = EditorGUILayout.Toggle(itemAttributeResource.ItemAttributeNames[i], attribSelectionChecklist[i]);
                }

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                // button used to actually create the asset
                if (GUILayout.Button("Create Item"))
                {
                    List<System.Type> selectedAttribs = new List<System.Type>();
                    for(int j = 0; j < itemAttributeResource.ItemAttributeTypes.Count; j++)
                    {
                        if (attribSelectionChecklist[j])
                        {
                            selectedAttribs.Add(itemAttributeResource.ItemAttributeTypes[j]);
                        }
                    }
                    itemResource.CreateNewItem(newItemName, selectedAttribs);
                    currentTab = EDIT_ITEM_TAB;
                }

                GUILayout.EndHorizontal();
                break;

            case EDIT_ITEM_TAB:
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
