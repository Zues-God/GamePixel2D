using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip energyClip;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip skillClip;
    [SerializeField] private AudioClip changeWeaponClip;
    [SerializeField] private AudioClip takeWeaponClip;



    public void PlayShootSound()
    {
        audioSource.PlayOneShot(shootClip);
    }

    public void PlayEnergySound()
    {
        audioSource.PlayOneShot(energyClip);
    }
    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackClip);
    }
    public void PlaySkillSound()
    {
        audioSource.PlayOneShot(skillClip);
    }
    public void PlayChangeWeaponSound()
    {
        audioSource.PlayOneShot(changeWeaponClip);
    }

    public void PlayTakeWeaponSound()
    {
        audioSource.PlayOneShot(takeWeaponClip);
    }


}
