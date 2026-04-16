using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager Instance { get; private set; }

    [SerializeField] private List<ChoiceSO> choiceList;
    [SerializeField] private ChoiceSO[] allChoices; // Auto-populated by weird editor script magic
   
    [SerializeField] private GameObject choicePanel;

    [SerializeField] private TMP_Text patientText;
    
    [SerializeField] private GameObject optionAPanel;
    [SerializeField] private TMP_Text optionAText;
    [SerializeField] private Button optionAButton;
  
    [SerializeField] private GameObject optionBPanel;

    [SerializeField] private TMP_Text optionBText;
    [SerializeField] private Button optionBButton;

    [SerializeField] private TMP_Text notesOptionText;
    [SerializeField] private Button notesOptionButton;

    private ChoiceSO currentChoice;
    private ChoiceSO optionANextChoice;
    private ChoiceSO optionBNextChoice;

    //Add notes choice NoteSO;


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
        HideChoice();
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
        choiceList.Clear();
        
        if (allChoices != null)
        {
            foreach (ChoiceSO choice in allChoices)
            {
                if (choice.patientName == patientName)
                {
                    choiceList.Add(choice);
                }
            }
        }
        
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

    public void ShowChoice(string choiceName)
    {
        currentChoice = choiceList.FirstOrDefault(c => c.choiceName == choiceName);
        if (currentChoice != null)
        {
            BarkManager.Instance.IsSpawning = false;
            choicePanel.SetActive(true);

            patientText.text = currentChoice.patientDialogue;


            if(optionAText == null || string.IsNullOrEmpty(currentChoice.optionAText)){
                optionAPanel.SetActive(false);
            }
            else
            {
                optionAPanel.SetActive(true);
            }

            optionAText.text = currentChoice.optionAText;
            optionANextChoice = choiceList.FirstOrDefault(c => c.choiceName == currentChoice.optionANext);

            if(optionBText == null || string.IsNullOrEmpty(currentChoice.optionBText)){
                optionBPanel.SetActive(false);
            }
            else
            {
                optionBPanel.SetActive(true);
            }

            optionBText.text = currentChoice.optionBText;
            optionBNextChoice = choiceList.FirstOrDefault(c => c.choiceName == currentChoice.optionBNext);

            notesOptionText.text = currentChoice.notesText;
        }

    }

    public void HideChoice()
    {
        choicePanel.SetActive(false);
    }

    public void ShowNextChoice(ChoiceSO nextChoice)
    {
        if (nextChoice != null)
        {
            ShowChoice(nextChoice.choiceName);
        }
        else
        {
            HideChoice();
        }
    }

    public void SelectOptionA(){
        GameManager.instance.empathyPoints += currentChoice.optionAEmpthayPoint;
        Debug.Log("Empathy Points Gained: " + currentChoice.optionAEmpthayPoint.ToString());
        ShowNextChoice(optionANextChoice);
        BarkManager.Instance.IsSpawning = true;
    }

    public void SelectOptionB(){
        GameManager.instance.empathyPoints += currentChoice.optionBEmpthayPoint;
        Debug.Log("Empathy Points Gained: " + currentChoice.optionBEmpthayPoint.ToString());
        ShowNextChoice(optionBNextChoice);
        BarkManager.Instance.IsSpawning = true;
    }

    public void EndConversationEarly(){
        HideChoice();
        BarkManager.Instance.IsSpawning = true;
    }

    public void TakeNote(){
        NotesManager.Instance.AddNote(currentChoice.notesName);
        HideChoice();
        BarkManager.Instance.IsSpawning = true;

    }

#if UNITY_EDITOR
    public void RefreshChoiceReferences()
    {
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ChoiceSO");
        allChoices = new ChoiceSO[guids.Length];
        
        for (int i = 0; i < guids.Length; i++)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            allChoices[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<ChoiceSO>(path);
        }
        
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEngine.Debug.Log($"Auto-populated {allChoices.Length} choice references in ChoiceManager");
    }
#endif

}
