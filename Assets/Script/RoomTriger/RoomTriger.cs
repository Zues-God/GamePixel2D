using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoomTriger : MonoBehaviour
{
    [SerializeField] private GameObject [] enemyPrefab;
    public GameObject bossHPBar, introBoss, player;
    private List<GameObject> enemy = new List<GameObject>();
    public AudioSource bossSound;
    public BoxCollider2D spawArena;
    public int enemySpaw = 5;
    public float introDuration = 2.5f;
    private bool spawed = false;
    public Animator [] door;
    public Transform enemyParent;
    private bool doorOpened = false;
    [SerializeField] private bool isBossRoom;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spawed)
        {
            spawed = true;
            SpawEnemy();
            ActiveAllEnemy();

            if (isBossRoom)
            {
                if (bossHPBar != null || introBoss != null || bossSound != null)
                    bossHPBar.SetActive(true);
                    StartCoroutine(PlayIntro());
                    bossSound.Play();
              
            }

            foreach (Animator d in door)
            {
                d.gameObject.SetActive(true);
            }
        }
    }

    void Start()
    {
        enemy.Clear();

        foreach (Transform child in enemyParent)
        {
            Enemy e = child.GetComponent<Enemy>();

            if (e != null)
            {
                enemy.Add(child.gameObject);
            }
        }
    }



    void SpawEnemy()
    {
        Debug.Log("Enemy Prefab = " + enemyPrefab);
        Bounds bounds = spawArena.bounds;
        for (int i = 0; i < enemySpaw; i++)
        {
            float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
            Vector2 spawnPos = new Vector2(randomX, randomY);
            GameObject randomEnemy = enemyPrefab[ Random.Range(0, enemyPrefab.Length)];
            GameObject e = Instantiate(randomEnemy, spawnPos, Quaternion.identity, enemyParent);
            enemy.Add(e);

        }

    }

    IEnumerator PlayIntro()
    {
        Time.timeScale = 0f;
        player.GetComponent<Player>().enabled = false;
        if (introBoss != null)
            introBoss.SetActive(true);
        yield return new WaitForSecondsRealtime(introDuration);
        player.GetComponent<Player>().enabled = true;
        if (introBoss != null)
            introBoss.SetActive(false);
        Time.timeScale = 1f;
        GetComponent<Collider2D>().enabled = false;
    }



    void Update()
    {
        if (!spawed || doorOpened) return;

        enemy.RemoveAll(e => e == null);

        Debug.Log("Enemy còn lại trong phòng: " + enemy.Count);


        if (enemy.Count == 0)
        {
            doorOpened = true;
            OpenDoor();
        }

        foreach (var e in enemy)
        {
            if (e != null)
            {
                Debug.Log("Enemy trong list: " + e.name);
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
        }
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
           Enemy e  = child.GetComponent<Enemy>();

            if (e != null)
            {
                e.ActivateEnemy();
            }
           
        }
    }
}