using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

/**
 *  Base class for editors within the item system.
 *  
 *  Contains variables and methods that will need to be commonly accessible by most
 *  or all of the editor windows.
 */
public abstract class ItemSystemEditorBase : BaseCustomEditorWindow
{

    // list of ItemAttribute subclasses
    protected List<System.Type> attribSubclassTypes = new List<System.Type>();

    // list containing names for each Type in attribSubclassTypes
    protected List<string> attribNames = new List<string>();

    // scans for all subclass types of ItemAttribute, so that we know what kinds of attributes to look for
    protected void ScanForAttribTypes()
    {
        // clear out any previous finds, in case they no longer exist in the project
        attribSubclassTypes.Clear();
        attribNames.Clear();

        // get all types relevant to the ItemAttribute class
        System.Type[] subclassTypeArray = Assembly.GetAssembly(typeof(ItemAttribute))
            .GetTypes();

        foreach (System.Type t in subclassTypeArray)
        {
            // make sure that the given type is a subclass of ItemAttribute
            if (t.IsSubclassOf(typeof(ItemAttribute)))
            {
                // if it is, add the type to one list and its name to the other
                attribSubclassTypes.Add(t);
                attribNames.Add(t.Name);
            }
        }

    }
}
