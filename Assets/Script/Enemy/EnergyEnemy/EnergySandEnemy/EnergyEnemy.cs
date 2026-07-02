using UnityEngine;

public class EnergyEnemy : Enemy
{
    [SerializeField] private GameObject energyDrop;
    protected override void DestroyEnemy()
    {

        if (energyDrop != null)
        {
            GameObject energy = Instantiate(energyDrop, transform.position, Quaternion.identity);
            Destroy(energy, 5f);
        }
        base.DestroyEnemy();
    }
}
