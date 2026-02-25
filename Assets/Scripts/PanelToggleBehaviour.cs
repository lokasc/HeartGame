using UnityEngine;
using UnityEngine.UI;

public class PanelToggleBehaviour : MonoBehaviour
{
    public float lerpTime = 3f;
    
    public Toggle toggleComponent;
    public Transform startPosition;
    public Transform endPosition;

    private Transform parentTransform;
    private Transform targetTransform;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toggleComponent = GetComponent<Toggle>();
        toggleComponent.onValueChanged.AddListener(OnValueChanged);
        
        
        parentTransform = transform.parent;
        parentTransform.position =  startPosition.position;
        targetTransform = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        parentTransform.position = Vector3.Lerp(parentTransform.position, targetTransform.position, Time.deltaTime * lerpTime);
    }

    void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            targetTransform = endPosition;
        }
        else
        {
            targetTransform = startPosition;
        }
        
        
    }
}
