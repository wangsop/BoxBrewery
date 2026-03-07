using System.Collections.Generic;
using UnityEngine;

public class IngredientDropper : MonoBehaviour
{
    [SerializeField]
    private List<DroppedIngredient> _droppedIngredientsPool = new List<DroppedIngredient>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (DroppedIngredient ingredient in GetComponentsInChildren<DroppedIngredient>()) {
            _droppedIngredientsPool.Add(ingredient);
            ingredient.gameObject.SetActive(false);
        }
    }

    public void DropIngredient(int ingredientIndex)
    {
        foreach (DroppedIngredient ingredient in _droppedIngredientsPool)
        {
            if (!ingredient.gameObject.activeInHierarchy)
            {
                ingredient.gameObject.SetActive(true);
                ingredient.Activate(ingredientIndex);
                break;
            }
        }
    }
}
