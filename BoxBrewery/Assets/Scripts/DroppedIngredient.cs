using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DroppedIngredient : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;
    private float _timeActive;
    private RectTransform _rParent;
    private float _leftBound;
    private float _rightBound;
    private float _startX;
    [SerializeField]
    private AnimationClip _dropClip;
    private Image _image;

    void Awake()
    {
        if (_gameManager == null)
            _gameManager = FindFirstObjectByType<GameManager>();

        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeActive >= _dropClip.length)
        {
            gameObject.SetActive(false);
            return;
        }
        _timeActive += Time.deltaTime;
    }

    public void Activate(int ingredientIndex)
    {
        _image.sprite = _gameManager.inventory[ingredientIndex].sprite;
        _timeActive = 0;
    }
}
