using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

#if UNITY_EDITOR
using UnityEditor.Rendering;
using UnityEditor;
#endif

public class ChartManager : MonoBehaviour
{
    public static ChartManager Instance { get; private set; }

    [SerializeField] private List<TraitSO> chartTraits;

    [SerializeField] private TMP_Text nameSlot;
    [SerializeField] private TMP_Text ageSlot;
    [SerializeField] private TMP_Text dobSlot;
    [SerializeField] private TMP_Text sexSlot;
    [SerializeField] private TMP_Text pronounSlot;
    [SerializeField] private TMP_Text weightSlot;
    [SerializeField] private TMP_Text raceSlot;
    [SerializeField] private TMP_Text tempSlot;
    [SerializeField] private TMP_Text o2Slot; 
    [SerializeField] private TMP_Text bpSlot;
    [SerializeField] private TMP_Text rrSlot;
    [SerializeField] private TMP_Text painSlot;
    [SerializeField] private TMP_Text urinarySlot;

    private TraitSO weightTrait;
    private TraitSO raceTrait;
    private TraitSO fullNameTrait;
    private TraitSO ageTrait;
    private TraitSO dobTrait;
    private TraitSO sexTrait;
    private TraitSO pronounsTrait;
    private TraitSO tempTrait;
    private TraitSO o2Trait;
    private TraitSO bpTrait;
    private TraitSO rrTrait;
    private TraitSO painTrait;
    private TraitSO urinaryTrait;

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
        chartTraits = TraitManager.Instance.GetTraitsByRepresentedOn("chart");

        foreach (TraitSO trait in chartTraits)
        {
            Debug.Log($"Chart trait found: {trait.traitName} with data: {trait.traitData}");
        }


        ImportChartTrait("fullName", nameSlot, out TraitSO fullNameTrait);
        this.fullNameTrait = fullNameTrait;
        ImportChartTrait("age", ageSlot, out TraitSO ageTrait);
        this.ageTrait = ageTrait;
        ImportChartTrait("dob", dobSlot, out TraitSO dobTrait);
        this.dobTrait = dobTrait;
        ImportChartTrait("sexatbirth", sexSlot, out TraitSO sexTrait);
        this.sexTrait = sexTrait;
        ImportChartTrait("pronouns", pronounSlot, out TraitSO pronounsTrait);
        this.pronounsTrait = pronounsTrait;
        ImportChartTrait("weight", weightSlot, out TraitSO weightTrait);
        this.weightTrait = weightTrait;
        ImportChartTrait("race", raceSlot, out TraitSO raceTrait);
        this.raceTrait = raceTrait;
        ImportChartTrait("temperature", tempSlot, out TraitSO tempTrait);
        this.tempTrait = tempTrait;
        ImportChartTrait("o2", o2Slot, out TraitSO o2Trait);
        this.o2Trait = o2Trait;
        ImportChartTrait("bp", bpSlot, out TraitSO bpTrait);
        this.bpTrait = bpTrait;
        ImportChartTrait("rr", rrSlot, out TraitSO rrTrait);
        this.rrTrait = rrTrait;
        ImportChartTrait("pain", painSlot, out TraitSO painTrait);
        this.painTrait = painTrait;
        ImportChartTrait("urinary", urinarySlot, out TraitSO urinaryTrait);
        this.urinaryTrait = urinaryTrait;
    }

    private void ImportChartTrait(string traitSuffix, TMP_Text targetSlot, out TraitSO foundTrait){
        Debug.Log($"Importing trait for {TraitManager.Instance.CurrentPatientName}_{traitSuffix}");
        TraitSO trait = chartTraits.FirstOrDefault(trait => trait.traitName == $"{TraitManager.Instance.CurrentPatientName}_{traitSuffix}");
        Debug.Log(trait != null ? $"Found trait {trait.traitName} with data: {trait.traitData}" : $"Trait {TraitManager.Instance.CurrentPatientName}_{traitSuffix} not found");
        if (trait != null)
        {
            targetSlot.text = trait.traitData;
        }
        foundTrait = trait;
    }

    public void ClickWeightButton(){
        if(weightTrait.triggersDialogue !=null){
            ChoiceManager.Instance.ShowChoice(weightTrait.triggersDialogue);
        }
    }

    public void ClickTempButton(){
        if(tempTrait.triggersDialogue !=null){
            ChoiceManager.Instance.ShowChoice(tempTrait.triggersDialogue);
        }
    }

    public void ClickPainButton(){
        if(painTrait.triggersDialogue !=null){
            ChoiceManager.Instance.ShowChoice(painTrait.triggersDialogue);
        }
    }

    public void ClickUrinaryButton(){
        if(urinaryTrait.triggersDialogue !=null){
            ChoiceManager.Instance.ShowChoice(urinaryTrait.triggersDialogue);
        }
    }
}