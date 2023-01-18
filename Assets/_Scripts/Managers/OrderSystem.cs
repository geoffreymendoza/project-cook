using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OrderSystem
{
    private static List<RecipeBags> _levelRecipeList = new List<RecipeBags>();
    private static List<ItemType> _currentOrdersList = new List<ItemType>();
    private static int _currentOrders;
    private static int _maxOrders = 7;
    
    private static int _randomIndex;
    private static TimerBehaviour _currentTimer;
    private static GameObject _ordersGridUI;

    public static void ResetOrdersList()
    {
        _currentOrdersList.Clear();
        _currentOrders = 0;
    }

    public static void AddRecipeList(List<RecipeBags> recipe)
    {
        _levelRecipeList.Clear();
        _levelRecipeList = recipe;
    }

    private static void AddOrder(ItemType order)
    {
        _currentOrdersList.Add(order);
        SpawnOrderUI();
        _currentOrders++;
        Debug.Log($"New order: {order}");
        if (_currentOrders >= _maxOrders) return;
        StartOrdering();
    }

    public static void RemoveOrder(ItemType order)
    {
        _currentOrdersList.Remove(order);
        _currentOrders--;
    }

    public static void CheckOrder(ItemType order)
    {
        //if done remove
        //check if current order list are not full, then start ordering again
    }

    public static void StartOrdering()
    {
        var inGame = GameManager.StillInGame();
        if (!inGame) return;
        _randomIndex = Random.Range(0, _levelRecipeList.Count);
        var instance = DataManager.GetSpawnData<TimerBehaviour>(SpawnType.TimerBehaviour);
        _currentTimer = Object.Instantiate(instance);
        var intervalTime = _levelRecipeList[_randomIndex].IntervalOrderDuration;
        _currentTimer.Initialize(intervalTime);
        _currentTimer.Timer.OnTimerDone += OnTimerDone;
    }

    private static void SpawnOrderUI()
    {
        RecipeBags orderData = _levelRecipeList[_randomIndex];
        var ingredientsCount = orderData.IngredientsData.Length;
        Debug.Log(ingredientsCount);
        var mainCanvas = UIManager.GetMainCanvas();
        if (_ordersGridUI == null)
        {
            _ordersGridUI = UIManager.GetUIObject(UIType.OrdersGrid);
            _ordersGridUI.transform.SetParent(mainCanvas.transform, false);
        }
        OrderBehaviour orderBehaviour = ingredientsCount switch
        {
            1 => UIManager.GetUIObject<OrderBehaviour>(UIType.OneIngredientOrder),
            2 => UIManager.GetUIObject<OrderBehaviour>(UIType.TwoIngredientOrder),
            3 => UIManager.GetUIObject<OrderBehaviour>(UIType.ThreeIngredientOrder),
            _ => null
        };
        orderBehaviour.Initialize(orderData);
        orderBehaviour.transform.SetParent(_ordersGridUI.transform, false);
    }

    private static void OnTimerDone()
    {
        _currentTimer.Timer.OnTimerDone -= OnTimerDone;
        var orderRecipe = _levelRecipeList[_randomIndex].Type;
        AddOrder(orderRecipe);
    }
}
