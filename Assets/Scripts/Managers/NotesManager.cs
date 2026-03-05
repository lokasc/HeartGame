using UnityEngine;

public class NotesManager : MonoBehaviour
{
    public static NotesManager Instance { get; private set; }

    [SerializeField] private GameObject notesPanel;
    [SerializeField] private GameObject notePrefab;
    
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

    public void AddNote(string noteText)
    {
        // Instantiate the note prefab at the desired position and rotation
        GameObject note = Instantiate(notePrefab, Vector3.zero, Quaternion.identity, notesPanel.transform);

        // Get the Text component from the instantiated prefab and set the note text
        TMPro.TMP_Text noteTextMesh = note.GetComponentInChildren<TMPro.TMP_Text>();
        if (noteTextMesh != null)
        {
            noteTextMesh.text = noteText;
        }
    }
}
