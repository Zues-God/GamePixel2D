using UnityEngine;

public class ExitGate : MonoBehaviour
{
    private bool isLoading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isLoading)
            return;

        if (!other.CompareTag("Player"))
            return;

        isLoading = true;

        RunManager.Instance.GoNextStage();
    }
}