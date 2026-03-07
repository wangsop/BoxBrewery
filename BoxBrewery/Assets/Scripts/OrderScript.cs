using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class OrderScript : MonoBehaviour
{
    public Button chatbox;
    public TextMeshProUGUI namelabel;
    public TextMeshProUGUI dialoguebox;
    public Image charImage;
    public Sprite questionmark;
    public GameObject orderSlip;
    public Image potionorder;
    public Image ingredientorder;
    public Button nextButton;
    private GameManager gameManager;
    public Button fillorder;
    private string line;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        chatbox.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        orderSlip.gameObject.SetActive(false);
        if (gameManager.currentCustomer != null)
        {
            charImage.sprite = ((Customer)gameManager.currentCustomer).sprite;
            orderSlip.SetActive(true);
            potionorder.sprite = ((Customer)gameManager.currentCustomer).potion.sprite;
            namelabel.text = ((Customer)gameManager.currentCustomer).name;
            if (((Customer)gameManager.currentCustomer).ingredients.Count > 1)
            {
                ingredientorder.sprite = questionmark;
            }
            else
            {
                ingredientorder.sprite = ((Customer)gameManager.currentCustomer).ingredients[0].sprite;
            }
        } else if (gameManager.offcooldown)
        {
            NewCustomer();
        }
    }

    public void NewCustomer()
    {
        charImage.gameObject.SetActive(true);
        int index = 0;
        List<int> temp = new List<int>();
        for (int i = 0; i < gameManager.potions.Count; i++)
        {
            if (gameManager.potions[i].unlocked)
            {
                temp.Add(i);
            }
        }
        index = UnityEngine.Random.Range(0, temp.Count);
        index = temp[index];
        gameManager.currentCustomer = gameManager.customers[index];
        charImage.sprite = ((Customer)gameManager.currentCustomer).sprite;
        namelabel.text = ((Customer)gameManager.currentCustomer).name;
        line = ((Customer)gameManager.currentCustomer).request;
        StartCoroutine(WriteLine());
        orderSlip.SetActive(true);
        potionorder.sprite = ((Customer)gameManager.currentCustomer).potion.sprite;
        if (((Customer)gameManager.currentCustomer).ingredients.Count > 1)
        {
            ingredientorder.sprite = questionmark;
        }
        else
        {
            ingredientorder.sprite = ((Customer)gameManager.currentCustomer).ingredients[0].sprite;
        }
    }

    public IEnumerator WriteLine()
    {
        chatbox.gameObject.SetActive(true);
        dialoguebox.SetText("");
        string updating = "";
        foreach (char c in line.ToCharArray())
        {
            updating += c;
            dialoguebox.SetText(updating);
            yield return new WaitForSeconds(0.03f);
        }
        nextButton.gameObject.SetActive(true);
    }
    public void HideText()
    {
        chatbox.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        if (gameManager.currentCustomer == null)
        {
            charImage.gameObject.SetActive(false);
        }
    }
    public void FulfillOrder()
    {
        int index = gameManager.potions.IndexOf(((Customer)gameManager.currentCustomer).potion);
        if (gameManager.potions[index].owned > 0)
        {
            gameManager.UsePotion(index);
            int amount = UnityEngine.Random.Range(1, 4);
            int ind = UnityEngine.Random.Range(0, ((Customer)gameManager.currentCustomer).ingredients.Count);
            gameManager.AddIngredient(ind, amount);
            orderSlip.SetActive(false);

            line = ((Customer)gameManager.currentCustomer).response;
            StartCoroutine(WriteLine());
            gameManager.currentCustomer = null;
            gameManager.StartCooldown();
        }
        else
        {
            fillorder.image.color = Color.red;
            fillorder.GetComponentInChildren<TextMeshProUGUI>().text = "Not Enough";
        }
        //order fulfill visual indicators
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public struct Customer
{
    public string name;
    public string request;
    public string response;
    public Sprite sprite;
    public Potion potion;
    public List<Ingredient> ingredients;
}
