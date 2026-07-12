using System.Collections;
using UnityEngine;

public class RoomBoss : RoomTriger
{
    [SerializeField] private GameObject bossPrefab;
    private bool triggered = false;
    private GameObject bossInstance;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (!collision.CompareTag("Player")) return;

        triggered = true;

        if (door != null)
        {
            foreach (Animator d in door)
            {
                if (d != null) d.gameObject.SetActive(true);
            }
        }

        if (bossHPBar != null) bossHPBar.SetActive(true);

        if (bossSound != null) bossSound.Play();

        StartCoroutine(PlayIntroAndSpawn());
    }

    private IEnumerator PlayIntroAndSpawn()
    {
        Time.timeScale = 0f;

        if (player != null)
        {
            var pl = player.GetComponent<Player>();
            if (pl != null) pl.enabled = false;
            player.gameObject.SetActive(false);
        }

        if (introBoss != null) introBoss.SetActive(true);

        yield return new WaitForSecondsRealtime(introDuration);

        if (player != null)
        {
            player.gameObject.SetActive(true);
            var pl = player.GetComponent<Player>();
            if (pl != null) pl.enabled = true;
        }

        if (introBoss != null) introBoss.SetActive(false);

        Time.timeScale = 1f;

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        SpawnBoss();
        StartCoroutine(MonitorBoss());
    }

    private void SpawnBoss()
    {
        if (bossPrefab == null) return;

        Vector2 spawnPos = transform.position;

        if (spawArena != null)
        {
            spawnPos = spawArena.bounds.center;
        }

        bossInstance = Instantiate(bossPrefab, spawnPos, Quaternion.identity, enemyParent);

        Enemy e = bossInstance.GetComponent<Enemy>();
        if (e != null)
        {
            e.ActivateEnemy();
        }
    }

    private IEnumerator MonitorBoss()
    {
        while (bossInstance != null)
        {
            yield return null;
        }

        if (door != null)
        {
            foreach (Animator d in door)
            {
                if (d != null)
                {
                    d.SetBool("isOpen", true);
                    StartCoroutine(DisableAfterAnim(d));
                }
            }
        }
    }

    private IEnumerator DisableAfterAnim(Animator d)
    {
        yield return new WaitForSeconds(0.3f);
        if (d != null) d.gameObject.SetActive(false);
    }

}
