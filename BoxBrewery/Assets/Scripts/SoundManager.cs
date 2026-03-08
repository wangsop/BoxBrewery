using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource soundtrack;
    public AudioLowPassFilter lowpassfilter;
    public AudioSource sfx;
    public AudioSource rainsfx;
    public AudioClip bgm;
    public AudioClip fullrain;
    public AudioClip talksfx;
    public AudioClip brewsfx;
    public AudioClip clicksfx;
    public AudioClip pagesfx;
    public AudioClip moneysfx;
    public AudioClip failsfx;
    public AudioClip successsfx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    public void PlayTrack()
    {
        if (!soundtrack.isPlaying)
        {
            soundtrack.clip = bgm;
            soundtrack.Play();
        }
    }
    public void PlayRain()
    {
        if (!rainsfx.isPlaying)
        {
            rainsfx.Play();
        }
    }
    public void PlayTalkSFX()
    {
        if (!sfx.isPlaying)
        {
            sfx.clip = talksfx;
            sfx.Play();
        }
        
    }
    public void PlayBrewSFX()
    {
        sfx.clip = brewsfx;
        sfx.Play();
    }
    public void PlayClickSFX()
    {
        sfx.clip = clicksfx;
        sfx.Play();
    }
    public void PlayPageSFX()
    {
        sfx.clip = pagesfx;
        sfx.Play();
    }
    public void PlayMoneySFX()
    {
        sfx.clip = moneysfx;
        sfx.Play();
    }
    public void PlayFailSFX()
    {
        sfx.clip = failsfx;
        sfx.Play();
    }
    public void PlaySuccessSFX()
    {
        sfx.clip = successsfx;
        sfx.Play();
    }
}
