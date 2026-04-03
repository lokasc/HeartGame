using UnityEngine;
// Use the CreateAssetMenu attribute to allow creating instances of this ScriptableObject from the Unity Editor.
[CreateAssetMenu(fileName = "Custom", menuName = "Custom/BarkSO")]
public class BarkSO : ScriptableObject
{
    public string patientName;
    public string barkName;
    public string barkText;
    public string triggersDialogue;
}