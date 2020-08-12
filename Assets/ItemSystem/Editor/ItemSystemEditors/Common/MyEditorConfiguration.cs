using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Provides an interface for adding new data, loaders, etc. to editor windows
 * 
 * Based on a tutorial by Christina Qi
 */
public class MyEditorConfiguration
{
    private static MyEditorConfiguration instance = null;
    public static MyEditorConfiguration Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MyEditorConfiguration();
                instance.Setup();
            }

            return instance;
        }
    }

    public void Setup()
    {
        // add resources here
        AddResource(new ItemAttributeResource());
        AddResource(new ItemResource());
        AddResource(new ChestResource());

    }

    protected List<EditorResource> resources = new List<EditorResource>();

    public T GetResource<T>() where T: EditorResource
    {
        foreach(var extension in resources)
        {
            if (extension is T)
            {
                return extension as T;
            }
        }

        return null;
    }

    public void AddResource(EditorResource resource)
    {
        resources.Add(resource);
    }
}

public class EditorResource: Object
{

}
