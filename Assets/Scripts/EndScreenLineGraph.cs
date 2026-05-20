using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NaughtyAttributes;

public class EndScreenLineGraph : MonoBehaviour
{
    [Header("Data")] [SerializeField] public List<DataPoint> dataPoints = new List<DataPoint> { };

    public static EndScreenLineGraph instance;

    [Header("References")]
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private Sprite dotSprite;
    [SerializeField] private GameObject tooltipPrefab;

    [Header("Graph Settings")]
    [SerializeField] private float dotSize = 14f;
    [SerializeField] private float lineThickness = 3f;

    [Header("Colors")]
    [SerializeField] private Color lineColor      = new Color(1f, 1f, 1f, 0.8f);
    [SerializeField] private Color dotColorNormal = Color.white;
    [SerializeField] private Color dotColorHover  = Color.cyan;

    [Header("Axes")]
    [SerializeField] private float axisThickness  = 2f;
    [SerializeField] private Color axisColor      = new Color(1f, 1f, 1f, 0.5f);

    private readonly List<GameObject> _graphObjects = new List<GameObject>();

    private GameObject _horizontalAxis;
    private GameObject _verticalAxis;

    public class DataPoint
    {
        public float NewEmpathyValue;
        public float EmpathyChange;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    [Button]
    public void BuildGraph()
    {
        ShowGraph(dataPoints);
    }

    // ---------------------------------------------------------------
    //  Public API
    // ---------------------------------------------------------------

    public void AddPoint(DataPoint newData)
    {
        dataPoints.Add(newData);
    }

    public void ShowGraph(List<DataPoint> points)
    {
        ClearGraph();

        if (points == null || points.Count == 0) return;

        float w    = graphContainer.sizeDelta.x;
        float h    = graphContainer.sizeDelta.y;

        // Find yMin/yMax, always including 0 so the zero line is visible
        float yMin = 0f;
        float yMax = 0f;
        foreach (var dp in points)
        {
            yMin = Mathf.Min(yMin, dp.NewEmpathyValue);
            yMax = Mathf.Max(yMax, dp.NewEmpathyValue);
        }
        float yRange = yMax - yMin;
        if (yRange == 0f) yRange = 1f;

        // Y position (in pixels) that corresponds to value 0
        float yZero = (-yMin / yRange) * h;

        float xSpacing = points.Count > 1 ? w / (points.Count - 1) : w / 2f;

        // Draw axes first so they sit behind everything
        CreateAxes(w, h, yZero);

        GameObject prevDot = null;

        for (int i = 0; i < points.Count; i++)
        {
            float x = points.Count > 1 ? xSpacing * i : w / 2f;
            float y = ((points[i].NewEmpathyValue - yMin) / yRange) * h;

            GameObject dot = CreateDot(new Vector2(x, y), i, points[i]);
            _graphObjects.Add(dot);

            if (prevDot != null)
            {
                GameObject line = CreateLine(
                    prevDot.GetComponent<RectTransform>().anchoredPosition,
                    dot.GetComponent<RectTransform>().anchoredPosition
                );
                _graphObjects.Add(line);
                line.transform.SetSiblingIndex(0);
            }

            prevDot = dot;
        }
    }

    public void ClearGraph()
    {
        foreach (var go in _graphObjects)
            if (go != null) Destroy(go);

        _graphObjects.Clear();

        if (_horizontalAxis != null) { Destroy(_horizontalAxis); _horizontalAxis = null; }
        if (_verticalAxis   != null) { Destroy(_verticalAxis);   _verticalAxis   = null; }
    }

    // ---------------------------------------------------------------
    //  Internal builders
    // ---------------------------------------------------------------

    private void CreateAxes(float w, float h, float yZero = 0f)
    {
        // --- Horizontal axis (zero line) ---
        _horizontalAxis = new GameObject("Axis_Horizontal", typeof(Image));
        _horizontalAxis.transform.SetParent(graphContainer, false);
        _horizontalAxis.transform.SetSiblingIndex(0);

        var hImg           = _horizontalAxis.GetComponent<Image>();
        hImg.color         = axisColor;
        hImg.raycastTarget = false;

        var hRt              = _horizontalAxis.GetComponent<RectTransform>();
        hRt.anchorMin        = Vector2.zero;
        hRt.anchorMax        = Vector2.zero;
        hRt.pivot            = new Vector2(0f, 0.5f);
        hRt.sizeDelta        = new Vector2(w, axisThickness);
        hRt.anchoredPosition = new Vector2(0f, yZero);

        // --- Vertical axis (Y axis) ---
        _verticalAxis = new GameObject("Axis_Vertical", typeof(Image));
        _verticalAxis.transform.SetParent(graphContainer, false);
        _verticalAxis.transform.SetSiblingIndex(0);

        var vImg           = _verticalAxis.GetComponent<Image>();
        vImg.color         = axisColor;
        vImg.raycastTarget = false;

        var vRt              = _verticalAxis.GetComponent<RectTransform>();
        vRt.anchorMin        = Vector2.zero;
        vRt.anchorMax        = Vector2.zero;
        vRt.pivot            = new Vector2(0.5f, 0f);
        vRt.sizeDelta        = new Vector2(axisThickness, h);
        vRt.anchoredPosition = Vector2.zero;
    }

