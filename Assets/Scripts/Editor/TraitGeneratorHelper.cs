using UnityEngine;
using UnityEditor;

public class TraitGeneratorHelper
{
    /// <summary>
    /// Call this method at the end of your CSV trait generation process
    /// to automatically refresh all trait references in managers
    /// </summary>
    [MenuItem("Tools/Refresh All Trait References")]
    public static void RefreshAllTraitReferences()
    {
        // Find and update TraitManager if it exists in the scene
        TraitManager traitManager = Object.FindFirstObjectByType<TraitManager>();
        if (traitManager != null)
        {
            traitManager.RefreshTraitReferences();
        }
        else
        {
            Debug.LogWarning("TraitManager not found in current scene. Make sure it exists and try again.");
        }

        // Find and update BarkManager if it exists in the scene
        BarkManager barkManager = Object.FindFirstObjectByType<BarkManager>();
        if (barkManager != null)
        {
            barkManager.RefreshBarkReferences();
        }
        else
        {
            Debug.LogWarning("BarkManager not found in current scene. Make sure it exists and try again.");
        }

        ChoiceManager choiceManager = Object.FindFirstObjectByType<ChoiceManager>();
        if (choiceManager != null)
        {
            choiceManager.RefreshChoiceReferences();
        }
        else
        {
            Debug.LogWarning("ChoiceManager not found in current scene. Make sure it exists and try again.");
        }


        NotesManager notesManager = Object.FindFirstObjectByType<NotesManager>();
        if (notesManager != null)
        {
            notesManager.RefreshNoteReferences();
        }
        else
        {
            Debug.LogWarning("NotesManager not found in current scene. Make sure it exists and try again.");
        }


        AssetDatabase.SaveAssets();
        Debug.Log("All trait, bark, and choice references refreshed successfully!");
    }

    /// <summary>
    /// Programmatic version for calling from your CSV generation script
    /// </summary>
    public static void RefreshTraitReferencesFromScript()
    {
        RefreshAllTraitReferences();
    }
}