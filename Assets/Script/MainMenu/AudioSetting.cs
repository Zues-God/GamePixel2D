using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;

    private const string MASTER_VOLUME = "MyExposedParam";

    private void Start()
    {
        float volume =
            PlayerPrefs.GetFloat(MASTER_VOLUME, 1f);

        masterSlider.value = volume;

        SetMasterVolume(volume);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    public void SetMasterVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        // Slider 0~1 -> dB
        audioMixer.SetFloat(
            "MyExposedParam",
            Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat(MASTER_VOLUME, value);
        PlayerPrefs.Save();
    }
}