using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using Unity.EditorCoroutines.Editor;

public class SymptomsGenerator : MonoBehaviour
{
    [InfoBox("Publish your Google Sheet via File > Share > Publish to web > CSV. Then paste the published URLs below. To target different tabs, change the gid= number at the end of each URL (find the gid in the browser URL bar when clicking each tab).")]

    [Header("Google Sheets URLs")]
    // If these are ever changed or wrong, click file -> publish -> the table you want and CSV.
    private string traitURL  = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQTDb_aV1hB1ILQubNG1xp8HTd666F61iv1dU6r7tPIUygW5GRELSK8-_SK0P0ll27v41DWPSSJJK3M/pub?gid=0&single=true&output=csv";
    private string barkURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQTDb_aV1hB1ILQubNG1xp8HTd666F61iv1dU6r7tPIUygW5GRELSK8-_SK0P0ll27v41DWPSSJJK3M/pub?gid=1467885359&single=true&output=csv";
    private string noteURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQTDb_aV1hB1ILQubNG1xp8HTd666F61iv1dU6r7tPIUygW5GRELSK8-_SK0P0ll27v41DWPSSJJK3M/pub?gid=310640785&single=true&output=csv";
    private string choiceURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQTDb_aV1hB1ILQubNG1xp8HTd666F61iv1dU6r7tPIUygW5GRELSK8-_SK0P0ll27v41DWPSSJJK3M/pub?gid=251592851&single=true&output=csv";
    
    [Header("Output Folders")]
    private string traitFolderPath = "Assets/ScriptableObjects/Traits";
    private string barkFolderPath = "Assets/ScriptableObjects/Barks";
    private string choiceFolderPath = "Assets/ScriptableObjects/Choices";
    private string noteFolderPath = "Assets/ScriptableObjects/Notes";


    // =====================================================================
    //  SYMPTOMS
    // =====================================================================

    [Button]
    public void Generate()
    {
        EditorCoroutineUtility.StartCoroutineOwnerless(FetchAndGenerateSymptoms());
    }

    private IEnumerator FetchAndGenerateSymptoms()
    {
        Debug.Log("Fetching everything sheet from Google Sheets...");

        // For Symptoms 
        Debug.Log("Fetching traits sheet from Google Sheets...");
        using (UnityWebRequest request = UnityWebRequest.Get(traitURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch Trait sheet: " + request.error);
                yield break;
            }
            
            ProcessTraitCSV(request.downloadHandler.text);
        }
        
        Debug.Log("Fetching bark sheet from Google Sheets...");
        using (UnityWebRequest request = UnityWebRequest.Get(barkURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch bark sheet: " + request.error);
                yield break;
            }
            
            ProcessBarkCSV(request.downloadHandler.text);
        }
        
        Debug.Log("Fetching choice sheet from Google Sheets...");
        using (UnityWebRequest request = UnityWebRequest.Get(choiceURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch choice sheet: " + request.error);
                yield break;
            }
            
            ProcessChoiceCSV(request.downloadHandler.text);
        }
        
        Debug.Log("Fetching note sheet from Google Sheets...");
        using (UnityWebRequest request = UnityWebRequest.Get(noteURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch note sheet: " + request.error);
                yield break;
            }
            
            ProcessNoteCSV(request.downloadHandler.text);
        }
    }

