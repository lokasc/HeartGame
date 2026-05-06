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

    public float efficiencyPoints;
    public static GameManager instance;
    
    [Header("Efficiency Points")]
    public float startEfficiencyPoints;
    public float efficiencyPointsSubtractedPerSecond;

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
            rank = "S";
        else if (empathyPoints == 2)
            rank = "A";
        else if (empathyPoints >= 0)
            rank = "B";
        else if (empathyPoints == -1 || empathyPoints == -2)
            rank = "C";
        else
            rank = "F";

        if (efficiencyPoints >= startEfficiencyPoints)
            efficiencyStars = 5;
        else if (efficiencyPoints >= 150f)
            efficiencyStars = 4;
        else if (efficiencyPoints >= 100f)
            efficiencyStars = 3;
        else if (efficiencyPoints >= 50f)
            efficiencyStars = 2;
        else if (efficiencyPoints >= 0f)
            efficiencyStars = 1;

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

    public void OnOneSecondTimerEnd()
    {
        efficiencyPoints -= efficiencyPointsSubtractedPerSecond;
        oneSecondTimer.StartTimer(1);
    }

    public void OnChoiceSelected(ChoiceManager.Answer playerAnswer)
    {
        empathyPoints += playerAnswer.EmpathyGained;
        EndScreenLineGraph.instance.AddPoint(new EndScreenLineGraph.DataPoint()
        {
            NewEmpathyValue = empathyPoints, EmpathyChange = playerAnswer.EmpathyGained 
        });
    }
}