using UnityEngine;

[CreateAssetMenu(
    fileName = "New Character",
    menuName = "Character/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Info")]
    public string characterName;

    [TextArea]
    public string description;

    [Header("Stats")]
    public int hp;

    public int mana;

    public int attack;

    public int defense;

    [Header("Skill")]
    public string skillName;

    [TextArea]
    public string skillDescription;

    [Header("Visual")]
    public Sprite portrait;

    [Header("Prefab")]
    public GameObject playerPrefab;
}