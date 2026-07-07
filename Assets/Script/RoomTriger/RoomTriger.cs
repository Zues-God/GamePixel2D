using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoomTriger : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    public GameObject bossHPBar, introBoss, player, boss;
    private List<GameObject> enemy = new List<GameObject>();
    public AudioSource bossSound;
    public BoxCollider2D spawArena;
    public int enemySpaw = 5;
    public float introDuration = 2.5f;
    private bool spawed = false;
    public Animator[] door;
    public Transform enemyParent;
    [SerializeField] private bool isBossRoom;
    [Header("Wave System")]
    [SerializeField] private int totalWaves = 3;
    [SerializeField] private int minEnemyPerWave = 2;
    [SerializeField] private int maxEnemyPerWave = 6;
    [SerializeField] private GameObject exitPortal;
    [SerializeField] private bool isLastRoom;
    private int currentWave = 0;
    private bool roomCleared = false;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spawed)
        {
            spawed = true;

            if (isBossRoom)
            {
                if (bossHPBar != null)
                    bossHPBar.SetActive(true);

                if (bossSound != null)
                    bossSound.Play();

                // activate boss after playing intro
                if (boss != null)
                {
                    StartCoroutine(ActivateBossAfterIntro());
                }

            }

            foreach (Animator d in door)
            {
                d.gameObject.SetActive(true);
            }

            // start normal waves only when not a boss room
            if (!isBossRoom)
            {
                StartNextWave();
            }
        }
    }



    void Start()
    {
        enemy.Clear();

        // If this room is a boss room, ensure boss gameobject is inactive until player enters
        if (isBossRoom && boss != null)
        {
            boss.SetActive(false);
        }

        foreach (Transform child in enemyParent)
        {
            Enemy e = child.GetComponent<Enemy>();

            if (e != null)
            {
                enemy.Add(child.gameObject);
            }
        }
    }


    void StartNextWave()
    {
        currentWave++;

        if (currentWave > totalWaves)
        {
            RoomCleared();
            return;
        }

        int enemyCount = Random.Range(minEnemyPerWave, maxEnemyPerWave + 1);

        SpawnWave(enemyCount);
    }

    void SpawnWave(int amount)
    {
        Bounds bounds = spawArena.bounds;

        for (int i = 0; i < amount; i++)
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);

            Vector2 spawnPos = new Vector2(randomX, randomY);

            GameObject randomEnemy = enemyPrefab[Random.Range(0, enemyPrefab.Length)];

            GameObject e = Instantiate(randomEnemy, spawnPos, Quaternion.identity, enemyParent);

            enemy.Add(e);
        }

        ActiveAllEnemy();
    }
    IEnumerator PlayIntro()
    {
        
        Time.timeScale = 0f;

        player.GetComponent<Player>().enabled = false;

        if (introBoss != null)
            introBoss.SetActive(true);
         player.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(introDuration);
        player.gameObject.SetActive(true);
        player.GetComponent<Player>().enabled = true;

        if (introBoss != null) introBoss.SetActive(false);

        Time.timeScale = 1f;

        GetComponent<Collider2D>().enabled = false;
    }

    private IEnumerator ActivateBossAfterIntro()
    {
        // Wait for intro coroutine to finish (play intro handles time scale and player disable)
        yield return StartCoroutine(PlayIntro());

        if (boss == null) yield break;

        // Activate boss GameObject and ensure its Enemy logic runs
        boss.SetActive(true);

        Enemy e = boss.GetComponent<Enemy>();
        if (e != null)
        {
            // add to tracking list so room knows there is an enemy
            enemy.Add(boss);
            e.ActivateEnemy();
        }

        // Optionally play boss intro animation or reset animator state
        Animator anim = boss.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("isRun", true);
        }

        // Start monitoring boss to clear room when defeated
        StartCoroutine(MonitorBossObject());
    }

    private IEnumerator MonitorBossObject()
    {
        // Wait until boss GameObject is destroyed (or set to null)
        while (boss != null)
        {
            yield return null;
        }

        // Boss defeated: hide HP bar and clear room
        if (bossHPBar != null) bossHPBar.SetActive(false);
        RoomCleared();
    }



    void Update()
    {
        if (!spawed || roomCleared) return;

        enemy.RemoveAll(e => e == null);

        if (enemy.Count == 0)
        {
            Debug.Log("Wave " + currentWave + " cleared!");

            // only start normal waves when not a boss room
            if (!isBossRoom)
            {
                StartNextWave();
            }
        }
    }
    void OpenDoor()
    {
        Debug.Log("Door Open!");

        foreach (Animator d in door)
        {
            if (d != null)
            {
                d.SetBool("isOpen", true);
                StartCoroutine(DisableAfterAnim(d));
            }

            if (isLastRoom && exitPortal != null)
            {
                exitPortal.SetActive(true);
            }

        }
    }
    void RoomCleared()
    {
        roomCleared = true;


        OpenDoor();
    }
    IEnumerator DisableAfterAnim(Animator d)
    {
        yield return new WaitForSeconds(0.3f);

        d.gameObject.SetActive(false);
    }
    void ActiveAllEnemy()
    {
        foreach (Transform child in enemyParent)
        {
            Enemy e = child.GetComponent<Enemy>();

            if (e != null)
            {
                e.ActivateEnemy();
            }

        }
    }
}