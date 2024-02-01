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
    
    public void PlayBGM(SoundType soundType)
    {
        Sound sound = Array.Find(sounds, s => s.soundType == soundType);

        bgmSource.clip = sound.clip;
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

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}

public enum SoundType
{
    BGM_MENU,
    PLAYER_ATTACK,
    PLAYER_HURT_1,
    PLAYER_HURT_2,
    ENEMY_HURT_1,
    ENEMY_HURT_2,
    ENEMY_DEATH_1,
    ENEMY_DEATH_2,
    PLAYER_DEATH,
    POWER_UP,
    DOOR_OPEN,
    GAME_END
}

[Serializable]
class Sound
{
    public SoundType soundType;
    public AudioClip clip;
    public bool oneShot;
}