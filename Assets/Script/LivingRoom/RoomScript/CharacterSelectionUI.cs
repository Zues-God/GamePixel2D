using TMPro;
using UnityEngine;

public class CharacterSelectionUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Texts")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private TMP_Text skillText;
    [SerializeField] private TMP_Text descriptionText;



    private CharacterData currentCharacter;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void Show(CharacterData data)
    {
        currentCharacter = data;

        panel.SetActive(true);

        nameText.text =
            data.characterName;

        statsText.text =
            $"HP: {data.hp}\n" +
            $"Mana: {data.mana}\n" +
            $"Attack: {data.attack}\n" +
            $"Defense: {data.defense}";

        skillText.text =
            $"{data.skillName}\n\n" +
            data.skillDescription;

        descriptionText.text =
            data.description;
    }

    public void PlayCharacter()
    {

        CharacterDisplay display = SelectionManager.Instance.GetCurrentDisplay();

        Instantiate(currentCharacter.playerPrefab, display.spawnPoint.position, Quaternion.identity);

        panel.SetActive(false);
        display.gameObject.SetActive(false);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}