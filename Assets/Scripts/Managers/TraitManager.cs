using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TraitManager : MonoBehaviour
{    
    public static TraitManager Instance { get; private set; }

    public string currentPatientName;

    [SerializeField] private TraitSO[] allTraits; // Auto-populated by weird editor script magic

    [SerializeField] private List<TraitSO> patientTraits;
    
#if UNITY_EDITOR
    public void RefreshTraitReferences()
    {
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:TraitSO");
        allTraits = new TraitSO[guids.Length];
        
        for (int i = 0; i < guids.Length; i++)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            allTraits[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<TraitSO>(path);
        }
        
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEngine.Debug.Log($"Auto-populated {allTraits.Length} trait references in TraitManager");
    }
#endif

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

    public List<TraitSO> GetTraitsByPatient(string patientName)
    {
        if (allTraits == null) 
        {
            patientTraits = new List<TraitSO>();
            return patientTraits;
        }
        
        patientTraits = allTraits.Where(trait => trait.patientName == patientName).ToList();
        return patientTraits;
    }

    public List<TraitSO> GetTraitsByRepresentedOn(string representedOnValue)
    {
        if (patientTraits == null) return new List<TraitSO>();
        return patientTraits.Where(trait => trait.representedOn == representedOnValue).ToList();
    }
}
