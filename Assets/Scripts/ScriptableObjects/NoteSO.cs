using UnityEngine;
// Use the CreateAssetMenu attribute to allow creating instances of this ScriptableObject from the Unity Editor.
[CreateAssetMenu(fileName = "Custom", menuName = "Custom/NoteSO")]
public class NoteSO : ScriptableObject
{
    public string patientName;
    public string noteName;
    public string noteText;
    public int symptomsCaught;
}