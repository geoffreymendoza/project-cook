using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class RecipeSystem
{
    private static Dictionary<ItemType, RecipeBags> _recipeData = new Dictionary<ItemType, RecipeBags>();
    private static List<RecipeBags> _levelRecipeList = new List<RecipeBags>();
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var recipeBags = Resources.LoadAll<RecipeBags>(Data.RECIPE_BAGS_PATH);
        foreach (var rb in recipeBags)
        {
            _recipeData[rb.Type] = rb;
        }
        
        //TODO Level System to handle below recipes
        _levelRecipeList.Clear();
        _recipeData.TryGetValue(ItemType.TomatoSoup, out RecipeBags rb1);
        _recipeData.TryGetValue(ItemType.MushroomSoup, out RecipeBags rb2);
        _recipeData.TryGetValue(ItemType.OnionSoup, out RecipeBags rb3);
        _recipeData.TryGetValue(ItemType.FishSashimi, out RecipeBags rb4);
        _levelRecipeList.Add(rb1);
        _levelRecipeList.Add(rb2);
        _levelRecipeList.Add(rb3);
        _levelRecipeList.Add(rb4);
        // Debug.Log(_levelRecipeList.Count);
    }

    public static void CombineItem(Item container, Item ingredient)
    {
        bool CombineHoldingPlate(Item plateItem, Item cookingContainer)
        {
            if (plateItem.Type is not ItemType.Plate || cookingContainer.Type is not ItemType.CookContainer)
                return false;

            if (CheckExactRecipe(plateItem, cookingContainer))
                return true;

            var item = plateItem.CurrentIngredients[0];
            if (item.State is ItemState.Sliced)
            {
                MoveItemToContainer(cookingContainer, item);
                plateItem.CurrentIngredients.Clear();
                return true;
            }
            return false;
        }
        
        bool CombineHoldingCookContainer(Item cookingContainer, Item plateItem)
        {
            if (plateItem.Type is not ItemType.Plate || cookingContainer.Type is not ItemType.CookContainer)
                return false;
            return CheckExactRecipe(plateItem, cookingContainer);
        }

        (List<RecipeBags> possibleRecipes, ItemType[] currentIngredients)  GetPossibleRecipe()
        {
            var currentIngredients = container.CurrentIngredients.Select(c => c.Type).ToArray();
            var possibleRecipeList = (from currentIngredient in currentIngredients 
                from recipeBag in _levelRecipeList 
                from info in recipeBag.IngredientsData 
                where currentIngredient == info.IngredientType
                select recipeBag).Distinct().ToList();
            return (possibleRecipeList, currentIngredients);
        }

        bool combinedAtPlate =  CombineHoldingPlate(container, ingredient);
        if (combinedAtPlate) return;
        
        combinedAtPlate = CombineHoldingCookContainer(container, ingredient);
        if (combinedAtPlate) return;

        //HACK only sliced/cooked are allowed in plate/cook container to start recipe
        if (container.CurrentIngredients.Count is 0 && ingredient.State != ItemState.Raw)
        {
            if (container.Type is ItemType.CookContainer && !ingredient.CanCook) return;
            if (container.Type is ItemType.DirtyPlate) return;
            MoveItemToContainer(container, ingredient);
            
            var recipeList = GetPossibleRecipe();
            foreach (var rb in recipeList.possibleRecipes.Where(rb => 
                         rb.IngredientsData.Length >= recipeList.currentIngredients.Length))
            {
                CheckExactRecipeFromIngredient(container, rb);
            }
            return;
        }

        //ASSUME character only have the ingredient
        if (container.CurrentIngredients.Count >= 0 && ingredient.State != ItemState.Raw)
        {
            //TODO only specified recipe lists on that level
            var recipeList = GetPossibleRecipe();
            if (recipeList.possibleRecipes.Count <= 0) return;

            foreach (var rb in recipeList.possibleRecipes.Where(rb => 
                         rb.IngredientsData.Length > recipeList.currentIngredients.Length))
            {
                MoveItemToContainer(container, ingredient);
                CheckExactRecipeFromIngredient(container, rb);
            }
        }
    }

    private static void CheckExactRecipeFromIngredient(Item plate, RecipeBags recipeBag)
    {
        if (recipeBag == null) return;
        if (plate.CurrentIngredients.Count != recipeBag.IngredientsData.Length) return;
        if (plate.Type is not ItemType.Plate) return;

        ItemType finalRecipeType = recipeBag.Type;
        var itemData = DataManager.GetItemData(finalRecipeType);
        var instanceItem = new Item(itemData);
        plate.CurrentIngredients.Clear();
        plate.AddIngredient(instanceItem);
        Debug.Log($"Recipe Created: {instanceItem.Type}!");
    }

    private static bool CheckExactRecipe(Item plateItem, Item cookingContainer)
    {
        var currentIngredientInfoList = cookingContainer.CurrentIngredients.Select(itm => 
            new IngredientsInfo(itm.Type, itm.State)).Select(i => i.IngredientType).ToList();
        
        var exactRecipe = false;
        ItemType finalRecipeType = ItemType.Unassigned;
        foreach (var recipe in _levelRecipeList)
        {
            //TODO check if all item states are equal also
            var recipeItemTypes = recipe.IngredientsData.Select(x => x.IngredientType).ToList();
            exactRecipe = currentIngredientInfoList.IsEqual(recipeItemTypes);
            if (!exactRecipe) continue;
            finalRecipeType = recipe.Type;
            break;
        }
        if (!exactRecipe) return false;
        
        var itemData = DataManager.GetItemData(finalRecipeType);
        var instanceItem = new Item(itemData);
        plateItem.AddIngredient(instanceItem);
        plateItem.ItemContainer.ChangeMesh(itemData);
        cookingContainer.CurrentIngredients.Clear();
        Debug.Log($"Recipe Created: {instanceItem.Type}!");
        return true;
    }
    
    private static void MoveItemToContainer(Item container, Item ingredient)
    {
        if (container.Type == ItemType.CookContainer)
        {
            if (container.CurrentTimerBehaviour != null)
            {
                container.ExtendTime(ingredient.InteractDuration);
            }
            else
            {
                Action onDone = ingredient.UpStateByOne;
                container.CreateTimerUI(ingredient.InteractDuration,onDone);
            }
        }
        container.AddIngredient(ingredient);

        if(ingredient.ItemContainer != null)
            UnityEngine.Object.Destroy(ingredient.ItemContainer.gameObject);
    }
}