using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    [Header("All Biomes")]
    [SerializeField] private List<BiomeData> allBiomes;

    private List<BiomeData> completedBiomes = new();

    private BiomeData currentBiome;

    private int currentStage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnterPortal()
    {
        if (currentBiome == null)
        {
            StartNewRun();
            return;
        }
    }

    private void StartNewRun()
    {
        Debug.Log("StartNewRun()");

        List<BiomeData> available = new();

        foreach (BiomeData biome in allBiomes)
        {
            if (!completedBiomes.Contains(biome))
                available.Add(biome);
        }

        if (available.Count == 0)
        {
 
            completedBiomes.Clear();

            available.AddRange(allBiomes);
        }

        currentBiome =
            available[Random.Range(0, available.Count)];

        currentStage = 1;

        SceneManager.LoadScene(currentBiome.stage1);
    }

    public void CompleteCurrentBiome()
    {
        if (currentBiome == null)
            return;

        completedBiomes.Add(currentBiome);

        currentBiome = null;

        currentStage = 0;

        SaveManager.Instance.DeleteSave();

        SceneManager.LoadScene("MainLobby");
    }

    [ContextMenu("Clear Data")]
    public void ClearData()
    {
        completedBiomes.Clear();
        currentBiome = null;
        currentStage = 0;
    }

    public void PrintCompleted()
    {
        foreach (BiomeData biome in completedBiomes)
        {
            Debug.Log(biome.biomeName);
        }
    }

    public void GoNextStage()
    {

        switch (currentStage)
        {
            case 1:
                currentStage = 2;
                SceneManager.LoadScene(currentBiome.stage2);
                break;

            case 2:
                currentStage = 3;
                SceneManager.LoadScene(currentBiome.bossScene);
                break;

            case 3:
                Debug.Log("Biome Completed!");
                CompleteCurrentBiome();
                break;
        }
    }
}