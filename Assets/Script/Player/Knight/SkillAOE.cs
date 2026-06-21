using UnityEngine;
using System.Collections.Generic;

public class SkillAOE : MonoBehaviour
{
    public float force = 10f;

    private HashSet<Enemy> hitEnemies = new HashSet<Enemy>();

    private void OnEnable()
    {
        hitEnemies.Clear(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null || hitEnemies.Contains(enemy)) return;

        hitEnemies.Add(enemy);

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = (enemy.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, 0.5f, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
            {
                force *= 0.3f;
            }
            rb.linearVelocity = Vector2.zero;
           
        }
    }

}