    private void ProcessTraitCSV(string csvText)
    {
        List<string[]> rows = CSVParser.Parse(csvText);

        int counter         = 0;
        int overridedCounter = 0;

        foreach (string[] values in rows)
        {
            counter++;
            if (counter == 1) continue; // skip header row

            // CSV columns: ID, InternalName, SymptomType, DisplayName, Description, Priority, DevNotes
            TraitSO newSO = ScriptableObject.CreateInstance<TraitSO>();

            newSO.patientName = values[0];
            newSO.traitName = values[1];
            newSO.traitData = values[2];
            newSO.representedOn = values[3];
            newSO.socketGO = values[4];
            newSO.triggersDialogue = values[5];
            
            
            //newSO.devNotes = string.IsNullOrEmpty(values[6]) ? "No description provided" : values[6];

            var nameAndPath = traitFolderPath + "/" + newSO.traitName + ".asset";
            if (AssetDatabase.AssetPathExists(nameAndPath))
                overridedCounter++;

            AssetDatabase.CreateAsset(newSO, nameAndPath);
        }

        Debug.Log("Created " + counter + " Traits");
        Debug.LogWarning("Overrided " + overridedCounter + " Traits");
        AssetDatabase.SaveAssets();
    }
    
    private void ProcessBarkCSV(string csvText)
    {
        List<string[]> rows = CSVParser.Parse(csvText);

        int counter         = 0;
        int overridedCounter = 0;

        foreach (string[] values in rows)
        {
            counter++;
            if (counter == 1) continue; // skip header row

       
            BarkSO newSO = ScriptableObject.CreateInstance<BarkSO>();

            newSO.patientName = values[0];
            newSO.barkName = values[1];
            newSO.barkText = values[2];
            newSO.triggersDialogue = values[3];

            var nameAndPath = barkFolderPath + "/" + newSO.barkName + ".asset";
            if (AssetDatabase.AssetPathExists(nameAndPath))
                overridedCounter++;

            AssetDatabase.CreateAsset(newSO, nameAndPath);
        }

        Debug.Log("Created " + counter + " Barks");
        Debug.LogWarning("Overrided " + overridedCounter + " Bark");
        AssetDatabase.SaveAssets();
    }
    
    private void ProcessChoiceCSV(string csvText)
    {
        List<string[]> rows = CSVParser.Parse(csvText);

        int counter         = 0;
        int overridedCounter = 0;

        foreach (string[] values in rows)
        {
            counter++;
            if (counter == 1) continue; // skip header row

            // CSV columns: ID, InternalName, SymptomType, DisplayName, Description, Priority, DevNotes
            ChoiceSO newSO = ScriptableObject.CreateInstance<ChoiceSO>();

            newSO.patientName = values[0];
            newSO.choiceName = values[1];
            newSO.patientDialogue = values[2];
            newSO.optionAText = values[3];
            newSO.optionANext = values[4];
            newSO.optionBText = values[6];
            newSO.optionBNext = values[7];
            newSO.notesText = values[9];
            newSO.notesName = values[10];
            
            //newSO.devNotes = string.IsNullOrEmpty(values[6]) ? "No description provided" : values[6];

            var nameAndPath =  choiceFolderPath + "/" + newSO.choiceName + ".asset";
            if (AssetDatabase.AssetPathExists(nameAndPath))
                overridedCounter++;

            AssetDatabase.CreateAsset(newSO, nameAndPath);
        }

        Debug.Log("Created " + counter + " Choice");
        Debug.LogWarning("Overrided " + overridedCounter + " Choice");
        AssetDatabase.SaveAssets();
    }
   
    private void ProcessNoteCSV(string csvText)
    {
        List<string[]> rows = CSVParser.Parse(csvText);

        int counter         = 0;
        int overridedCounter = 0;

        foreach (string[] values in rows)
        {
            counter++;
            if (counter == 1) continue; // skip header row

            // CSV columns: ID, InternalName, SymptomType, DisplayName, Description, Priority, DevNotes
            NoteSO newSO = ScriptableObject.CreateInstance<NoteSO>();

            newSO.patientName = values[0];
            newSO.noteName = values[1];
            newSO.noteText = values[2];
            newSO.symptomsCaught = int.Parse(values[3]);
            
            
            //newSO.devNotes = string.IsNullOrEmpty(values[6]) ? "No description provided" : values[6];

            var nameAndPath = noteFolderPath + "/" + newSO.noteName + ".asset";
            Debug.Log(newSO.noteName);
            if (AssetDatabase.AssetPathExists(nameAndPath))
                overridedCounter++;

            AssetDatabase.CreateAsset(newSO, nameAndPath);
        }
    
        Debug.Log("Created " + counter + " Note");
        Debug.LogWarning("Overrided " + overridedCounter + " Note");
        AssetDatabase.SaveAssets();
    }
    
    
    // =====================================================================
    //  DIALOGUE
    // =====================================================================

