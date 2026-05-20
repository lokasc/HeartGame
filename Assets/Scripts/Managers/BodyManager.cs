using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public class BodyManager : MonoBehaviour
{
    public static BodyManager Instance { get; private set; }

    [SerializeField] private List<TraitSO> bodyTraits;

    [SerializeField] private Button neckButton;
    [SerializeField] private Button abdomenButton;
    [SerializeField] private Button armButton;
    [SerializeField] private Button footButton;
    [SerializeField] private Button handButton;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        Patient.OnPatientTraitsLoaded += HandlePatientTraitsLoaded;
        neckButton.onClick.AddListener(() => ShowTraitDialogue("neck"));
        abdomenButton.onClick.AddListener(() => ShowTraitDialogue("abdomen"));
        armButton.onClick.AddListener(() => ShowTraitDialogue("arm"));
        footButton.onClick.AddListener(() => ShowTraitDialogue("foot"));
        handButton.onClick.AddListener(() => ShowTraitDialogue("hand"));
    }

    private void OnDisable()
    {
        Patient.OnPatientTraitsLoaded -= HandlePatientTraitsLoaded;
            neckButton.onClick.RemoveAllListeners();
            abdomenButton.onClick.RemoveAllListeners();
            armButton.onClick.RemoveAllListeners();
            footButton.onClick.RemoveAllListeners();
            handButton.onClick.RemoveAllListeners();    
    }


    private void HandlePatientTraitsLoaded(string patientName)
    {
        bodyTraits = TraitManager.Instance.GetTraitsByRepresentedOn("body");

        List<TraitSO> traitsToRemove = new List<TraitSO>();

        foreach (TraitSO trait in bodyTraits)
        {
            if(trait.triggersDialogue == null || trait.triggersDialogue == "")
            {
                traitsToRemove.Add(trait);
                continue;
            }
            Debug.Log($"Body trait found: {trait.traitName} with data: {trait.traitData}");
        }

        foreach(TraitSO trait in traitsToRemove)
        {
            bodyTraits.Remove(trait);
        }
    }

    private void ShowTraitDialogue(string bodyPart)
    {
        Debug.Log($"Button clicked for {bodyPart}");
        TraitSO traitToShow = bodyTraits.Find(trait => trait.traitData == bodyPart);
        if (traitToShow != null)
        {
            ChoiceManager.Instance.ShowChoice(traitToShow.triggersDialogue);
        }
        else
        {
            Debug.Log($"No trait found for {bodyPart}");
        }
    }

}
