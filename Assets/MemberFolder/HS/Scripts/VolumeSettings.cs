using UnityEngine;
using UnityEngine.Audio; // Keep for mixer reference if needed, though not strictly necessary now
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;

    void Awake()
    {
        if (masterSlider == null)
        {
            Debug.LogError("Master Slider is not assigned in VolumeSettings!");
            return;
        }
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    private void Start()
    {
        float currentVolume = PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, 0.75f);

        masterSlider.SetValueWithoutNotify(currentVolume);

        // Optional: Immediately apply volume when settings panel opens?
        // AudioManager should already handle this on scene load, but if the panel
        // opens *after* scene load, you might want this line. However, the slider
        // value already reflects the loaded setting, so it might be redundant.
        // if (AudioManager.instance != null)
        // {
        //     AudioManager.instance.SetMasterVolume(currentVolume);
        // }
    }

    void SetMasterVolume(float value)
    {
        // Call the AudioManager's method to apply and save the volume
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMasterVolume(value);
        }
        else
        {
            Debug.LogError("AudioManager instance not found! Cannot set volume.");
        }
    }

    // If you need to remove the listener when the object is destroyed (good practice)
    void OnDestroy()
    {
        if (masterSlider != null)
        {
            masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        }
    }
}