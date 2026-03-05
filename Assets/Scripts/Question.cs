using UnityEngine;

public class Question : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowQuestion()
    {
        DialogueManager.Instance.ShowDialogue(dialogue);
        Destroy(gameObject);
    }   

    public void ShowDialogue()
    {
        DialogueManager.Instance.ShowDialogue(dialogue);
        Destroy(gameObject);
    }
}
