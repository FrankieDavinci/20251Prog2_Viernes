using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdater : MonoBehaviour
{
    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _fxSlider;

    private void Start()
    {
        if (AudioManager.Instance.MasterVolume != 0)
        {
            _masterSlider.value = AudioManager.Instance.MasterVolume;
        }
        else
        {
            _masterSlider.value = 0.75f;
            AudioManager.Instance.SetMasterVolume(.75f);
        }

        if (AudioManager.Instance.MusicVolume != 0)
        {
            _musicSlider.value = AudioManager.Instance.MusicVolume;
        }
        else
        {
            _musicSlider.value = 0.75f;
            AudioManager.Instance.SetMusicVolume(.75f);
        }

        if (AudioManager.Instance.FxVolume != 0)
        {
            _fxSlider.value = AudioManager.Instance.FxVolume;
        }
        else
        {
            _fxSlider.value = 0.75f;
            AudioManager.Instance.SetFxVolume(.75f);
        }
    }

    public void SetMasterVolume(float newValue)
    {
        AudioManager.Instance.SetMasterVolume(newValue);
    }

    public void SetMusicVolume(float newValue)
    {
        AudioManager.Instance.SetMusicVolume(newValue);
    }

    public void SetFxVolume(float newValue)
    {
        AudioManager.Instance.SetFxVolume(newValue);
    }
}
