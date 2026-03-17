using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Music clip")]
    public AudioClip musicClip;

    [Header("Audio Clips")]
    public AudioClip pickUpItemClip;
    public AudioClip gameWinClip;
    public AudioClip gameLoseClip;

    [Header ("VFX sounds")]
    public AudioClip cooldownClip;
    public AudioClip shotClip;
    [SerializeField] private AudioClip bmHit;
    [SerializeField] private AudioClip bmSpwn;
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private AudioClip castLightning;
    [SerializeField] private AudioClip clickButton;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    private const string MUSIC_VOL_KEY = "MusicVolume";
    private const string SFX_VOL_KEY = "SFXVolume";
    private const string MUSIC_MUTE_KEY = "MusicMuted";
    private const string SFX_MUTE_KEY = "SFXMuted";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Load dữ liệu PlayerPrefs
        LoadAudioSettings();
    }

    private void Start()
    {
        PlayMusic(musicClip);
    }

    // ------------------ LOAD / SAVE ------------------
    private void LoadAudioSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(MUSIC_VOL_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOL_KEY, 1f);

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;

        bool musicMuted = PlayerPrefs.GetInt(MUSIC_MUTE_KEY, 0) == 1;
        bool sfxMuted = PlayerPrefs.GetInt(SFX_MUTE_KEY, 0) == 1;

        musicSource.mute = musicMuted;
        sfxSource.mute = sfxMuted;
    }

    private void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat(MUSIC_VOL_KEY,Mathf.Clamp(musicVolume, 0f, 1f));
        PlayerPrefs.SetFloat(SFX_VOL_KEY, Mathf.Clamp(sfxVolume, 0f, 1f));

        PlayerPrefs.SetInt(MUSIC_MUTE_KEY, musicSource.mute ? 1 : 0);
        PlayerPrefs.SetInt(SFX_MUTE_KEY, sfxSource.mute ? 1 : 0);

        PlayerPrefs.Save();
    }

    // ------------------ GET VALUE ------------------
    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;
    public bool IsMusicMuted() => musicSource.mute;
    public bool IsSFXMuted() => sfxSource.mute;

    // ------------------ PLAY ------------------
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.Play();
    }

    public void PlayPickUpItem() => PlaySFX(pickUpItemClip);
    public void PlayGameWin() => PlaySFX(gameWinClip);
    public void PlayGameLose() => PlaySFX(gameLoseClip);
    public void PlayCooldown() => PlaySFX(cooldownClip);
    public void PlayShoot() => PlaySFX(shotClip);
    public void PlayBommerangeHit() => PlaySFX(bmHit);
    public void PlayBoomerangSpawm() => PlaySFX(bmSpwn);
    public void PlayEnemyHit() => PlaySFX(enemyHit);
    public void PlayCastLighning() => PlaySFX(castLightning);
    public void PlayClickButton() => PlaySFX(clickButton);
    // ------------------ VOLUME ------------------
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
        SaveAudioSettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = sfxVolume;
        SaveAudioSettings();
    }

    // ------------------ MUTE ------------------
    public void ToggleMusicMute(bool isMute)
    {
        musicSource.mute = isMute;
        SaveAudioSettings();
    }

    public void ToggleSFXMute(bool isMute)
    {
        sfxSource.mute = isMute;
        SaveAudioSettings();
    }
}
