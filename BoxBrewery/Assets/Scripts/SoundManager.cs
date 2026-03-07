using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static GameObject instance;
    public AudioSource soundtrack;
    public AudioLowPassFilter lowpassfilter;
    public AudioSource sfx;
    public AudioSource rainsfx;
    public AudioClip bgm;
    public AudioClip fullrain;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = gameObject;
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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("MainScene"))
        {
            lowpassfilter.cutoffFrequency = 500;
        }
        else
        {
            lowpassfilter.cutoffFrequency = 5000;
        }
        if (!rainsfx.isPlaying)
        {
            rainsfx.Play();
        }
    }
}
