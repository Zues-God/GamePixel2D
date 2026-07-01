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
            ShootArrow();
        }
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
        arrow.Shoot(dir, arrowSpeed);

        if (animator != null)
            animator.SetTrigger("isAttack");
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