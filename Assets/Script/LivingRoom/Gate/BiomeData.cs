using UnityEngine;

[CreateAssetMenu(fileName = "NewBiome", menuName = "Game/Biome")]
public class BiomeData : ScriptableObject
{
    public string biomeName;

    [Header("Scenes")]
    public string stage1;
    public string stage2;
    public string bossScene;
}