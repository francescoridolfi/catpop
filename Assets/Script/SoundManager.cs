using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    [SerializeField] Button toggleMusicButton;

    // Start is called before the first frame update
    void Start()
    {

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    public void ToggleMusic()
    {

        if (AudioListener.volume > 0)
        {
            AudioListener.volume = 0;
            volumeSlider.value = 0;

        }
        else
        {
            volumeSlider.value = 0.5f;
            AudioListener.volume = volumeSlider.value;
        }
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}