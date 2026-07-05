using UnityEngine;

public class HealEnemy : Enemy
{
    [SerializeField] private GameObject healDrop;

    protected override void DestroyEnemy()
    {

        if (healDrop != null)
        {
            GameObject heal = Instantiate(healDrop, transform.position, Quaternion.identity);
            Destroy(heal, 5f);
        }
        base.DestroyEnemy();
    }

}
