using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NaughtyAttributes;



public class GoogleSheetsReader : MonoBehaviour
{
    // Paste your Spreadsheet ID here
    [SerializeField] private string spreadsheetId = "YOUR_SPREADSHEET_ID";

    // The name of the tab/sheet you want to read
    [SerializeField] private string sheetName = "Sheet1";

    [Button]
    public void Generate()
    {
        StartCoroutine(FetchSheet());
    }
   
    
    IEnumerator FetchSheet()
    {
        // Build the export URL — exports the named tab as CSV
        string url = $"https://docs.google.com/spreadsheets/d/{spreadsheetId}/gviz/tq?tqx=out:csv&sheet={sheetName}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch sheet: " + request.error);
                yield break;
            }

            string csvData = request.downloadHandler.text;
            List<string[]> parsedData = ParseCSV(csvData);

            // Print each row to the console
            foreach (string[] row in parsedData)
            {
                Debug.Log(string.Join(" | ", row));
            }

            // Example: access row 1, column 0 (skip row 0 which is the header)
            if (parsedData.Count > 1)
            {
                Debug.Log("First data value: " + parsedData[1][0]);
            }
        }
    }

    // Simple CSV parser — handles quoted fields with commas inside them
    List<string[]> ParseCSV(string csvText)
    {
        List<string[]> rows = new List<string[]>();
        string[] lines = csvText.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            rows.Add(SplitCSVLine(line));
        }

        return rows;
    }

    string[] SplitCSVLine(string line)
    {
        List<string> fields = new List<string>();
        bool inQuotes = false;
        System.Text.StringBuilder current = new System.Text.StringBuilder();

        foreach (char c in line)
        {
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
