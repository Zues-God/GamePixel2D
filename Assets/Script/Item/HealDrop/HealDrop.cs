using UnityEngine;

public class HealDrop : MonoBehaviour
{
    public float attractDistance = 3f;
    public float moveSpeed = 10f;
    public float healAmount = 20f;

    private Transform player;
    private bool isAttracting = false;

    void Start()
    {
        player = FindAnyObjectByType<Player>().transform;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < attractDistance)
        {
            isAttracting = true;
        }

        if (isAttracting)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime * 8f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                p.Heal(healAmount);
            }

            Destroy(gameObject);
        }
    }
}