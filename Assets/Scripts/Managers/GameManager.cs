using System;
using UnityEngine;
using NaughtyAttributes;

// The game manager calculates the scores and etc.
public class GameManager : MonoBehaviour
{
    public int empathyPoints;
    public float efficiencyPoints;
    public static GameManager instance;
    
    [Header("Efficiency Points")]
    public float startEfficiencyPoints;
    public float efficiencyPointsSubtractedPerSecond;

    [HideInInspector] public int efficiencyStars;
    
    [HideInInspector] public string rank;

    [Foldout("References")][SerializeField] private GameObject diagnosisPanel;
    [Foldout("References")]public Transform scoreUI; // This is a panel that hides everything and tells you your score.
    
    [Foldout("References")][SerializeField]private Timer countDownTimer;
    [Foldout("References")][SerializeField] private Timer oneSecondTimer; 
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        scoreUI.gameObject.SetActive(false);
        countDownTimer.StartTimer(startEfficiencyPoints);
        countDownTimer.OnTimerFinished.AddListener(OnPerfectTimeEnd);
        efficiencyPoints = startEfficiencyPoints;
    }

    [Button]
    // hooked up to a button.
    public void OnGameEnd()
    {
        // Calculate Rank based on current Empathy Points

        if (empathyPoints >= 3)
        {
            rank = "S";
        }
        else if (empathyPoints == 2)
        {
            rank = "A";
        }
        else if (empathyPoints >= 0)
        {
            rank = "B";
        }
        else if (empathyPoints == -1 || empathyPoints == -2)
        {
            rank = "C";
        }
        else // -3 or below
        {
            rank = "F";
        }

        if (efficiencyPoints >= startEfficiencyPoints)
        {
            efficiencyStars = 5;
        }
        else if (efficiencyPoints >= 150f)
        {
            efficiencyStars = 4;
        }
        else if (efficiencyPoints >= 100f)
        {
            efficiencyStars = 3;
        }
        else if (efficiencyPoints >= 50f)
        {
            efficiencyStars = 2;
        }
        else if (efficiencyPoints >= 0f)
        {
            efficiencyStars = 1;
        }
        
        scoreUI.GetComponent<EndScreenUI>().UpdateUI();
        scoreUI.gameObject.SetActive(true);
    }

    public void ShowDiagnosisPanel()
    {
        if(diagnosisPanel != null)
        {
            diagnosisPanel.SetActive(true);
        }
    }
    
    
    // tha max possible time that the player can get a perf.
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
    
    
    
    
    
}


