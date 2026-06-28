using System.Collections;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private BossLaser laser;
    [SerializeField] private Transform players;
    [SerializeField] private Rigidbody2D rbs;
    [SerializeField] private MonoBehaviour movementScript;
    protected override void Start()
    {
        base.Start();

        laser.SetTarget(players);

        StartCoroutine(LaserLoop()); 
    }

    private IEnumerator FireLaser()
    {
        if (rbs != null)
            rbs.linearVelocity = Vector2.zero;

        if (movementScript != null)
            movementScript.enabled = false;

        yield return StartCoroutine(laser.Fire());

        if (movementScript != null)
            movementScript.enabled = true;
    }

    private IEnumerator LaserLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); 
            yield return StartCoroutine(FireLaser());
        }
    }
}