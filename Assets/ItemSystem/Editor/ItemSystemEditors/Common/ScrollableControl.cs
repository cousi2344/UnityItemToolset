using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

/**
 * Wrapper class for scrolling ability in an editor window
 * 
 * Based on a tutorial by Christina Qi
 */
public class ScrollableControl
{
    public bool enabled = false;

    public Vector2 scrollPosition = Vector2.zero;

    public void BeginScroll()
    {
        if (!enabled)
        {
            return;
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
    }

    public void EndScroll()
    {
        if (!enabled)
        {
            return;
        }

        EditorGUILayout.EndScrollView();
    }
}
