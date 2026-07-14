using UnityEngine;

public class LavaSkill : MonoBehaviour
{
    [Header("Skill")]
    [SerializeField] private GameObject skillObject;

    [Header("Mana")]
    [SerializeField] private float manaCost = 30f;

    [Header("Cooldown")]
    [SerializeField] private float cooldown = 3f;

    private float nextUseTime;

    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();

        if (skillObject != null)
            skillObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UseSkill();
        }
    }

    private void UseSkill()
    {
        if (Time.time < nextUseTime)
            return;

        if (!player.UseMana(manaCost))
            return;

        nextUseTime = Time.time + cooldown;

        if (skillObject != null)
        {
            skillObject.SetActive(true);
        }
    }
}
