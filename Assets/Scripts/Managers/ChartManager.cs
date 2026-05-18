using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;




#if UNITY_EDITOR
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
    [SerializeField] private TMP_Text edemaSlot;
    [SerializeField] private TMP_Text efSlot;
    [SerializeField] private TMP_Text hrSlot;

    [SerializeField] private TMP_Text vitalsNameSlot;
    [SerializeField] private TMP_Text intakeNameSlot;


    [SerializeField] private Button personalIngoButton;
    [SerializeField] private Button vitalsInfoButton;
    [SerializeField] private Button instakeInfoButton;

    [SerializeField] private GameObject personalInfoPanel;
    [SerializeField] private GameObject vitalsPanel;
    [SerializeField] private GameObject intakePanel;


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
    private TraitSO edemaTrait;
    private TraitSO efTrait;
    private TraitSO hrTrait;

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

    private void Start()
    {
        personalIngoButton.onClick.AddListener(() => ShowPanel(personalInfoPanel));
        vitalsInfoButton.onClick.AddListener(() => ShowPanel(vitalsPanel));
        instakeInfoButton.onClick.AddListener(() => ShowPanel(intakePanel));

        personalInfoPanel.SetActive(false);
        vitalsPanel.SetActive(false);
        intakePanel.SetActive(false);

        ShowPanel(personalInfoPanel);
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
        ImportChartTrait("edema", edemaSlot, out TraitSO edemaTrait);
        this.edemaTrait = edemaTrait;
        ImportChartTrait("ef", efSlot, out TraitSO efTrait);
        this.efTrait = efTrait;
        ImportChartTrait("hr", hrSlot, out TraitSO hrTrait);
        this.hrTrait = hrTrait;

        UpdatePanelNameSlots();
    }

    private void UpdatePanelNameSlots()
    {
        vitalsNameSlot.text = $"{fullNameTrait.traitData}";
        intakeNameSlot.text = $"{fullNameTrait.traitData}";
    }

    private void ShowPanel(GameObject panelToShow)
    {
        personalInfoPanel.SetActive(panelToShow == personalInfoPanel);
        vitalsPanel.SetActive(panelToShow == vitalsPanel);
        intakePanel.SetActive(panelToShow == intakePanel);
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
}