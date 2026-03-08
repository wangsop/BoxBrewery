using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net.NetworkInformation;
using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Potion> potions; //inventory of potions owned
    public List<Ingredient> inventory;
    public Customer? currentCustomer;
    public bool greeted;
    public List<Customer> customers;
    public bool offcooldown = true;
    public bool firstentry = true;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pausing");
            gameObject.GetComponentInChildren<Canvas>().enabled = true;
        }
    }
    public void QuitTo(int wh)
    {
        if (wh == 0)
        {
            SceneManager s = FindFirstObjectByType<SceneManager>();
            s.LoadScene("TitleScreen");
        }
        else
        {
            SceneManager s = FindFirstObjectByType<SceneManager>();
            s.QuitToDesktop();
        }
    }
    public void Resume()
    {
        gameObject.GetComponentInChildren<Canvas>().enabled = false;
    }
    public void AddIngredient(int index, int amount=1)
    {
        inventory[index] = new Ingredient(inventory[index], amount);
    }
    public void UseIngredient(int index)
    {
        inventory[index] = new Ingredient(inventory[index], -1);
    }
    public void AddPotion(int index)
    {
        potions[index] = new Potion(potions[index], 1);
    }
    public void UsePotion(int index)
    {
        potions[index] = new Potion(potions[index], -1);
    }
    public void StartCooldown()
    {
        Debug.Log("Starting cooldown");
        offcooldown = false;
        StartCoroutine(WaitCooldown());
    }
    public IEnumerator WaitCooldown()
    {
        float rand = UnityEngine.Random.Range(5.0f, 10.0f);
        yield return new WaitForSeconds(rand);
        Debug.Log("cooldown up");
        offcooldown = true;
    }
}

[System.Serializable]
public struct Potion
{
    public int index;
    public string name;
    public Sprite sprite;
    public int owned;
    public string description;
    public List<Ingredient> ingredients;
    public bool unlocked;
    public Potion(int i, string n, Sprite s, List<Ingredient> ing, bool u = false, int o=0, string d="")
    {
        index = i;
        name = n;
        sprite = s;
        owned = o;
        description = d;
        ingredients = new List<Ingredient>();
        for (int j = 0; j < ing.Count; j++)
        {
            ingredients.Add(new Ingredient(ing[j]));
        }
        unlocked = u;
    }
    public Potion(Potion o, bool u=false)
    {
        index = o.index;
        name = o.name;
        sprite = o.sprite;
        owned = o.owned;
        description = o.description;
        ingredients = new List<Ingredient>();
        for (int j = 0; j < o.ingredients.Count; j++)
        {
            ingredients.Add(new Ingredient(o.ingredients[j]));
        }
        unlocked = o.unlocked;
        if (u)
        {
            unlocked = u;
        }
    }
    public Potion(Potion o, int add)
    {
        index = o.index;
        name = o.name;
        sprite = o.sprite;
        owned = o.owned+add;
        description = o.description;
        ingredients = new List<Ingredient>();
        for (int j = 0; j < o.ingredients.Count; j++)
        {
            ingredients.Add(new Ingredient(o.ingredients[j]));
        }
        unlocked = o.unlocked;
    }
    public override bool Equals(object obj)
    {
        return name.Equals(((Potion)obj).name);
    }
    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
}
[System.Serializable]
public struct Ingredient
{
    public int index;
    public string name;
    public Sprite sprite;
    public int owned;
    public Ingredient(int i, string n, Sprite s, int o = 0)
    {
        index = i;
        name = n;
        sprite = s;
        owned = o;
    }
    public Ingredient(Ingredient i)
    {
        index = i.index;
        name = i.name;
        sprite = i.sprite;
        owned = i.owned;
    }
    public Ingredient(Ingredient i, int add)
    {
        index = i.index;
        name = i.name;
        sprite = i.sprite;
        owned = i.owned+add;
    }
    public override bool Equals(object obj)
    {
        return name.Equals(((Ingredient)obj).name);
    }
    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
}
