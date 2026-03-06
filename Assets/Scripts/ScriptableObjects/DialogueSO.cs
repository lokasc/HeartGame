using NaughtyAttributes;
using UnityEngine;

// Use the CreateAssetMenu attribute to allow creating instances of this ScriptableObject from the Unity Editor.
[CreateAssetMenu(fileName = "Custom", menuName = "Custom/DialogueSO", order = 1)]
public class DialogueSO : ScriptableObject
{
    public enum Origin
    {
        Chart = 1 << 0,
        RedHerring = 1 << 1,
        Audio = 1 << 2,
        Medicine = 1 << 3,
        Visual = 1 << 4,
        PreExist = 1 << 5,
        Ambience = 1 << 6,
    }
    
    
    [ReadOnly] public int id; // id of the symptom (don't know if we'll ever use this, good for system)
    public string internalName;
    
    [EnumFlags]
    public Origin origin;
    public string displayName;
    [HorizontalLine(color : EColor.Black)]
    
    [TextArea]
    public string dialogueOne;
    [TextArea]
    public string dialogueTwo;
    [TextArea]
    public string dialogueThree;
    
    [HorizontalLine(color : EColor.Black)]
    
    public string nextSO;
    public string symptomSO;
    
    [ResizableTextArea]
    public string devNotes;
}
