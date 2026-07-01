using UnityEngine;

public class Gate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(RunManager.Instance);
        Debug.Log("Có object đi vào Portal: " + other.name);

        Player player = other.GetComponent<Player>();

        if (player == null)
            return;

        Debug.Log("Player vào cổng");

        RunManager.Instance.EnterPortal();
    }
}