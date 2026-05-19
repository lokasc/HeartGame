using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BodyManager : MonoBehaviour
{
    public static BodyManager Instance { get; private set; }

    [SerializeField] private List<TraitSO> bodyTraits;

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
    }

    private void OnDisable()
    {
        Patient.OnPatientTraitsLoaded -= HandlePatientTraitsLoaded;
    }


    private void HandlePatientTraitsLoaded(string patientName)
    {
        bodyTraits = TraitManager.Instance.GetTraitsByRepresentedOn("body");

        foreach (TraitSO trait in bodyTraits)
        {
            Debug.Log($"Body trait found: {trait.traitName} with data: {trait.traitData}");
        }


        // ImportChartTrait("fullName", nameSlot, out TraitSO fullNameTrait);
        // this.fullNameTrait = fullNameTrait;
        // ImportChartTrait("age", ageSlot, out TraitSO ageTrait);
        // this.ageTrait = ageTrait;
        // ImportChartTrait("dob", dobSlot, out TraitSO dobTrait);
        // this.dobTrait = dobTrait;
        // ImportChartTrait("sexatbirth", sexSlot, out TraitSO sexTrait);
        // this.sexTrait = sexTrait;
        // ImportChartTrait("pronouns", pronounSlot, out TraitSO pronounsTrait);
        // this.pronounsTrait = pronounsTrait;
        // ImportChartTrait("weight", weightSlot, out TraitSO weightTrait);
        // this.weightTrait = weightTrait;
        // ImportChartTrait("race", raceSlot, out TraitSO raceTrait);
        // this.raceTrait = raceTrait;
        // ImportChartTrait("temperature", tempSlot, out TraitSO tempTrait);
        // this.tempTrait = tempTrait;
        // ImportChartTrait("o2", o2Slot, out TraitSO o2Trait);
        // this.o2Trait = o2Trait;
        // ImportChartTrait("bp", bpSlot, out TraitSO bpTrait);
        // this.bpTrait = bpTrait;
        // ImportChartTrait("rr", rrSlot, out TraitSO rrTrait);
        // this.rrTrait = rrTrait;
        // ImportChartTrait("pain", painSlot, out TraitSO painTrait);
        // this.painTrait = painTrait;
        // ImportChartTrait("edema", edemaSlot, out TraitSO edemaTrait);
        // this.edemaTrait = edemaTrait;
        // ImportChartTrait("ef", efSlot, out TraitSO efTrait);
        // this.efTrait = efTrait;
        // ImportChartTrait("hr", hrSlot, out TraitSO hrTrait);
        // this.hrTrait = hrTrait;

        // UpdatePanelNameSlots();
    }
}
