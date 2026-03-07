using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecipeBook : MonoBehaviour
{
    public List<Sprite> recipes;
    public Image image;
    public Canvas canvas;
    public Button book;
    private int index;
    private bool active = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
        canvas.gameObject.SetActive(false);
        active = false;
    }
    public void Show()
    {
        if (active){
            return;
        }
        canvas.gameObject.SetActive(true);
        book.gameObject.SetActive(false);
        if (recipes != null && recipes.Count > 0)
        {
            image.sprite = recipes[0];
            index = 0;
        }
        active = true;
    }
    public void Hide()
    {
        canvas.gameObject.SetActive(false);
        book.gameObject.SetActive(true);
        index = 0;
        active = false;
    }
    public void Cycle(int direction)
    {
        if (direction > 0)
        {
            index++;
            index = index % recipes.Count;
            image.sprite = recipes[index];
        } else if (direction < 0)
        {
            index--;
            if (index < 0)
            {
                index += recipes.Count;
            }
            image.sprite = recipes[index];
        }
    }
}
