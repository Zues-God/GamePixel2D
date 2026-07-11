using System.Collections.Generic;
using UnityEngine;

public class PlayerBowAttack : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed = 10f;
    public int poolSize = 15;

    private List<Arrow> arrowPool = new List<Arrow>();
    private Animator animator;

    private bool canShoot = true;
    private bool shootRequested = false;

    public Animator animationAttack;

    void Start()
    {
        animator = GetComponent<Animator>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(arrowPrefab);
            obj.SetActive(false);

            arrowPool.Add(obj.GetComponent<Arrow>());
        }
    }

    void Update()
    {
        // CHỈ 1 LẦN / CLICK + CHỐNG GIỮ INPUT LỖI
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            shootRequested = true;
            canShoot = false;

            if (animator != null)
                animator.SetTrigger("isAttack");
        }

    }

    public void AnimationShoot()
    {
        Debug.Log("AnimationShoot");
        Debug.Log("shootRequested = " + shootRequested);
        if (!shootRequested)
            return;

        shootRequested = false;

        ShootArrow();
    }

    public void EnableShoot()
    {
        canShoot = true;
    }

    public void DisableShoot()
    {
        canShoot = false;
    }

    void ShootArrow()
    {
        if (arrowPrefab == null || firePoint == null) return;

        Arrow arrow = GetArrowFromPool();
        if (arrow == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 dir = (mousePos - firePoint.position).normalized;

        arrow.transform.position = firePoint.position;
        arrow.SetAttacker(transform);

        Debug.Log("Shoot");
        arrow.Shoot(dir, arrowSpeed);

        Debug.Log(arrow);

        if (arrow == null)
        {
            Debug.Log("No Arrow");
            return;
        }

    }

    Arrow GetArrowFromPool()
    {
        foreach (var a in arrowPool)
        {
            if (!a.gameObject.activeInHierarchy)
                return a;
        }
        return null;
    }
}
