using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

/**
 * Base class for custom editor windows.
 * 
 * Based on tutorial by Christina Qi
 */
public abstract class BaseCustomEditorWindow : EditorWindow
{
    // this dictionary is non-serializable and will be set to null by Unity when the editor has been invalidated (e.g. play mode, code compile)
    private Dictionary<string, string> didConstruct;

    protected ScrollableControl scrollableControl;

    public bool Construct()
    {
        if (didConstruct != null)
        {
            return false;
        }
        didConstruct = new Dictionary<string, string>();

        scrollableControl = new ScrollableControl();

        ConfigureWindow();
        ConstructInnerWindow();

        return true;
    }

    public T GetExtension<T>() where T: EditorExtension
    {
        return MyEditorConfiguration.Instance.GetExtension<T>();
    }

    public void OnInspectorUpdate()
    {
        Repaint();
    }

    public void OnGUI()
    {
        DrawWindow();
    }

    private void DrawWindow()
    {
        if (didConstruct == null)
        {
            Construct();
        }

        scrollableControl.BeginScroll();
        DrawInnerWindow();
        scrollableControl.EndScroll();
    }

    protected abstract void ConfigureWindow();
    protected abstract void ConstructInnerWindow();
    protected abstract void DrawInnerWindow();
}
