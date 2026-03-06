using System.IO;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using System.Collections.Generic;


public class SymptomsGenerator : MonoBehaviour
{
    [InfoBox("Ensure that the csv file is placed inside of Asset/StreamingAssets Folder, csv file has the same name as the variable csvName, and the folder path is in a desired location.")]
    // Make sure that this file is stored in the Assets/StreamingAssets folder and is case sensitive.
    public string csvName = "SymptomSchema.csv";
    // This is where our scriptable objects are stored.
    public string folderPath = "Assets/Scripts/Symptoms";
    
    
    public string dialogueCSVname = ".csv";
    public string dialogueFolderPath = "Assets/Scripts/Dialogue";
    
    [Button]
    // This creates scriptable objects in the desired folder
    public void Generate()
    {
        // Parse through the CSV file and ge through each row
        string path = Path.Combine(Application.streamingAssetsPath, dialogueCSVname);
        string[] lines = File.ReadAllLines(path);

        int counter = 0;
        int overridedCounter = 1;
        // Go through each row and split them into sizeable chunks.
        foreach (string line in lines)
        {
            // Skip first line.
            counter++;
            if (counter == 1)
            {
                continue;
            }
            
            string[] values = line.Split(',');
            
            // CSV stored like this
            // ID, InternalName, Origin, Display, Dialogue1, Dialogue2, Dialogue3
            
            // Create and assign scriptable object.
            SymptomSO newSO = ScriptableObject.CreateInstance<SymptomSO>();
            newSO.id = int.Parse(values[0]);
            newSO.internalName = values[1];

            if (values[2] == "Audio")
            {
                newSO.symptomType = SymptomType.Audio;
            }
            else if (values[2] == "Visual")
            {
                newSO.symptomType = SymptomType.Visual;
            }
            else if (values[2] == "Paper")
            {
                newSO.symptomType = SymptomType.Paper;
            }
            else if (values[2] == "Dialogue")
            {
                newSO.symptomType = SymptomType.Dialogue;
            }
            else
            {
                Debug.LogError("Could not create a symptom type for: " + newSO.internalName);
            }
            
            newSO.displayName = values[3];
            newSO.description = values[4];

            if (values[5] == "High")
            {
                newSO.priority = Priority.High;
            }
            else if (values[5] == "Low")
            {
                newSO.priority = Priority.Low;
            }
            else if (values[5] == "Medium")
            {
                newSO.priority = Priority.Medium;
            }
            else if (values[5] == "Critical")
            {
                newSO.priority = Priority.Critical;
            }
            else
            {
                Debug.LogError("Could not create symptom priority for: " + newSO.internalName);
            }

            if (values[6] == "")
            {
                newSO.devNotes = "No description provided";
            }
            
            // Set path and create asset.
            var nameAndPath = folderPath + "/" + newSO.internalName + ".asset";
        
            if (AssetDatabase.AssetPathExists(nameAndPath))
            {
                overridedCounter++;
                //Debug.LogWarning("Overrided " + newSO.internalName);
            }
            AssetDatabase.CreateAsset(newSO, nameAndPath);
        }

        Debug.Log("Created " + counter + " Symptoms");
        Debug.LogWarning("Overrided " + overridedCounter + " Symptoms");
        AssetDatabase.SaveAssets();
    }

    [Button]
    public void GenerateDialogueSO()
    {
        // Parse through the CSV file and ge through each row
        string path = Path.Combine(Application.streamingAssetsPath, dialogueCSVname);
        string[] lines = File.ReadAllLines(path);


        var counter = 0;
        
        // Things that have multiple things will have quotes.
        foreach (var line in lines)
        {
            counter++;
            if (counter == 1) continue;
            DialogueSO newSO = ScriptableObject.CreateInstance<DialogueSO>();
            string[] values = CSVParser.ParseLine(line);
            
            newSO.id = int.Parse(values[0]);
            newSO.internalName = values[1];

            var originString = values[2];
            if (originString.Contains("Chart"))      newSO.origin |= DialogueSO.Origin.Chart;
            if (originString.Contains("Audio"))      newSO.origin |= DialogueSO.Origin.Audio;
            if (originString.Contains("Visual"))     newSO.origin |= DialogueSO.Origin.Visual;
            if (originString.Contains("Medicine"))   newSO.origin |= DialogueSO.Origin.Medicine;
            if (originString.Contains("Red Herring")) newSO.origin |= DialogueSO.Origin.RedHerring;
            if (originString.Contains("Pre-Existing Q")) newSO.origin |= DialogueSO.Origin.PreExist;
            
            
            newSO.displayName = values[3];
            newSO.dialogueOne = values[4];
            newSO.dialogueTwo = values[5];
            newSO.dialogueThree = values[6];
            newSO.nextSO = values[7];
            newSO.symptomSO = values[8];
            
            // Set path and create asset.
            var nameAndPath = dialogueFolderPath + "/" + newSO.internalName + ".asset";
            // Debug.Log("Created " + newSO.internalName);
            if (AssetDatabase.AssetPathExists(nameAndPath))
            {
                Debug.LogWarning("Overrided " + newSO.internalName);
            }
            AssetDatabase.CreateAsset(newSO, nameAndPath);
        }
        AssetDatabase.SaveAssets();
        Debug.Log("Created " + counter + " Dialogue");
    }

}

public static class CSVParser
{
    public static List<string[]> Parse(string csvText)
    {
        var rows = new List<string[]>();
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
        var fields = new List<string>();
        var current = new System.Text.StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes; // toggle quote mode
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

        fields.Add(current.ToString().Trim()); // add last field
        return fields.ToArray();
    }
}
