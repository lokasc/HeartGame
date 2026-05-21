using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using Febucci.TextAnimatorForUnity;
using Febucci.TextAnimatorCore.Data;
using Febucci.TextAnimatorCore.Typing;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChoiceManager : MonoBehaviour
{
    public event Action<Answer> OnAnswerSelected;
    public static ChoiceManager Instance { get; private set; }

    [SerializeField] private TypewriterComponent patientTypewriter;

    [SerializeField] private List<ChoiceSO> choiceList;
    [SerializeField] private ChoiceSO[] allChoices; // Auto-populated by weird editor script magic
   
    [SerializeField] private GameObject choicePanel;

    [SerializeField] private TMP_Text patientText;
    
    private float currentPatientSpeakingSpeed;

    [SerializeField] private GameObject optionAPanel;
    [SerializeField] private TMP_Text optionAText;
    [SerializeField] private Button optionAButton;
  
    [SerializeField] private GameObject optionBPanel;

    [SerializeField] private TMP_Text optionBText;
    [SerializeField] private Button optionBButton;

    [SerializeField] private GameObject notesOptionPanel;

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
        patientTypewriter.onMessage.AddListener(OnTypewriterMessage);
        // patientTypewriter.onTextStart.AddListener();
        patientTypewriter.onTypewriterStart.AddListener(PlayVoiceSound);
        patientTypewriter.onTextShowed.AddListener(StopVoiceSound);
        // The correct way to do patient sounds is to do an subscribe to OnCharacterVisible

    }

    private void PlayVoiceSound()
    {
        SoundManager.Instance.PlayClip(SoundManager.Instance.VoiceAudioClip);
    }

    private void StopVoiceSound()
    {
        SoundManager.Instance.StopAudioClip();
    }

    
    private void OnDisable()
    {
        Patient.OnPatientTraitsLoaded -= HandlePatientTraitsLoaded;
        patientTypewriter.onMessage.RemoveListener(OnTypewriterMessage);
        patientTypewriter.onTypewriterStart.RemoveListener(PlayVoiceSound);
        patientTypewriter.onTextShowed.RemoveListener(StopVoiceSound);
    }

    private void OnTypewriterMessage(EventMarker marker)
    {
        
        Debug.Log("Event: " + marker.name);

        switch (marker.name)
        {
            case "ShowAllOptions":
                ShowAllOptions(currentChoice);
                break;
            case "ShowOptionA":
                ShowOptionA(currentChoice);
                break;
            case "ShowOptionB":
                ShowOptionB(currentChoice);
                break;
            case "ShowNotesOption":
                ShowNotesOption(currentChoice);
                break;
        }
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

    public void SetCurrentPatientSpeakingSpeed(float speed)
    {
        currentPatientSpeakingSpeed = speed;
        Debug.Log($"Set current patient speaking speed to {currentPatientSpeakingSpeed}");
    }

    public void ShowChoice(string choiceName)
    {
        currentChoice = choiceList.FirstOrDefault(c => c.choiceName == choiceName);
        if (currentChoice != null)
        {
            optionAText.enabled = false;
            optionBText.enabled = false;
            notesOptionText.enabled = false;
            optionAButton.enabled = false;
            optionBButton.enabled = false;
            notesOptionButton.enabled = false;

            BarkManager.Instance.IsSpawning = false;
            choicePanel.SetActive(true);

            patientTypewriter.SetTypewriterSpeed(currentPatientSpeakingSpeed);
            // patientTypewriter.ShowText("Hello!<?ShowOptionA> I am wondering if you think <?ShowOptionB> I should for a backflip<?ShowNotesOption> now or later. <?ShowAllOptions>");
            // Here I play the sound
            patientTypewriter.ShowText($"{currentChoice.patientDialogue}<?ShowAllOptions>");
            StartCoroutine(RebuildChoiceLayoutNextFrame());
        }
    }

    private IEnumerator RebuildChoiceLayoutNextFrame()
    {
        yield return null;

        patientText.ForceMeshUpdate();
        Canvas.ForceUpdateCanvases();

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            choicePanel.GetComponent<RectTransform>()
        );
    }

    private void ShowOptionA(ChoiceSO choice)
    {
            if(optionAText == null || string.IsNullOrEmpty(currentChoice.optionAText)){
                optionAPanel.SetActive(false);
            }
            else
            {
                optionAPanel.SetActive(true);
                optionAText.enabled = true;
                optionAButton.enabled = true;
            }

            optionAText.text = currentChoice.optionAText;
            optionANextChoice = choiceList.FirstOrDefault(c => c.choiceName == currentChoice.optionANext);

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                choicePanel.GetComponent<RectTransform>()
            );
    }

    private void ShowOptionB(ChoiceSO choice)
    {
            if(optionBText == null || string.IsNullOrEmpty(currentChoice.optionBText)){
                optionBPanel.SetActive(false);
            }
            else
            {
                optionBPanel.SetActive(true);
                optionBText.enabled = true;
                optionBButton.enabled = true;
            }

            optionBText.text = currentChoice.optionBText;
            optionBNextChoice = choiceList.FirstOrDefault(c => c.choiceName == currentChoice.optionBNext);

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                choicePanel.GetComponent<RectTransform>()
            );
    }

    private void ShowNotesOption(ChoiceSO choice)
    {
            notesOptionPanel.SetActive(true);
            notesOptionText.enabled = true;
            notesOptionButton.enabled = true;
            notesOptionText.text = currentChoice.notesText;

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                choicePanel.GetComponent<RectTransform>()
            );
    }

    private void ShowAllOptions(ChoiceSO choice)
    {
            if(optionAText == null || string.IsNullOrEmpty(currentChoice.optionAText)){
                optionAPanel.SetActive(false);
            }
            else
            {
                optionAPanel.SetActive(true);
                optionAText.enabled = true;
                optionAButton.enabled = true;
            }

            optionAText.text = currentChoice.optionAText;
            optionANextChoice = choiceList.FirstOrDefault(c => c.choiceName == currentChoice.optionANext);

            if(optionBText == null || string.IsNullOrEmpty(currentChoice.optionBText)){
                optionBPanel.SetActive(false);
            }
            else
            {
                optionBPanel.SetActive(true);
                optionBText.enabled = true;
                optionBButton.enabled = true;
            }

            optionBText.text = currentChoice.optionBText;
            optionBNextChoice = choiceList.FirstOrDefault(c => c.choiceName == currentChoice.optionBNext);

            notesOptionPanel.SetActive(true);
            notesOptionText.enabled = true;
            notesOptionButton.enabled = true;
            notesOptionText.text = currentChoice.notesText;

            Canvas.ForceUpdateCanvases();

            LayoutRebuilder.ForceRebuildLayoutImmediate(
                choicePanel.GetComponent<RectTransform>()
            );
    }

    public void HideChoice()
    {

        choicePanel.SetActive(false);
        BarkManager.Instance.IsSpawning = true;
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

    public class Answer
    {
        public string Question;
        public string AnswerName;
        public int EmpathyGained;
    }
    
    
    public void SelectOptionA(){
        OnAnswerSelected?.Invoke(new Answer {  AnswerName = currentChoice.optionAText, 
            EmpathyGained = currentChoice.optionAEmpthayPoint,
            Question = currentChoice.patientDialogue   
        });
        
        
        ShowNextChoice(optionANextChoice);
    }

    public void SelectOptionB(){
        OnAnswerSelected?.Invoke(new Answer {  AnswerName = currentChoice.optionBText, 
            EmpathyGained = currentChoice.optionBEmpthayPoint,
            Question = currentChoice.patientDialogue   
        });
       
        ShowNextChoice(optionBNextChoice);
    }

    public void EndConversationEarly(){
        HideChoice();
    }

    public void TakeNote(){
        NotesManager.Instance.AddNote(currentChoice.notesName);
        HideChoice();
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
