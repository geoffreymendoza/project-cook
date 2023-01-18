using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class OrderSystem
{
    private static List<RecipeBags> _levelRecipeList = new List<RecipeBags>();
    private static List<OrderBehaviour> _currentOrdersList = new List<OrderBehaviour>();
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

    private static void AddOrder()
    {
        if (_currentOrders >= _maxOrders) return;
        SpawnOrder();
        StartOrdering();
    }

    public static void RemoveOrder(OrderBehaviour order)
    {
        _currentOrdersList.Remove(order);
        _currentOrders--;
        AddOrder();
    }

    public static void ServeOrder(ItemType orderType)
    {
        Debug.Log(orderType);
        bool correctOrder = false;
        //if done remove order
        foreach (var order in _currentOrdersList.Where(t => t.GetOrderType() == orderType))
        {
            Debug.Log(order.GetOrderType());
            order.OrderServed();
            correctOrder = true;
            break;
        }

        if (correctOrder)
        {
            //ADD SCORE + REMAINING TIME TO SCORE SYSTEM
            Debug.Log("Correct Order");

        }
        else
        {
            //DEDUCT SCORE TO SCORE SYSTEM
            Debug.Log("Wrong Order");
        }
    }

    public static void StartOrdering()
    {
        var inGame = GameManager.StillInGame();
        if (!inGame) return;
        _randomIndex = Random.Range(0, _levelRecipeList.Count);
        //debug to check if wrong order or not
        // _randomIndex = 0;
        var instance = DataManager.GetSpawnData<TimerBehaviour>(SpawnType.TimerBehaviour);
        _currentTimer = Object.Instantiate(instance);
        var intervalTime = _levelRecipeList[_randomIndex].IntervalOrderDuration;
        _currentTimer.Initialize(intervalTime);
        _currentTimer.Timer.OnTimerDone += OnTimerDone;
    }

    private static void SpawnOrder()
    {
        RecipeBags orderData = _levelRecipeList[_randomIndex];
        var ingredientsCount = orderData.IngredientsData.Length;
        if (_ordersGridUI == null)
        {
             var mainCanvas = UIManager.GetMainCanvas();
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
        _currentOrdersList.Add(orderBehaviour);
        _currentOrders++;
        // Debug.Log($"New order: {orderBehaviour.GetOrderType()}");
    }

    private static void OnTimerDone()
    {
        _currentTimer.Timer.OnTimerDone -= OnTimerDone;
        AddOrder();
    }
}
