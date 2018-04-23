using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class Settings : MonoBehaviour {

    public Dropdown quality;
    public Slider master, music, sfx;
    public PostProcessingBehaviour behaviour;
    public PostProcessingProfile best, medium, low;

    public void OnEnable()
    {
        behaviour = Camera.main.GetComponent<PostProcessingBehaviour>();
        master.value = SoundManager.instance.MasterVolume;
        music.value = SoundManager.instance.MusicVolume;
        sfx.value = SoundManager.instance.SfxVolume;
        
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }


    public void Master()
    {
        SoundManager.instance.ChangeMasterVolume(master.value);
    }

    public void Music()
    {

        SoundManager.instance.ChangeMusicVolume(music.value);
    }

    public void SFX()
    {
        SoundManager.instance.ChangeSoundVolume(sfx.value);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Quality()
    {
        switch (quality.value)
        {
            case 0:
                QualitySettings.SetQualityLevel(2);
                behaviour.profile = best;
                break;
            case 1:
                QualitySettings.SetQualityLevel(1);
                behaviour.profile = medium;
                break;
            case 2:
                QualitySettings.SetQualityLevel(0);
                behaviour.profile = low;
                break;
        }
    }
}
