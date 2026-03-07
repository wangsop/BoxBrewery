using UnityEngine;

public class SceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadScene(string scene)
    {
        SoundManager.instance.GetComponent<SoundManager>().PlayRain();
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
