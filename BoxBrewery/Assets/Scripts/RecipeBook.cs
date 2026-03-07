using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class RecipeBook : MonoBehaviour
{
    public List<Potion> potions;
    public Sprite unknownpotion;
    public Image image;
    public TextMeshProUGUI namelabel;
    public TextMeshProUGUI description;
    public TextMeshProUGUI ingredientslabel;
    public TextMeshProUGUI amountlabel;
    public Canvas canvas;
    public Button book;
    private int index;
    private bool active = false;
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas.gameObject.SetActive(false);
        active = false;
        gameManager = FindFirstObjectByType<GameManager>();
    }
    public void Show()
    {
        if (active){
            return;
        }
        SoundManager s = SoundManager.instance.GetComponent<SoundManager>();
        s.PlayPageSFX();
        canvas.gameObject.SetActive(true);
        book.gameObject.SetActive(false);
        potions = gameManager.potions;
        for (int i = 0; i < gameManager.potions.Count; i++)
        {
            if (!gameManager.potions[i].unlocked)
            {
                potions[i] = new Potion(gameManager.potions[i].index, "???", unknownpotion, new List<Ingredient>(), false, 0, gameManager.potions[i].description); //change to "This potion has not been unlocked yet!" if description isn't needed as a hint
            }
        }
        if (potions != null && potions.Count > 0)
        {
            image.sprite = potions[0].sprite;
            namelabel.text = potions[0].name;
            description.text = potions[0].description;
            amountlabel.text = "x"+potions[0].owned;
            ingredientslabel.text = "";
            for (int i = 0; i < potions[0].ingredients.Count; i++)
            {
                ingredientslabel.text += "x1 " + potions[0].ingredients[i].name + "\n";
            }
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
        SoundManager s = SoundManager.instance.GetComponent<SoundManager>();
        s.PlayPageSFX();
        if (direction > 0)
        {
            index++;
            index = index % potions.Count;
            image.sprite = potions[index].sprite;
            namelabel.text = potions[index].name;
            description.text = potions[index].description;
            amountlabel.text = "x" + potions[index].owned;
            ingredientslabel.text = "";
            for (int i = 0; i < potions[index].ingredients.Count; i++)
            {
                ingredientslabel.text += "x1 " + potions[index].ingredients[i].name + "\n";
            }
        } else if (direction < 0)
        {
            index--;
            if (index < 0)
            {
                index += potions.Count;
            }
            image.sprite = potions[index].sprite;
            namelabel.text = potions[index].name;
            description.text = potions[index].description;
            amountlabel.text = "x" + potions[index].owned;
            ingredientslabel.text = "";
            for (int i = 0; i < potions[index].ingredients.Count; i++)
            {
                ingredientslabel.text += "x1 " + potions[index].ingredients[i].name + "\n";
            }
        }
    }
}
