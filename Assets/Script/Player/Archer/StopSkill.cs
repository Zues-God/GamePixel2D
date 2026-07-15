using UnityEngine;

public class StopSkill  : MonoBehaviour
{
    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}