    private GameObject CreateDot(Vector2 position, int index, DataPoint dataPoint)
    {
        var go = new GameObject($"Dot_{index}", typeof(Image));
        go.transform.SetParent(graphContainer, false);

        var img    = go.GetComponent<Image>();
        img.sprite = dotSprite
            ? dotSprite
            : Resources.GetBuiltinResource<Sprite>("UI/Skin/Knob.psd");
        img.color         = dotColorNormal;
        img.raycastTarget = true;

        var rt              = go.GetComponent<RectTransform>();
        rt.anchorMin        = Vector2.zero;
        rt.anchorMax        = Vector2.zero;
        rt.sizeDelta        = new Vector2(dotSize, dotSize);
        rt.anchoredPosition = position;

        GameObject tooltip = CreateTooltip(go, index, dataPoint);

        var trigger = go.AddComponent<EventTrigger>();
        AddTrigger(trigger, EventTriggerType.PointerEnter, _ => HoverDot(go, tooltip, true));
        AddTrigger(trigger, EventTriggerType.PointerExit,  _ => HoverDot(go, tooltip, false));

        return go;
    }

    private GameObject CreateTooltip(GameObject parent, int index, DataPoint dataPoint)
    {
        GameObject tooltip;

        if (tooltipPrefab != null)
        {
            tooltip = Instantiate(tooltipPrefab, parent.transform);
            tooltip.GetComponent<FeedbackBox>().SetText(dataPoint.EmpathyChange, dataPoint.NewEmpathyValue);
        }
        else
        {
            tooltip = new GameObject("Tooltip", typeof(Image));
            tooltip.transform.SetParent(parent.transform, false);

            var bg   = tooltip.GetComponent<Image>();
            bg.color = new Color(0f, 0f, 0f, 0.75f);

            var rt              = tooltip.GetComponent<RectTransform>();
            rt.pivot            = new Vector2(0.5f, 0f);
            rt.sizeDelta        = new Vector2(80f, 30f);
            rt.anchoredPosition = new Vector2(0f, dotSize + 10f);

            var labelGO = new GameObject("Label", typeof(Text));
            labelGO.transform.SetParent(tooltip.transform, false);

            var text           = labelGO.GetComponent<Text>();
            // Show both values from the DataPoint
            text.text          = $"Empathy: {dataPoint.NewEmpathyValue}\nChange: {dataPoint.EmpathyChange:+0.#;-0.#;0}";
            text.fontSize      = 12;
            text.alignment     = TextAnchor.MiddleCenter;
            text.color         = Color.white;
            text.font          = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.raycastTarget = false;

            var textRt       = labelGO.GetComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.offsetMin = Vector2.zero;
            textRt.offsetMax = Vector2.zero;
        }

        tooltip.SetActive(false);
        return tooltip;
    }

    private GameObject CreateLine(Vector2 from, Vector2 to)
    {
        var go = new GameObject("Line", typeof(Image));
        go.transform.SetParent(graphContainer, false);

        var img           = go.GetComponent<Image>();
        img.color         = lineColor;
        img.raycastTarget = false;

        Vector2 dir  = (to - from).normalized;
        float   dist = Vector2.Distance(from, to);

        var rt              = go.GetComponent<RectTransform>();
        rt.anchorMin        = Vector2.zero;
        rt.anchorMax        = Vector2.zero;
        rt.sizeDelta        = new Vector2(dist, lineThickness);
        rt.anchoredPosition = from + dir * (dist * 0.5f);
        rt.localEulerAngles = new Vector3(0f, 0f,
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        return go;
    }

    // ---------------------------------------------------------------
    //  Interaction
    // ---------------------------------------------------------------

    private void HoverDot(GameObject dot, GameObject tooltip, bool entering)
    {
        dot.GetComponent<Image>().color = entering ? dotColorHover : dotColorNormal;
        tooltip.SetActive(entering);
    }

    // ---------------------------------------------------------------
    //  Utility
    // ---------------------------------------------------------------

    private static void AddTrigger(EventTrigger et,
        EventTriggerType type, Action<BaseEventData> cb)
    {
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(data => cb(data));
        et.triggers.Add(entry);
    }
}