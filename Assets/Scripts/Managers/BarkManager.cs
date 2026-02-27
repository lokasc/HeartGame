using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BarkManager : MonoBehaviour
{
    public static BarkManager Instance { get; private set; }

    [SerializeField] List<BarkSO> barkList;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private GameObject barkPrefab;
    [SerializeField] private RectTransform[] spawnPoints;
    [SerializeField] private RectTransform barkSpawnpoint;

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
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (barkList.Count > 0)
        {
            BarkSO bark = barkList[Random.Range(0, barkList.Count)];
            RectTransform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject barkInstance = Instantiate(barkPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
            barkInstance.GetComponent<TextBark>().SetBarkSO(bark);
            TMP_Text barkTextMesh = barkInstance.GetComponentInChildren<TMP_Text>();
            if (barkTextMesh != null)
            {
                barkTextMesh.text = bark.barkText;
            }

            barkList.Remove(bark);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
