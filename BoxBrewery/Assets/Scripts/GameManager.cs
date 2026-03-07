using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameObject instance;
    public List<Potion> potions; //inventory of potions owned
    public List<Ingredient> inventory;

     void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = gameObject;
        }
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
        inventory[index] = new Ingredient(inventory[index], 1);
    }
    public void UsePotion(int index)
    {
        inventory[index] = new Ingredient(inventory[index], -1);
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
