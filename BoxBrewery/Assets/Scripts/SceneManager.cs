using UnityEngine;

public class SceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject popup;
    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        SoundManager.instance.GetComponent<SoundManager>().PlayRain();
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }
    public void Popup()
    {
        popup.SetActive(!popup.activeSelf);
    }
}
