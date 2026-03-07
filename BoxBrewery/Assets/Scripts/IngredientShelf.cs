using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientShelf : MonoBehaviour
{
    public List<Button> buttons;
    public List<TextMeshProUGUI> labels;
    public Button unlock;
    private GameManager gameManager;
    private List<int> holdingcell;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        holdingcell = new List<int>();
        gameManager = FindFirstObjectByType<GameManager>();
        unlock.gameObject.SetActive(false);
        UpdateShelf();
    }
    private void UpdateShelf()
    {
        for (int i = 0; i < gameManager.inventory.Count; i++)
        {
            if (gameManager.inventory[i].owned != 0)
            {
                buttons[i].GetComponentInChildren<Image>().sprite = gameManager.inventory[i].sprite;
                buttons[i].gameObject.SetActive(true);
                labels[i].text = "x" + gameManager.inventory[i].owned;
                if (gameManager.inventory[i].owned <= -39)
                {
                    labels[i].text = "inf";
                }
            }
            else
            {
                buttons[i].GetComponentInChildren<Image>().sprite = null;
                buttons[i].gameObject.SetActive(false);
                labels[i].text = "";
            }
        }
    }

    public void AddIngredient(int index)
    {
        Debug.Log("Add ingredient " + index + " to pot");
        holdingcell.Add(index);
        gameManager.UseIngredient(index);
        UpdateShelf();
    }
    public void CancelBrew()
    {
        for (int i = 0; i < holdingcell.Count; i++)
        {
            gameManager.AddIngredient(holdingcell[i]);
        }
        UpdateShelf();
        holdingcell = new List<int>();
    }

    public void Brew()
    {
        Debug.Log("brewing");
        List<Ingredient> ingredients = new List<Ingredient>();
        for (int i = 0; i < holdingcell.Count; i++)
        {
            ingredients.Add(gameManager.inventory[holdingcell[i]]);
        }
        ingredients = ingredients.Distinct().ToList();
        bool foundmatch = false;
        for (int i = 0; i < gameManager.potions.Count; i++)
        {
            if (ingredients.Count == gameManager.potions[i].ingredients.Count)
            {
                bool same = true;
                for (int j = 0; j < gameManager.potions[i].ingredients.Count; j++)
                {
                    if (ingredients.Contains(gameManager.potions[i].ingredients[j]))
                    {
                        continue;
                    }
                    else
                    {
                        same = false;
                        break;
                    }
                }
                if (same && !gameManager.potions[i].unlocked)
                {
                    gameManager.potions[i] = new Potion(gameManager.potions[i], true);
                    //other behavior for recipe unlock should happen here
                    unlock.gameObject.SetActive(true);
                    unlock.GetComponentInChildren<TextMeshProUGUI>().text = "New potion unlocked: \n" + gameManager.potions[i].name;
                    Debug.Log("potion unlocked!");
                    gameManager.AddPotion(i);
                    foundmatch = true;
                    break;
                } else if (same)
                {
                    gameManager.AddPotion(i);
                    unlock.gameObject.SetActive(true);
                    unlock.GetComponentInChildren<TextMeshProUGUI>().text = "Successful brew: \n" + gameManager.potions[i].name;
                    foundmatch = true;
                }
            }
        }
        if (!foundmatch)
        {
            gameManager.potions[0] = new Potion(gameManager.potions[0], true); //this should be the Slop recipe (nothing potion)
            unlock.gameObject.SetActive(true);
            unlock.GetComponentInChildren<TextMeshProUGUI>().text = "Ambiguous brew? \n Slop Potion";
        }
        holdingcell = new List<int>();
    }
    public void HideUnlock()
    {
        unlock.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
