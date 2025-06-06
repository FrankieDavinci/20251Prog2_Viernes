using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioMixer _audioMixer;

    [SerializeField] AudioSource _audioSource;

    public float MasterVolume { get; private set; }
    public float MusicVolume { get; private set; }
    public float FxVolume { get; private set; }

    [SerializeField] AudioMixerSnapshot _pauseSnapshot;
    [SerializeField] AudioMixerSnapshot _normalSnapshot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    //Guardado en disco
    //private void Start()
    //{
    //    PlayerPrefs.SetFloat(nameof(MasterVolume), MasterVolume);

    //    if (PlayerPrefs.HasKey(nameof(MasterVolume)))
    //    {
    //        SetMasterVolume(PlayerPrefs.GetFloat(nameof(MasterVolume)));
    //    }
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //Si mantengo apretada la P, se mantiene el snapshot de pausa
        {
            _pauseSnapshot.TransitionTo(0.5f);
        }
        else if (Input.GetKeyUp(KeyCode.P)) //Si suelto la P, vuelve a su estado normal
        {
            _normalSnapshot.TransitionTo(0.5f);
        }
    }

    public void SetMasterVolume(float newValue)
    {
        MasterVolume = newValue;

        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(newValue) * 20f);
    }
    
    public void SetMusicVolume(float newValue)
    {
        MusicVolume = newValue;
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(newValue) * 20f);
    }
    
    public void SetFxVolume(float newValue)
    {
        FxVolume = newValue;
        _audioMixer.SetFloat("FxVolume", Mathf.Log10(newValue) * 20f);
    }
}
