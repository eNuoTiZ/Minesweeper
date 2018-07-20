using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public List<AudioSource> audioSourcesList;
    public Slider volumeSlider;

    // Use this for initialization
    void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(delegate { SetVolume(); });
        }
    }
    
    void SetVolume()
    {
        foreach (AudioSource source in audioSourcesList)
        {
            source.volume = volumeSlider.value;
        }

        switch (gameObject.name)
        {
            case "MusicVolumeSlider":
                Options.Instance.MusicVolume = volumeSlider.value;
                break;
            case "MenuSoundVolumeSlider":
                Options.Instance.MenuSoundsVolume = volumeSlider.value;
                break;
            case "SoundEffectsVolumeSlider":
                Options.Instance.SoundEffectsVolume = volumeSlider.value;

                foreach (AudioSource source in PrefabHelper.Instance.SoundEffectsAudioSourcesList)
                {
                    source.volume = Options.Instance.SoundEffectsVolume;
                }
                break;
            default:
                break;
        }
    }
}
