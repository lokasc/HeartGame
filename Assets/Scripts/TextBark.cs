using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextBark : MonoBehaviour
{
    private BarkSO barkSO;

    private float moveDuration = 5f;
    private float fadeDuration = 1f;
    private float waitTime = 2f;
    private float moveDistance = 50f;

    void Start()
    {
        FadeInAll();
    }

    public void FadeInAll()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(transform.position.y + moveDistance, moveDuration).SetEase(Ease.OutQuad));
        sequence.Join(canvasGroup.DOFade(1, fadeDuration));
        sequence.AppendInterval(waitTime);
        sequence.Append(canvasGroup.DOFade(0, fadeDuration));
        sequence.OnComplete(() => Destroy(gameObject));
    }

    public void ShowQuestion()
    {
        Debug.Log($"Showing question for bark: {barkSO.barkText}");
        QuestionManager.Instance.ShowQuestion(barkSO);
        Destroy(gameObject);
    }   

    public void SetBarkSO(BarkSO newBarkSO)
    {
        barkSO = newBarkSO;
    }
}
