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
    public Button orderButton;
    private GameManager gameManager;
    private List<int> holdingcell;
    [SerializeField]
    private IngredientDropper _dropper;
    [SerializeField]
    private CauldronLister _lister;
    private int _maxCauldronIngredients = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        holdingcell = new List<int>();
        gameManager = FindFirstObjectByType<GameManager>();
        unlock.gameObject.SetActive(false);
        if (_dropper == null)
            _dropper = FindFirstObjectByType<IngredientDropper>();
        if (_lister == null)
            _lister = FindFirstObjectByType<CauldronLister>();
        UpdateShelf();
        orderButton.gameObject.SetActive(false);
        for (int i = 1; i < gameManager.potions.Count; i++)
        {
            if (gameManager.potions[i].unlocked)
            {
                orderButton.gameObject.SetActive(true);
            }
        }
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
            else if (i < buttons.Count)
            {
                buttons[i].GetComponentInChildren<Image>().sprite = null;
                buttons[i].gameObject.SetActive(false);
                labels[i].text = "";
            }
        }
    }

    public void AddIngredient(int index)
    {
        SoundManager s = SoundManager.instance.GetComponent<SoundManager>();
        s.PlayClickSFX();
        if (holdingcell.Count >= _maxCauldronIngredients)
        {
            Debug.Log("Attempted adding to cauldron, but cauldron is full.");
            return;
        }
        Debug.Log("Add ingredient " + index + " to pot");
        holdingcell.Add(index);
        gameManager.UseIngredient(index);
        _dropper.DropIngredient(index);
        UpdateShelf();
        _lister.UpdateText();
    }

    public void CancelBrew()
    {
        for (int i = 0; i < holdingcell.Count; i++)
        {
            gameManager.AddIngredient(holdingcell[i]);
        }
        holdingcell = new List<int>();
        UpdateShelf();
        _lister.UpdateText();
        SoundManager s = SoundManager.instance.GetComponent<SoundManager>();
        s.PlayClickSFX();
    }

    public void Brew()
    {
        if (holdingcell.Count == 0)
        {
            return;
        }
        
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
                    SoundManager s = SoundManager.instance.GetComponent<SoundManager>();
                    s.PlayBrewSFX();
                    gameManager.AddPotion(i);
                    orderButton.gameObject.SetActive(true);
                    foundmatch = true;
                    Debug.Log(_lister.BuildString() + "made a " + gameManager.potions[i].name);
                    break;
                } else if (same)
                {
                    gameManager.AddPotion(i);
                    unlock.gameObject.SetActive(true);
                    SoundManager s = SoundManager.instance.GetComponent<SoundManager>();
                    s.PlaySuccessSFX();
                    unlock.GetComponentInChildren<TextMeshProUGUI>().text = "Successful brew: \n" + gameManager.potions[i].name;
                    foundmatch = true;
                    Debug.Log(_lister.BuildString() + "made a " + gameManager.potions[i].name);
                }
            }
        }
        if (!foundmatch)
        {
            gameManager.AddPotion(0);
            unlock.gameObject.SetActive(true);
            unlock.GetComponentInChildren<TextMeshProUGUI>().text = "Ambiguous brew? \n Slop Potion";
            Debug.Log(_lister.BuildString() + "made a slop potion");
            SoundManager s = SoundManager.instance.GetComponent<SoundManager>();
            s.PlayFailSFX();
        }
        holdingcell = new List<int>();
        _lister.UpdateText();
    }
    public void HideUnlock()
    {
        unlock.gameObject.SetActive(false);
    }

    public Dictionary<string, int> HoldingCellToDict()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        foreach (int index in holdingcell)
        {
            if (dict.ContainsKey(gameManager.inventory[index].name))
            {
                // add one
                dict[gameManager.inventory[index].name] = dict[gameManager.inventory[index].name] + 1;
            } else
            {
                // initialize entry
                dict.Add(gameManager.inventory[index].name, 1);
            }
        }
        return dict;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
