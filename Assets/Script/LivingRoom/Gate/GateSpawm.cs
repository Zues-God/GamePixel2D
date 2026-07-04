using UnityEngine;

public class GateSpawm : MonoBehaviour
{
    public static GateSpawm Instance;

    [SerializeField] private GameObject portal;

    private void Awake()
    {
        Instance = this;

        portal.SetActive(false);
    }

    public void ShowPortal()
    {
        portal.SetActive(true);
    }
}