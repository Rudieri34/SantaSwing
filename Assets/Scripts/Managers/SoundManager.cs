using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Volume Managers")]
    [SerializeField] private AudioSource _soundEffectsSource;
    [HorizontalLine(color: EColor.Gray)]

    [Header("Hauntings")]
    [SerializeField] private List<AudioFile> _hauntingAudioClips;
    [HorizontalLine(color: EColor.Gray)]

    [Header("SFX")]
    [SerializeField] private List<AudioFile> _sfxClips;
    [HorizontalLine(color: EColor.Gray)]

    [Header("Loops")]
    [SerializeField] private AudioSource _loopSource;
    [SerializeField] private List<AudioFile> _audioClips;
    public bool IsSequencing;

    [HorizontalLine(color: EColor.Gray)]

    private GameObject _player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        PlayAudioLoop("BackgroundMusic");
    }

    public void PlaySFX(string audioName, float pitch = 1, float volume = 1)
    {
        AudioFile sfx = _sfxClips.FirstOrDefault(a => a.audioName == audioName);

        _soundEffectsSource.pitch = pitch;
        _soundEffectsSource.volume = volume;

        if (sfx.audio != null)
            _soundEffectsSource.PlayOneShot(sfx.audio);
        else
            Debug.LogError($"[SoundManager] The audio {audioName} was not found on the SFX clip list");
    }

    public void Play3DAudio(AudioClip clip, AudioSource audioSource)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayAudioLoop(string audioName, bool startSequencing = false)
    {
        IsSequencing = startSequencing;
        _loopSource.Stop();
        AudioFile loop = _audioClips.FirstOrDefault(a => a.audioName == audioName);

        if (IsSequencing)
            _loopSource.loop = false;
        else
            _loopSource.loop = true;

        _loopSource.clip = loop.audio;
        _loopSource.Play();
    }
    public void StopAudioLoop()
    {
        IsSequencing = false;
        _loopSource.Stop();
        _loopSource.loop = false;
    }
    public void StopSFX()
    {
        _soundEffectsSource.Stop();
    }


    public void PlayNextSong(AudioClip current)
    {
        int currentIndex = _audioClips.FindIndex(a => a.audio == current);

        AudioFile next = new AudioFile();
        if (currentIndex + 1 >= _audioClips.Count)
            next = _audioClips[0];
        else
            next = _audioClips[currentIndex + 1];

        _loopSource.clip = next.audio;
        _loopSource.Play();
    }

    private void Update()
    {

        if (IsSequencing && _loopSource.isPlaying == false)
        {
            PlayNextSong(_loopSource.clip);
        }
    }
}



[System.Serializable]
public struct AudioFile
{
    public string audioName;
    public AudioClip audio;
}

