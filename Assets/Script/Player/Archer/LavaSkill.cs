using System.Collections;
using UnityEngine;

public class LavaSkill : MonoBehaviour
{
    [Header("Skill Effect")]
    [SerializeField] private GameObject skillObject;

    [Header("Mana")]
    [SerializeField] private float manaCost = 30f;

    [Header("Cooldown")]
    [SerializeField] private float cooldown = 3f;

    [Header("Time Mark")]
    [SerializeField] private float markDuration = 4f;

    private float nextUseTime;
    private Player player;
    private Vector3 markedPosition;

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

        markedPosition = player.transform.position;

        StartCoroutine(TimeMarkRoutine());
    }

    private IEnumerator TimeMarkRoutine()
    {
        if (skillObject != null)
        {
            skillObject.transform.position = markedPosition;
            skillObject.SetActive(true);
        }

        yield return new WaitForSeconds(markDuration);

        player.transform.position = markedPosition;

        if (skillObject != null)
        {
            skillObject.SetActive(false);
        }
    }
}