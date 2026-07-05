using UnityEngine;

public class EnergyDrop : MonoBehaviour
{
    [SerializeField] private float energyValue = 5f;
    public float attractDistance = 3f;
    public float moveSpeed = 10f;
    private Transform player;
    private bool isAttracting = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.AddEnergy(energyValue);
        }

        Destroy(gameObject);
    }

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

}