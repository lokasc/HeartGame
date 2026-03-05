using NaughtyAttributes;
using UnityEngine;

public enum SymptomType
{
    Audio,
    Visual,
    Paper,
    Dialogue
}

public enum Priority
{
    Critical,
    High,
    Medium,
    Low
}

// Use the CreateAssetMenu attribute to allow creating instances of this ScriptableObject from the Unity Editor.
[CreateAssetMenu(fileName = "Custom", menuName = "Custom/SymptomSO", order = 1)]
public class SymptomSO : ScriptableObject
{
    [ReadOnly] public int id; // id of the symptom (don't know if we'll ever use this, good for system)
    public string internalName;
    public SymptomType symptomType;
    public string displayName;
    public Priority priority;
    
    public GameObject prefab; /// The idea is that in the future the manager who reads these symptoms will know what to do with this prefab
    [ResizableTextArea]
    public string description;

    [ResizableTextArea]
    public string devNotes;
}
