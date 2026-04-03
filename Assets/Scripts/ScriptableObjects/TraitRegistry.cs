using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Registry for all traits in the game. Auto-populated by editor scripts.
/// This is an alternative to manually assigning references in managers.
/// </summary>
[CreateAssetMenu(fileName = "TraitRegistry", menuName = "Custom/TraitRegistry")]
public class TraitRegistry : ScriptableObject
{
    [SerializeField] public TraitSO[] allTraits;

    /// <summary>
    /// Get all traits where representedOn equals the specified value
    /// </summary>
    public List<TraitSO> GetTraitsByRepresentedOn(string representedOnValue)
    {
        if (allTraits == null) return new List<TraitSO>();
        return allTraits.Where(trait => trait.representedOn == representedOnValue).ToList();
    }

    /// <summary>
    /// Get all traits where representedOn equals 'bark'
    /// </summary>
    public List<TraitSO> GetBarkTraits()
    {
        return GetTraitsByRepresentedOn("bark");
    }
}