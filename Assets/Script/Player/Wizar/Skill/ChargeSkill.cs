using UnityEngine;

public class ChargeSkill : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private GameObject chargeEffect;

    [Header("Charge")]
    [SerializeField] private float maxChargeTime = 5f;

    [Header("Cooldown")]
    [SerializeField] private float baseCooldown = 2f;

    [Header("Damage")]
    [SerializeField] private float minDamage = 100f;
    [SerializeField] private float maxDamage = 1000f;

    [Header("Scale")]
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 3f;

    [Header("Mana")]
    [SerializeField] private float manaPerSecond = 10f;

    private bool isChargingSkill;

    private float chargeStartTime;
    private float nextUseTime;

    private float finalChargeTime;
    private Vector3 targetPosition;

    private Player player;

    private void Start()
    {
        if (chargeEffect != null)
        {
            chargeEffect.SetActive(false);
        }
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (Time.time < nextUseTime)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCharge();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            ReleaseCharge();
        }
    }

    private void StartCharge()
    {
        isChargingSkill = true;

        chargeStartTime = Time.time;

        if (chargeEffect != null)
        {
            chargeEffect.SetActive(true);
        }
    }

    private void ReleaseCharge()
    {
        if (!isChargingSkill)
            return;

        isChargingSkill = false;

        if (chargeEffect != null)
        {
            chargeEffect.SetActive(false);
        }

        finalChargeTime =
            Mathf.Min(
                Time.time - chargeStartTime,
                maxChargeTime);

        targetPosition =
            Camera.main.ScreenToWorldPoint(
                Input.mousePosition);

        targetPosition.z = 0;

        if (finalChargeTime < 1)
        {
            float manaCost = (finalChargeTime * manaPerSecond) + manaPerSecond;
            float currentCooldown = (baseCooldown * finalChargeTime) + baseCooldown;

            if (!player.UseMana(manaCost))
            {
                return;
            }

            SpawnSkill();
            nextUseTime = Time.time + currentCooldown;
        }
        else
        {
            float manaCost = finalChargeTime * manaPerSecond;
            float currentCooldown = baseCooldown * finalChargeTime;
            if (!player.UseMana(manaCost))
            {
                return;
            }
            SpawnSkill();
            nextUseTime = Time.time + currentCooldown;

        }
    }

    private void SpawnSkill()
    {
        float chargePercent =
            finalChargeTime /
            maxChargeTime;

        float damage =
            Mathf.Lerp(
                minDamage,
                maxDamage,
                chargePercent);

        float scale =
            Mathf.Lerp(
                minScale,
                maxScale,
                chargePercent);

        GameObject skill =
            Instantiate(
                skillPrefab,
                targetPosition,
                Quaternion.identity);

        MeteorExplosion explosion =
            skill.GetComponent<MeteorExplosion>();

        if (explosion != null)
        {
            explosion.Initialize(
                scale,
                damage);
        }
    }
}