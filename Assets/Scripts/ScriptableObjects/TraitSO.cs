using UnityEngine;

// Use the CreateAssetMenu attribute to allow creating instances of this ScriptableObject from the Unity Editor.
[CreateAssetMenu(fileName = "Custom", menuName = "Custom/TraitSO")]
public class TraitSO : ScriptableObject
{
    public string patientName;
    public string traitName;
    public string traitData;
    public string representedOn;
    public string socketGO;
    public string triggersDialogue;

    public bool IsRepresentedOn(string component)
    {
        return representedOn == component;
    }
}