    public void GenerateDialogueSO()
    {
        //EditorCoroutineUtility.StartCoroutineOwnerless(FetchAndGenerateDialogue());
    }

    private IEnumerator FetchAndGenerateDialogue()
    {
        yield return null;
        // Debug.Log("Fetching Dialogue sheet from Google Sheets...");
        //
        // using (UnityWebRequest request = UnityWebRequest.Get(dialogueURL))
        // {
        //     yield return request.SendWebRequest();
        //
        //     if (request.result != UnityWebRequest.Result.Success)
        //     {
        //         Debug.LogError("Failed to fetch Dialogue sheet: " + request.error);
        //         yield break;
        //     }
        //
        //     ProcessDialogueCSV(request.downloadHandler.text);
        // }
    }

    private void ProcessDialogueCSV(string csvText)
    {
        // List<string[]> rows = CSVParser.Parse(csvText);
        //
        // int counter = 0;
        //
        // foreach (string[] values in rows)
        // {
        //     counter++;
        //     if (counter == 1) continue; // skip header row
        //
        //     // CSV columns: ID, InternalName, Origin, DisplayName, Dialogue1, Dialogue2, Dialogue3, NextSO, SymptomSO
        //     DialogueSO newSO = ScriptableObject.CreateInstance<DialogueSO>();
        //
        //     newSO.id           = int.Parse(values[0]);
        //     newSO.internalName = values[1];
        //
        //     var originString = values[2];
        //     if (originString.Contains("Chart"))          newSO.origin |= DialogueSO.Origin.Chart;
        //     if (originString.Contains("Audio"))          newSO.origin |= DialogueSO.Origin.Audio;
        //     if (originString.Contains("Visual"))         newSO.origin |= DialogueSO.Origin.Visual;
        //     if (originString.Contains("Medicine"))       newSO.origin |= DialogueSO.Origin.Medicine;
        //     if (originString.Contains("Red Herring"))    newSO.origin |= DialogueSO.Origin.RedHerring;
        //     if (originString.Contains("Pre-Existing Q")) newSO.origin |= DialogueSO.Origin.PreExist;
        //
        //     newSO.displayName   = values[3];
        //     newSO.dialogueOne   = values[4];
        //     newSO.dialogueTwo   = values[5];
        //     newSO.dialogueThree = values[6];
        //     newSO.nextSO        = values[7];
        //     newSO.symptomSO     = values[8];
        //
        //     var nameAndPath = dialogueFolderPath + "/" + newSO.internalName + ".asset";
        //     if (AssetDatabase.AssetPathExists(nameAndPath))
        //         Debug.LogWarning("Overrided " + newSO.internalName);
        //
        //     AssetDatabase.CreateAsset(newSO, nameAndPath);
        // }
        //
        // AssetDatabase.SaveAssets();
        // Debug.Log("Created " + counter + " Dialogue");
    }
}


// =====================================================================
//  CSV PARSER
// =====================================================================
public static class CSVParser
{
    public static List<string[]> Parse(string csvText)
    {
        var rows  = new List<string[]>();
        var lines = csvText.Split('\n');

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            rows.Add(ParseLine(line));
        }

        return rows;
    }

    public static string[] ParseLine(string line)
    {
        var fields    = new List<string>();
        var current   = new System.Text.StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(current.ToString().Trim());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        fields.Add(current.ToString().Trim());
        return fields.ToArray();
    }
}