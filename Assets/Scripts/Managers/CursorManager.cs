using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Textures")]
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D clickCursor;

    [Header("Hotspots")]
    [SerializeField] private Vector2 defaultHotspot = Vector2.zero;
    [SerializeField] private Vector2 clickHotspot = new Vector2(8f, 8f);

    [Header("Settings")]
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        ApplyDefaultCursor();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ApplyClickCursor();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ApplyDefaultCursor();
        }
    }

    private void ApplyDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, defaultHotspot, cursorMode);
    }

    private void ApplyClickCursor()
    {
        Cursor.SetCursor(clickCursor, clickHotspot, cursorMode);
    }

    public void ShowClickCursor() => ApplyClickCursor();
    public void ShowDefaultCursor() => ApplyDefaultCursor();
}
