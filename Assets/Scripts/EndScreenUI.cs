using TMPro;
using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    public TextMeshProUGUI rank;
    public TextMeshProUGUI empathyPoints;
    public void UpdateUI()
    {
        rank.text = GameManager.instance.rank;
        empathyPoints.text = "Empathy: " + GameManager.instance.empathyPoints.ToString();
    }
}
