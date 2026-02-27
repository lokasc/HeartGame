using UnityEngine;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    [SerializeField] private GameObject questionPanel;

    [SerializeField] private BarkSO testSO;

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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowQuestion(BarkSO barkSO)
    {
        // Instantiate the question prefab at the desired position and rotation
        GameObject question = Instantiate(barkSO.questionPrefab, Vector3.zero, Quaternion.identity, questionPanel.transform);

        // Get the Text component from the instantiated prefab and set the question text
        TMP_Text questionTextMesh = question.GetComponentInChildren<TMP_Text>();
        if (questionTextMesh != null)
        {
            questionTextMesh.text = barkSO.questionText;
        }
    }
}
