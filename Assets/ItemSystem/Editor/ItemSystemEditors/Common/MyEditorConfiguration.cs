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
        // add extensions here

        //AddExtension(new HelloWorldEditorExtension()
        //{
        //    MyName = "Jake"
        //});

    }

    protected List<EditorExtension> extensions = new List<EditorExtension>();

    public T GetExtension<T>() where T: EditorExtension
    {
        foreach(var extension in extensions)
        {
            if (extension is T)
            {
                return extension as T;
            }
        }

        return null;
    }

    public void AddExtension(EditorExtension extension)
    {
        extensions.Add(extension);
    }
}

public class EditorExtension
{

}
