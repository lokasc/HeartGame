using UnityEngine;
using System;

public class Patient : MonoBehaviour
{
    [SerializeField] private string patientName;
    public string PatientName => patientName;
    [SerializeField] private string patientFullName;
    public string PatientFullName => patientFullName;

    public static event Action<string> OnPatientTraitsLoaded;

    void Start()
    {
        LoadPatientTraits();
    }

    private void LoadPatientTraits()
    {
        Debug.Log($"Loading traits for patient {patientName}");
        var traits = TraitManager.Instance.GetTraitsByPatient(patientName);
        Debug.Log($"Patient {patientName} loaded {traits.Count} traits");

        TraitManager.Instance.CurrentPatientName = patientName;
        TraitManager.Instance.SetPatientFullName(patientFullName);
        
        OnPatientTraitsLoaded?.Invoke(patientName);
    }

}
