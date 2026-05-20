using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private Camera mainCamera;
    private Vector3 defaultPosition;
    private int footOffset = 6;
    [SerializeField] private Toggle footButton;

    private bool isLookingAtFoot = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultPosition = mainCamera.transform.position;
        footButton.onValueChanged.AddListener(delegate { ToggleFootView(); });
    }

    private void ToggleFootView()
    {
        if (isLookingAtFoot)
        {
            mainCamera.transform.DOMove(defaultPosition, 1f);
        }
        else
        {
            mainCamera.transform.DOMove(defaultPosition + Vector3.down * footOffset, 1f);
        }
        isLookingAtFoot = !isLookingAtFoot;
    }
}
