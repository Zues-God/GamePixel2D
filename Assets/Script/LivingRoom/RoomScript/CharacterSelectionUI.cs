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

    [SerializeField] private CameraController lobbyCamera;

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
            $"{data.hp}\n" +
            $"{data.mana}\n" +
            $"{data.attack}\n";

        skillText.text =
            $"{data.skillName}\n\n";

        descriptionText.text =
            data.skillDescription;
    }

    public void PlayCharacter()
    {

        CharacterDisplay display = SelectionManager.Instance.GetCurrentDisplay();

        if (CharacterManager.Instance != null)
        {
            CharacterManager.Instance.SelectCharacter(currentCharacter);
        }

        Instantiate(currentCharacter.playerPrefab, display.spawnPoint.position, Quaternion.identity);

        panel.SetActive(false);
        display.gameObject.SetActive(false);
    }

    public void Hide()
    {
        panel.SetActive(false);

        if (lobbyCamera != null)
        {
            lobbyCamera.BackToLobby();
        }

        currentCharacter = null;
    }

    public void CloseButton()
    {
        panel.SetActive(false);

        currentCharacter = null;

        if (lobbyCamera != null)
        {
            lobbyCamera.BackToLobby();
        }
    }
}