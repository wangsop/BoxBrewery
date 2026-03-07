using UnityEngine;

public class IngredientDropper : MonoBehaviour
{
    [SerializeField]
    private DroppedIngredient[] _droppedIngredientsPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _droppedIngredientsPool = GetComponentsInChildren<DroppedIngredient>();
    }

    public void DropIngredient(int ingredientIndex)
    {
        foreach (DroppedIngredient ingredient in _droppedIngredientsPool)
        {
            if (!ingredient.gameObject.activeInHierarchy)
            {
                ingredient.Activate(ingredientIndex);
                break;
            }
        }
    }
}
