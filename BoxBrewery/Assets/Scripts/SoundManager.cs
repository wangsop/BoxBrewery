using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static GameObject instance;
    public AudioSource soundtrack;
    public AudioSource sfx;
    public AudioSource rainsfx;
    public AudioClip bgm;
    public AudioClip fullrain;
    public AudioClip muffledrain;
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
        rainsfx.Stop();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("MainScene"))
        {
            rainsfx.clip = muffledrain;
        }
        else
        {
            rainsfx.clip = fullrain;
        }
        rainsfx.Play();
    }
}
