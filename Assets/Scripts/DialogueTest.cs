using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNote()
    {
        NotesManager.Instance.AddNote("Added note from dialogue.");
    }
}
