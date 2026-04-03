using UnityEngine;
using System;

public class Patient : MonoBehaviour
{
    [SerializeField] private string patientName;
    public string PatientName => patientName;

    public static event Action<string> OnPatientTraitsLoaded;

    void Start()
    {
        LoadPatientTraits();
    }

    private void LoadPatientTraits()
    {
        var traits = TraitManager.Instance.GetTraitsByPatient(patientName);
        Debug.Log($"Patient {patientName} loaded {traits.Count} traits");

        TraitManager.Instance.currentPatientName = patientName;
        
        OnPatientTraitsLoaded?.Invoke(patientName);
    }

}
