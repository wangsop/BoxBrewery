using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject popup;
    [SerializeField] private List<Sprite> images;
    [SerializeField] private GameObject cutsceneCanvas;
    private static string ActiveScene = "TitleScreen";
    private bool clicked = false;
    // Update is called once per frame
    private void Start()
    {
        if (cutsceneCanvas != null)
        {
            cutsceneCanvas.SetActive(false);
        }
    }
    void Update()
    {
        if (!clicked && Input.GetMouseButtonDown(0))
        {
            clicked = true;
        }
    }
    public void StartGame()
    {
        if (GameManager.instance.GetComponent<GameManager>().firstentry)
        {
            GameManager.instance.GetComponent<GameManager>().firstentry = false;
            StartCoroutine(RunCutscene());
            //do comic first
        }
        else
        {
            LoadScene("MainScene");
        }
    }
    public IEnumerator RunCutscene()
    {
        cutsceneCanvas.SetActive(true);
        clicked = false;
        for (int i = 0; i < images.Count; i++)
        {
            cutsceneCanvas.GetComponentInChildren<Image>().sprite = images[i];
            yield return new WaitUntil(() => clicked == true);
            clicked = false;
        }
        LoadScene("MainScene");
    }
    public void LoadScene(string scene)
    {
        if (!(scene.Equals(ActiveScene)))
        {
            ActiveScene = scene;
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
    }
    public void QuitToDesktop()
    {
        Debug.Log("Quitting to desktop");
        Application.Quit();
    }
    public void Popup()
    {
        popup.SetActive(!popup.activeSelf);
    }
}
