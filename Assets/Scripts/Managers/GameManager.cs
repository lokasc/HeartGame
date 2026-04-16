using System;
using UnityEngine;
using NaughtyAttributes;

// The game manager calculates the scores and etc.
public class GameManager : MonoBehaviour
{
    public Transform scoreUI; // This is a panel that hides everything and tells you your score.
    public int empathyPoints;
    public static GameManager instance;

    [HideInInspector] public string rank;

    [SerializeField] private GameObject diagnosisPanel;
    
    
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
    }

    // Update is called once per frame
    void Update()
    {
        
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

}


