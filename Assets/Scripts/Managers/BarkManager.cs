using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BarkManager : MonoBehaviour
{
    public static BarkManager Instance { get; private set; }

    [SerializeField] private List<BarkSO> barkList;
    [SerializeField] private BarkSO[] allBarks; // Auto-populated by weird editor script magic

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

    private void OnEnable()
    {
        Patient.OnPatientTraitsLoaded += HandlePatientTraitsLoaded;
    }

    private void OnDisable()
    {
        Patient.OnPatientTraitsLoaded -= HandlePatientTraitsLoaded;
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

#if UNITY_EDITOR
    public void RefreshBarkReferences()
    {
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:BarkSO");
        allBarks = new BarkSO[guids.Length];
        
        for (int i = 0; i < guids.Length; i++)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            allBarks[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<BarkSO>(path);
        }
        
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEngine.Debug.Log($"Auto-populated {allBarks.Length} bark references in BarkManager");
    }
#endif


    private void HandlePatientTraitsLoaded(string patientName)
    {
        Debug.Log($"BarkManager: Patient {patientName} traits loaded, processing bark traits...");
        
        List<TraitSO> barkTraits = TraitManager.Instance.GetTraitsByRepresentedOn("bark");
        Debug.Log($"Found {barkTraits.Count} bark traits");
        
        barkList.Clear();
        
        foreach (TraitSO barkTrait in barkTraits)
        {
            if (allBarks != null)
            {
                BarkSO matchingBark = System.Array.Find(allBarks, bark => bark.barkName == barkTrait.traitData);
                if (matchingBark != null)
                {
                    barkList.Add(matchingBark);
                }
                else
                {
                    Debug.LogWarning($"No BarkSO found with barkName matching traitData: {barkTrait.traitData}");
                }
            }
        }
        
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
