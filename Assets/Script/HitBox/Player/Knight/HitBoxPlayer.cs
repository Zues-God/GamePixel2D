using UnityEngine;

public class HitBox : MonoBehaviour
{

    public float damage = 10f;
    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, transform.root);
                Debug.Log("Hit enemy " + Time.frameCount);
                Debug.Log(damage);
               
                
            }
        }
    }
}
