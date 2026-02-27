using UnityEngine;
// Use the CreateAssetMenu attribute to allow creating instances of this ScriptableObject from the Unity Editor.
[CreateAssetMenu(fileName = "Custom", menuName = "Custom/BarkSO", order = 1)]
public class BarkSO : ScriptableObject
{
    public string barkText;

    public GameObject questionPrefab;
    public string questionText;

}