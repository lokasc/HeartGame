using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

[UnityEditor.InitializeOnLoad]
#endif
public class ColorInHierarchy : MonoBehaviour
{
#if UNITY_EDITOR
    #region [ static - Update editor ]
    private static Dictionary<UnityEngine.Object, Color> coloredText = new Dictionary<UnityEngine.Object, Color>();
    private static Dictionary<UnityEngine.Object, Color> coloredBackground = new Dictionary<UnityEngine.Object, Color>();
    private static Dictionary<UnityEngine.Object, Sprite> coloredIcon = new Dictionary<UnityEngine.Object, Sprite>();
    private static Vector2 offset = new Vector2(18, 0);
    private static Color widowBackground = new Color(0.2196079f, 0.2196079f, 0.2196079f, 1f);

    static ColorInHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID);

        if (obj != null && coloredBackground.ContainsKey(obj))
        {
            if ((obj as GameObject).GetComponent<ColorInHierarchy>())
            {
                HierarchyBackground(selectionRect, coloredBackground[obj]);
            }
            else
            {
                coloredBackground.Remove(obj);
            }
        }

        if (obj != null && coloredIcon.ContainsKey(obj))
        {
            if ((obj as GameObject).GetComponent<ColorInHierarchy>())
            {
                HierarchyIconBg(selectionRect);
                HierarchyIcon(selectionRect, coloredIcon[obj]);
            }
            else
            {
                coloredIcon.Remove(obj);
            }
        }

        if (obj != null && coloredText.ContainsKey(obj))
        {
            if ((obj as GameObject).GetComponent<ColorInHierarchy>())
            {
                HierarchyText(obj, selectionRect, coloredText[obj]);
            }
            else
            {
                coloredText.Remove(obj);
            }
        }
    }

    private static void HierarchyText(UnityEngine.Object obj, Rect selectionRect, Color colorText)
    {
        Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);

        EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
        {
            normal = new GUIStyleState() { textColor = colorText },
            fontStyle = FontStyle.Normal
        }
        );
    }

    private static void HierarchyBackground(Rect selectionRect, Color colorBackground)
    {
        Rect bgRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 50, selectionRect.height);
        EditorGUI.DrawRect(bgRect, colorBackground);
    }

    private static void HierarchyIconBg(Rect selectionRect)
    {
        Rect icRect = new Rect(selectionRect.x - 2, selectionRect.y, 20, 18);
        Color windowBg = widowBackground;
        EditorGUI.DrawRect(icRect, windowBg);
    }
    private static void HierarchyIcon(Rect selectionRect, Sprite colorIcon)
    {
        Rect icRect = new Rect(selectionRect.x - 2, selectionRect.y - 2, 20, 20);
        Texture2D tex = colorIcon.texture;
        GUI.Label(icRect, tex);
    }
    #endregion [ static - Update editor ]

    public Color colorText = Color.white;
    public Color colorBackground = Color.grey;
    public Sprite icon;

    private void Reset()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        SetAppearance();
    }

    private void Awake()
    {
        SetAppearance();
    }


    private void SetAppearance()
    {
        if (false == coloredBackground.ContainsKey(this.gameObject))
        {
            coloredBackground.Add(this.gameObject, colorBackground);
        }
        else if (coloredBackground[this.gameObject] != colorBackground)
        {
            coloredBackground[this.gameObject] = colorBackground;
        }

        if (icon)
        {
            if (icon && (false == coloredIcon.ContainsKey(this.gameObject)))
            {
                coloredIcon.Add(this.gameObject, icon);
            }
            else if (coloredIcon[this.gameObject] != icon)
            {
                coloredIcon[this.gameObject] = icon;
            }
        }

        if (false == coloredText.ContainsKey(this.gameObject))
        {
            coloredText.Add(this.gameObject, colorText);
        }
        else if (coloredText[this.gameObject] != colorText)
        {
            coloredText[this.gameObject] = colorText;
        }
    }
#endif
}