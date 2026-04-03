using UnityEngine;
// Use the CreateAssetMenu attribute to allow creating instances of this ScriptableObject from the Unity Editor.
[CreateAssetMenu(fileName = "Custom", menuName = "Custom/ChoiceSO")]
public class ChoiceSO : ScriptableObject
{
    public string patientName;
    public string choiceName;
    public string patientDialogue;
    public string optionAText;
    public string optionANext;
    public string optionBText;
    public string optionBNext;
    public string notesText;
    public string notesButtonText;
    public string notesName;
}