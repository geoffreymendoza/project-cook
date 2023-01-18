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
    private bool _init = false;
    public void Initialize(RecipeBags recipe)
    {
        if (_init) return;
        var duration = recipe.WaitingOrderDuration;
        _timer = new Timer(duration);
        _timer.OnTimerDone += OnDestroyObject;
        _sliderBarUI.Initialize(duration);
        _timer.OnUpdateTimeUI += _sliderBarUI.OnUpdateTimeUI;
        _orderImage.sprite = recipe.Sprite;
        //TODO even if for different ingredients
        Debug.Log(_ingredientsImages.Length);
        for (int i = 0; i < _ingredientsImages.Length; i++)
        {
            _ingredientsImages[i].sprite = recipe.IngredientsData[i].IngredientSprite;
            // _ingredientsImages[i].sprite = recipe.IngredientsData[0].IngredientSprite;
        }
        _init = true;
    }

    private void Update()
    {
        _timer.Tick(Time.deltaTime);
    }

    private void OnDestroyObject()
    {
        Destroy(this.gameObject);
    }
}