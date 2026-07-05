using UnityEngine;

public class Chest : MonoBehaviour
{

    [SerializeField] private Animator animationChest;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animationChest.SetTrigger("Open");

        }
    }
}
