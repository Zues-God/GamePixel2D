using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;
    [SerializeField] private CharacterSelectionUI ui;
    private CharacterDisplay currentDisplay;
    private void Awake()
    {
        Instance = this;
    }


    public void ShowCharacterInfo(
        CharacterData data, CharacterDisplay display)
    {
        currentDisplay = display;

        ui.Show(data);
    }

    public CharacterDisplay GetCurrentDisplay()
    {
        return currentDisplay;
    }
}