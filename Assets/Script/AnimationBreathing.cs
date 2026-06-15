using UnityEngine;

public class AnimationBreathing : MonoBehaviour
{
    public float breatheSpeed = 3f;     // Tốc độ thở (chỉnh tùy ý)
    public float breatheAmount = 0.03f; // Độ phập phồng (để số thật nhỏ)

    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        // Tạo nhịp thở lên xuống tuần hoàn
        float newY = startScale.y + Mathf.Sin(Time.time * breatheSpeed) * breatheAmount;

        // Cập nhật lại hình dáng
        transform.localScale = new Vector3(startScale.x, newY, startScale.z);
    }
}