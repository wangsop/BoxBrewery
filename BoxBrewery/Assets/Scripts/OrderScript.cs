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
    public Button character;
    public Sprite questionmark;
    public GameObject orderSlip;
    public Image potionorder;
    public Image ingredientorder;
    public Button nextButton;
    public Button fillorder;
    private string line;
    private string named;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        chatbox.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        orderSlip.gameObject.SetActive(false);
        if (GameManager.instance.GetComponent<GameManager>().currentCustomer != null)
        {
            charImage.sprite = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).sprite;
            if (GameManager.instance.GetComponent<GameManager>().greeted)
            {
                orderSlip.SetActive(true);
                character.interactable = false;
            }
            else
            {
                character.interactable = true;
                line = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).request;
                name = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).name;
            }
            potionorder.sprite = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).potion.sprite;
            if (((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).ingredients.Count > 1)
            {
                ingredientorder.sprite = questionmark;
            }
            else
            {
                ingredientorder.sprite = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).ingredients[0].sprite;
            }
        } else if (GameManager.instance.GetComponent<GameManager>().offcooldown)
        {
            NewCustomer();
        }
        else
        {
            charImage.gameObject.SetActive(false);
        }
    }

    public void NewCustomer()
    {
        charImage.gameObject.SetActive(true);
        character.interactable = true;
        int index = 0;
        List<int> temp = new List<int>();
        for (int i = 0; i < GameManager.instance.GetComponent<GameManager>().customers.Count; i++)
        {
            if (GameManager.instance.GetComponent<GameManager>().potions[GameManager.instance.GetComponent<GameManager>().customers[i].potion.index].unlocked)
            {
                temp.Add(i);
            }
        }
        index = UnityEngine.Random.Range(0, temp.Count);
        index = temp[index];
        GameManager.instance.GetComponent<GameManager>().currentCustomer = GameManager.instance.GetComponent<GameManager>().customers[index];
        charImage.sprite = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).sprite;
        line = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).request;
        named = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).name;
        GameManager.instance.GetComponent<GameManager>().greeted = false;
        GameManager.instance.GetComponent<GameManager>().offcooldown = false;
    }

    public void StartDialogue()
    {
        Debug.Log("starting dialogue");
        character.interactable = false;
        GameManager.instance.GetComponent<GameManager>().greeted = true;
        StartCoroutine(WriteLine());
    }

    public IEnumerator WriteLine()
    {
        Debug.Log("writing line");
        nextButton.gameObject.SetActive(false);
        chatbox.gameObject.SetActive(true);
        namelabel.text = named;
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
        Debug.Log("Hiding text");
        Debug.Log(GameManager.instance.GetComponent<GameManager>().currentCustomer == null);
        chatbox.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        if (GameManager.instance.GetComponent<GameManager>().currentCustomer == null)
        {
            Debug.Log("ending dialogue with no current customer");
            charImage.gameObject.SetActive(false);
            orderSlip.gameObject.SetActive(false);
            GameManager.instance.GetComponent<GameManager>().StartCooldown();
        }
        else
        {
            Debug.Log("there exists a customer");
            Debug.Log(((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).name);
            orderSlip.SetActive(true);
            potionorder.sprite = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).potion.sprite;
            if (((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).ingredients.Count > 1)
            {
                ingredientorder.sprite = questionmark;
            }
            else
            {
                ingredientorder.sprite = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).ingredients[0].sprite;
            }
        }
    }
    public void FulfillOrder()
    {
        int index = GameManager.instance.GetComponent<GameManager>().potions.IndexOf(((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).potion);
        if (GameManager.instance.GetComponent<GameManager>().potions[index].owned > 0)
        {
            GameManager.instance.GetComponent<GameManager>().UsePotion(index);
            int amount = UnityEngine.Random.Range(1, 4);
            int ind = UnityEngine.Random.Range(0, ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).ingredients.Count);
            GameManager.instance.GetComponent<GameManager>().AddIngredient(ind, amount);
            
            orderSlip.SetActive(false);
            line = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).response;
            named = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).name;
            GameManager.instance.GetComponent<GameManager>().currentCustomer = null;
            Debug.Log("setting GameManager.instance.GetComponent<GameManager>() current customer to null after fulfilling order");
            if (GameManager.instance.GetComponent<GameManager>().currentCustomer != null)
            {
                Debug.Log("not null: " + ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).name);
            }
            StartCoroutine(WriteLine());
        }
        else
        {
            fillorder.image.color = Color.red;
            fillorder.GetComponentInChildren<TextMeshProUGUI>().text = "Not Enough";
        }
        //order fulfill visual indicators
        
    }
    public void CancelOrder()
    {
        orderSlip.SetActive(false);
        line = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).sadresponse;
        named = ((Customer)GameManager.instance.GetComponent<GameManager>().currentCustomer).name;
        GameManager.instance.GetComponent<GameManager>().currentCustomer = null;
        StartCoroutine(WriteLine());
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetComponent<GameManager>().currentCustomer == null && GameManager.instance.GetComponent<GameManager>().offcooldown)
        {
            Debug.Log("getting new customer from update");
            NewCustomer();
            GameManager.instance.GetComponent<GameManager>().offcooldown = false;
        }
    }
}

[System.Serializable]
public struct Customer
{
    public string name;
    public string request;
    public string response;
    public string sadresponse;
    public Sprite sprite;
    public Potion potion;
    public List<Ingredient> ingredients;
}
