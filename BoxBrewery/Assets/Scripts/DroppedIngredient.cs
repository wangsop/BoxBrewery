using Unity.VisualScripting;
using UnityEngine;

public class DroppedIngredient : MonoBehaviour
{
    private GameManager _gameManager;
    private float _timeActive;
    private RectTransform _rParent;
    private float _leftBound;
    private float _rightBound;
    private float _startX;
    private AnimationClip _dropClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        _rParent = transform.parent.GetComponent<RectTransform>();
        _leftBound = _rParent.rect.xMin;
        _rightBound = _rParent.rect.xMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeActive >= _dropClip.length)
        {
            Deactivate();
            return;
        }
        transform.position = new Vector3(Mathf.Lerp(_startX, 0, _timeActive / _dropClip.length), transform.position.y, transform.position.z);
    }

    public void Activate(int ingredientIndex)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = _gameManager.inventory[ingredientIndex].sprite;
        _startX = Random.Range(_leftBound, _rightBound);
        transform.position = new Vector3(_startX, 0, transform.position.z);
        gameObject.SetActive(true);
        _timeActive = 0;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
