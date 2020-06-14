using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * Base class for stylized components of editor extensions 
 * 
 * Based on a tutorial by Christina Qi
 */
public abstract class BaseStylizedEditorComponent
{
    public bool isVerticalLayout = true;

    public int width = 0;
    public int height = 0;

    public GUIStyle style;

    public Color backgroundColor = Color.white;
    public Color fontColor = Color.black;

    public BaseStylizedEditorComponent(RectOffset margin = null)
    {
        if (margin == null)
        {
            margin = new RectOffset(0, 0, 0, 0);
        }
        style = new GUIStyle()
        {
            normal =
            {
                background = EditorGUIUtility.whiteTexture
            },
            margin = margin
        };
    }

    public void Draw() 
    {
        // get the UI colors so we can restore them after we render this component
        Color color = GUI.color;
        Color contentColor = GUI.contentColor;

        // swap out with our specified colors
        GUI.color = backgroundColor;
        GUI.contentColor = fontColor;

        if (isVerticalLayout)
        {
            GUILayout.BeginVertical(style, GUILayout.Width(width), GUILayout.Height(height));
        }
        else
        {
            GUILayout.BeginHorizontal(style, GUILayout.Width(width), GUILayout.Height(height));
        }

        DrawInnerComponents();

        if (isVerticalLayout)
        {
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.EndHorizontal();
        }

        // restore the default UI colors
        GUI.color = color;
        GUI.contentColor = contentColor;
    }

    protected abstract void DrawInnerComponents();
}
