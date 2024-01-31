using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;
    
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private Sound[] sounds;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }
    
    public void PlaySFX(SoundType soundType)
    {
        Sound sound = Array.Find(sounds, s => s.soundType == soundType);
        sfxSource.clip = sound.clip;
        if (sound.oneShot)
        {
            sfxSource.PlayOneShot(sound.clip);
            return;
        }
        sfxSource.Play();
    }
}

public enum SoundType
{
    PLAYER_ATTACK,
    PLAYER_HURT_1,
    PLAYER_HURT_2,
    PLAYER_EXPLODE
}

[Serializable]
class Sound
{
    public SoundType soundType;
    public AudioClip clip;
    public bool oneShot;
}