using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class NotesManager : MonoBehaviour
{
    public static NotesManager Instance { get; private set; }

    [SerializeField] private NoteSO[] allNotes; // Auto-populated by weird editor script magic
    [SerializeField] private List<NoteSO> noteList = new List<NoteSO>();

    [SerializeField] private GameObject notesPanel;
    [SerializeField] private GameObject notePrefab;

    public int TotalSymptomShares = 0;
    public List<string> UniqueSymptoms = new List<string>();
    
    public int SymptomSharesCaught = 0;
    public List<string> UniqueSymptomsCaught = new List<string>();

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

    private void OnEnable()
    {
        Patient.OnPatientTraitsLoaded += HandlePatientTraitsLoaded;
    }

    private void OnDisable()
    {
        Patient.OnPatientTraitsLoaded -= HandlePatientTraitsLoaded;
    }

    private void HandlePatientTraitsLoaded(string patientName)
    {
        noteList.Clear();
        
        if (allNotes != null)
        {
            foreach (NoteSO note in allNotes)
            {
                if (note.patientName == patientName)
                {
                    noteList.Add(note);
                }
            }
        }

        GenerateUniqueSymptomsList();
        Debug.Log($"Loaded {noteList.Count} notes for patient {patientName}. Unique symptoms caught: {UniqueSymptoms.Count}. Total symptom shares: {TotalSymptomShares}.");
        
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

    private void GenerateUniqueSymptomsList()
    {
        UniqueSymptoms.Clear();
        TotalSymptomShares = 0;

        foreach (NoteSO note in noteList)
        {
            if (!UniqueSymptoms.Contains(note.symptomCaught))
            {
                UniqueSymptoms.Add(note.symptomCaught);
                int shares = note.symptomSharesCaught;
                TotalSymptomShares += shares;
            }
        }
    }


    public void AddNote(string noteName)
    {
        NoteSO currentNote = noteList.FirstOrDefault(n => n.noteName == noteName);
        GameObject note = Instantiate(notePrefab, Vector3.zero, Quaternion.identity, notesPanel.transform);

        // Get the Text component from the instantiated prefab and set the note text
        TMPro.TMP_Text noteTextMesh = note.GetComponentInChildren<TMPro.TMP_Text>();
        if (noteTextMesh != null)
        {
            noteTextMesh.text = currentNote != null ? currentNote.noteText : "There was no text in the SO for this note.";
        }

        UpdateSymptomScore(currentNote);
    }

    private void UpdateSymptomScore(NoteSO note)
    {
        if (note == null) return;

        if (!UniqueSymptomsCaught.Contains(note.symptomCaught))
        {
            UniqueSymptomsCaught.Add(note.symptomCaught);
            SymptomSharesCaught += note.symptomSharesCaught;
            Debug.Log($"Caught symptom: {note.symptomCaught}. Shares caught = {SymptomSharesCaught}/{TotalSymptomShares}");
        }
    }

#if UNITY_EDITOR
    public void RefreshNoteReferences()
    {
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:NoteSO");
        allNotes = new NoteSO[guids.Length];
        
        for (int i = 0; i < guids.Length; i++)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            allNotes[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<NoteSO>(path);
        }
        
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEngine.Debug.Log($"Auto-populated {allNotes.Length} note references in NotesManager");
    }
#endif
}
