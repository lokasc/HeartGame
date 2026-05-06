using TMPro;
using UnityEngine;

public class FeedbackBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI empathyGain;
    [SerializeField] private TextMeshProUGUI currentEmpathy;
    [SerializeField] private TextMeshProUGUI feedbackText;

    public void SetText(float empathyGained, float playerEmpathy)
    {
        empathyGain.text = empathyGained.ToString("0");
        if (Mathf.Sign(empathyGained) > 0)
        {
            empathyGain.text += " Empathy Gained";
        }
        else
        {
            empathyGain.text += " Empathy Lost";
        }
        
        currentEmpathy.text = "Current Empathy " + playerEmpathy.ToString("0");
    }
}
