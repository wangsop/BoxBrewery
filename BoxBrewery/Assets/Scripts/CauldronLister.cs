using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CauldronLister : MonoBehaviour
{
    private TextMeshProUGUI _tmp;
    [SerializeField]
    private IngredientShelf _shelf;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        if (_shelf == null)
            _shelf = FindFirstObjectByType<IngredientShelf>();
    }

    public string BuildString()
    {
        Dictionary<string, int> dict = _shelf.HoldingCellToDict();
        string s = "";
        foreach (KeyValuePair<string, int> kvp in dict)
        {
            string line = string.Format("{0}x {1}\n", kvp.Value, kvp.Key);
            s += line;
        }
        if (s.Equals(""))
        {
            s = "Empty for now...";
        }
        return s;
    }

    public void UpdateText()
    {
        string s = BuildString();
        _tmp.text = s;
    }
}
