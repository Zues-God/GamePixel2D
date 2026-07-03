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

    //--------------------------------------------------

    public void EnterPortal()
    {
        Debug.Log("Đã gọi EnterPortal()");

        if (currentBiome == null)
        {
            Debug.Log("Chưa có biome -> Random biome");
            StartNewRun();
            return;
        }

        Debug.Log("Đang có biome: " + currentBiome.biomeName);
    }

    //--------------------------------------------------

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
            Debug.Log("==============================");
            Debug.Log("Đã hoàn thành tất cả Biome!");
            Debug.Log("Reset danh sách Completed.");
            Debug.Log("==============================");

            completedBiomes.Clear();

            available.AddRange(allBiomes);
        }

        currentBiome =
            available[Random.Range(0, available.Count)];

        currentStage = 1;

        Debug.Log("==============================");
        Debug.Log("Biome được chọn:");
        Debug.Log(currentBiome.biomeName);
        Debug.Log("Stage: 1");
        Debug.Log("==============================");

        Debug.Log("Chuẩn bị load scene: " + currentBiome.stage1);
        SceneManager.LoadScene(currentBiome.stage1);
    }

    //--------------------------------------------------

    // TEST
    public void CompleteCurrentBiome()
    {
        if (currentBiome == null)
            return;

        Debug.Log("==============================");
        Debug.Log("Biome hoàn thành:");
        Debug.Log(currentBiome.biomeName);
        Debug.Log("==============================");

        completedBiomes.Add(currentBiome);

        currentBiome = null;

        currentStage = 0;

        SceneManager.LoadScene("LivingRoom");
    }

    //--------------------------------------------------

    // TEST
    [ContextMenu("Clear Data")]
    public void ClearData()
    {
        completedBiomes.Clear();

        currentBiome = null;

        currentStage = 0;

        Debug.Log("==============================");
        Debug.Log("Đã Clear toàn bộ dữ liệu.");
        Debug.Log("==============================");
    }

    //--------------------------------------------------

    public void PrintCompleted()
    {
        Debug.Log("===== COMPLETED =====");

        foreach (BiomeData biome in completedBiomes)
        {
            Debug.Log(biome.biomeName);
        }

        Debug.Log("=====================");
    }

    public void GoNextStage()
    {
        Debug.Log("===== GO NEXT STAGE =====");

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

                SceneManager.LoadScene("LivingRoom");

                break;
        }
    }
}