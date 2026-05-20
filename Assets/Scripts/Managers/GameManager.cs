using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.PlayerLoop;

// The game manager calculates the scores and etc.
public class GameManager : MonoBehaviour
{
    // Backing field — use EmpathyPoints (capital E) everywhere instead of empathyPoints
    [SerializeField] public int empathyPoints;

    private int empathyScore;
    private int symptomScore;
    private int efficiencyScore;

    public float efficiencyPoints;
    public static GameManager instance;
    
    [Header("Efficiency Points")]
    public float startEfficiencyPoints;
    public float efficiencyPointsSubtractedPerSecond;

    [Header("Symptom Scoring")]
    [SerializeField] private int totalSymptomShares;
    [SerializeField] private int symptomSharesCaught;
    public float SymptomPercentageCaught;

    [HideInInspector] public int efficiencyStars;
    [HideInInspector] public string rank;

    [Foldout("References")][SerializeField] private GameObject diagnosisPanel;
    [Foldout("References")] public Transform scoreUI;
    [Foldout("References")][SerializeField] private Timer countDownTimer;
    [Foldout("References")][SerializeField] private Timer oneSecondTimer;
    
   

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        scoreUI.gameObject.SetActive(false);
        countDownTimer.StartTimer(startEfficiencyPoints);
        countDownTimer.OnTimerFinished.AddListener(OnPerfectTimeEnd);
        efficiencyPoints = startEfficiencyPoints;
        EndScreenLineGraph.instance.AddPoint(new EndScreenLineGraph.DataPoint() { NewEmpathyValue = empathyPoints,EmpathyChange = 0});
        ChoiceManager.Instance.OnAnswerSelected += OnChoiceSelected;
    }

    [Button]
    public void OnGameEnd()
    {
        if (empathyPoints >= 3)
        {
            empathyScore = 5;
        }
        else if (empathyPoints == 2)
        {
            empathyScore = 4;
        }
        else if (empathyPoints >= 0)
        {
            empathyScore = 3;
        }
        else if (empathyPoints == -1 || empathyPoints == -2)
        {
            empathyScore = 2;
        }
        else
        {
            empathyScore = 1;
        }

        if (efficiencyPoints >= startEfficiencyPoints)
        {
            efficiencyStars = 5;
            efficiencyScore = 5;
        }
        else if (efficiencyPoints >= 150f)
        {
            efficiencyStars = 4;
            efficiencyScore = 4;
        }
        else if (efficiencyPoints >= 100f)
        {
            efficiencyStars = 3;
            efficiencyScore = 3;
        }
        else if (efficiencyPoints >= 50f)
        {
            efficiencyStars = 2;
            efficiencyScore = 2;
        }
        else if (efficiencyPoints >= 0f)
        {
            efficiencyStars = 1;
            efficiencyScore = 1;
        }

        
        CalculateSymptomScore();

        CalculateRank();

        scoreUI.GetComponent<EndScreenUI>().UpdateUI();
        scoreUI.gameObject.SetActive(true);
    }

    public void ShowDiagnosisPanel()
    {
        if (diagnosisPanel != null)
            diagnosisPanel.SetActive(true);
    }

    public void OnPerfectTimeEnd()
    {
        oneSecondTimer.OnTimerFinished.AddListener(OnOneSecondTimerEnd);
        oneSecondTimer.StartTimer(1);
    }

    private void CalculateRank()
    {
        float averageScore = (empathyScore + symptomScore + efficiencyScore) / 3f;

        if (averageScore >= 4.5f)
            rank = "S";
        else if (averageScore >= 3.5f)
            rank = "A";
        else if (averageScore >= 2.5f)
            rank = "B";
        else if (averageScore >= 1.5f)
            rank = "C";
        else
            rank = "D";
    }

    private void CalculateSymptomScore()
    {
        totalSymptomShares = NotesManager.Instance.TotalSymptomShares;
        symptomSharesCaught = NotesManager.Instance.SymptomSharesCaught;

        if (totalSymptomShares > 0)
            SymptomPercentageCaught = (float)symptomSharesCaught / totalSymptomShares * 100f;
        else
            SymptomPercentageCaught = 0f;

        symptomScore = Mathf.Clamp(Mathf.CeilToInt(SymptomPercentageCaught / 20f), 1, 5);
    }

    public void OnOneSecondTimerEnd()
    {
        efficiencyPoints -= efficiencyPointsSubtractedPerSecond;
        oneSecondTimer.StartTimer(1);
    }

    public void OnChoiceSelected(ChoiceManager.Answer playerAnswer)
    {
        Debug.Log($"Empathy gained: {playerAnswer.EmpathyGained}. Total empathy before selection: {empathyPoints}.");
        empathyPoints += playerAnswer.EmpathyGained;
        EndScreenLineGraph.instance.AddPoint(new EndScreenLineGraph.DataPoint()
        {
            NewEmpathyValue = empathyPoints, EmpathyChange = playerAnswer.EmpathyGained 
        });
    }
}