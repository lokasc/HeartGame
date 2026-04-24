using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class EndScreenUI : MonoBehaviour
{
    public TextMeshProUGUI rank;
    public TextMeshProUGUI empathyPoints;

    public Transform starHBox;
    public GameObject starPrefab;

    [Header("Star Animation")]
    [SerializeField] private float starPopDuration = 0.35f;
    [SerializeField] private float starPopDelay = 0.15f;
    [SerializeField] private Ease starPopEase = Ease.OutBack;

    [Header("Star Wiggle")]
    [SerializeField] private float wiggleAngle = 20f;
    [SerializeField] private float wiggleDuration = 0.12f;
    [SerializeField] private float wigglePause = 0.6f;

    private readonly List<GameObject> _spawnedStars = new();

    public void UpdateUI()
    {
        rank.text = GameManager.instance.rank;
        empathyPoints.text = "Empathy: " + GameManager.instance.empathyPoints.ToString();
    }

    private void OnEnable()
    {
        SpawnStars(GameManager.instance.efficiencyStars);
    }

    /// <summary>
    /// Clears existing stars, spawns <paramref name="count"/> new ones,
    /// and animates each one popping up then wiggling.
    /// </summary>
    public void SpawnStars(int count)
    {
        ClearStars();

        for (int i = 0; i < count; i++)
        {
            GameObject instance = Instantiate(starPrefab, starHBox);

            // Target the Star child so the nested Canvas root is not disturbed.
            Transform starChild = instance.transform.Find("Star") ?? instance.transform;
            starChild.localScale = Vector3.zero;
            starChild.localRotation = Quaternion.identity;
            _spawnedStars.Add(instance);

            BuildStarSequence(starChild, i * starPopDelay);
        }
    }

    /// <summary>
    /// Builds a pop-then-wiggle sequence for a single star child transform,
    /// inserted at <paramref name="startTime"/> seconds into the overall stagger.
    /// The wiggle loops indefinitely after the pop completes.
    /// </summary>
    private void BuildStarSequence(Transform starChild, float startTime)
    {
        Sequence sequence = DOTween.Sequence();

        // Pop in.
        sequence.Insert(startTime,
            starChild.DOScale(Vector3.one, starPopDuration).SetEase(starPopEase));

        // After the pop, start a looping wiggle with a pause between each cycle.
        sequence.InsertCallback(startTime + starPopDuration, () =>
        {
            Sequence wiggle = DOTween.Sequence();
            wiggle.Append(
                starChild.DORotate(new Vector3(0f, 0f, wiggleAngle), wiggleDuration)
                         .SetEase(Ease.InOutSine));
            wiggle.Append(
                starChild.DORotate(new Vector3(0f, 0f, -wiggleAngle), wiggleDuration)
                         .SetEase(Ease.InOutSine));
            wiggle.Append(
                starChild.DORotate(Vector3.zero, wiggleDuration)
                         .SetEase(Ease.OutSine));
            wiggle.AppendInterval(wigglePause);
            wiggle.SetLoops(-1);
        });
    }

    private void ClearStars()
    {
        foreach (GameObject star in _spawnedStars)
        {
            if (star != null)
                Destroy(star);
        }
        _spawnedStars.Clear();
    }
}
