using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OrderBehaviour : MonoBehaviour
{
    [SerializeField] private SliderBarUI _sliderBarUI;
    [SerializeField] private Image _orderImage;
    [SerializeField] private Image[] _ingredientsImages;

    private Timer _timer;
    private ItemType _orderType;
    public ItemType GetOrderType() => _orderType;
    private bool _interrupt = false;
    private bool _init = false;
    public void Initialize(RecipeBags recipe)
    {
        if (_init) return;
        var duration = recipe.WaitingOrderDuration;
        _orderType = recipe.Type;
        _timer = new Timer(duration);
        _timer.OnTimerDone += OnDestroyObject;
        _sliderBarUI.Initialize(duration);
        _timer.OnUpdateTimeUI += _sliderBarUI.OnUpdateTimeUI;
        _orderImage.sprite = recipe.Sprite;
        for (int i = 0; i < _ingredientsImages.Length; i++)
        {
            _ingredientsImages[i].sprite = recipe.IngredientsData[i].IngredientSprite;
        }
        _init = true;
    }

    private void Update()
    {
        if(_interrupt) return;
        _timer.Tick(Time.deltaTime);
    }

    public void OrderServed()
    {
        _interrupt = true;
        OnDestroyObject();
    }

    private void OnDestroyObject()
    {
        _timer.OnTimerDone -= OnDestroyObject;
        OrderSystem.RemoveOrder(this);
        Destroy(this.gameObject);
    }
}