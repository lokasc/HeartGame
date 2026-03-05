using System.IO;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class SymptomsGenerator : MonoBehaviour
{
    [InfoBox("Ensure that the csv file is placed inside of Asset/StreamingAssets Folder, csv file has the same name as the variable csvName, and the folder path is in a desired location.")]
    // Make sure that this file is stored in the Assets/StreamingAssets folder and is case sensitive.
    public string csvName = "SymptomSchema.csv";
    // This is where our scriptable objects are stored.
    public string folderPath = "Assets/Scripts/Symptoms";
    
    [Button]
    // This creates scriptable objects in the desired folder
    public void Generate()
    {
        // Parse through the CSV file and ge through each row
        string path = Path.Combine(Application.streamingAssetsPath, csvName);
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
            // ID, Internal Name, Category, DisplayName, Description, PriorityTier, Dev Description
            
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
}
