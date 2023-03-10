using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class RecipeSystem
{
    private static List<RecipeBags> _levelRecipeList = new List<RecipeBags>();
    
    public static void AddRecipeList(List<RecipeBags> recipe)
    {
        _levelRecipeList.Clear();
        _levelRecipeList = recipe;
        
        foreach (var rb in _levelRecipeList)
        {
            Debug.Log($"Recipe: {rb.Type} is added");
        }
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
        
        var currentIngredientTypes = plate.CurrentIngredients.Select(itm => 
            new IngredientsInfo(itm.Type, itm.State)).Select(i => i.IngredientType).ToList();
        
        var currentIngredientStates = plate.CurrentIngredients.Select(itm => 
            new IngredientsInfo(itm.Type, itm.State)).Select(i => i.IngredientState).ToList();
        
        //TODO refactor
        bool exactRecipe = false;
        ItemType finalRecipeType = recipeBag.Type;
        foreach (var recipe in _levelRecipeList)
        {
            //TODO check if all item states are equal also
            var recipeItemTypes = recipe.IngredientsData.Select(x => x.IngredientType).ToList();
            exactRecipe = currentIngredientTypes.IsEqual(recipeItemTypes);
            var recipeItemStates = recipe.IngredientsData.Select(x => x.IngredientState).ToList();
            exactRecipe = currentIngredientStates.IsEqual(recipeItemStates);
            if (!exactRecipe) continue;
            // finalRecipeType = recipe.Type;
            break;
        }
        if (!exactRecipe) return;

        // ItemType finalRecipeType = recipeBag.Type;
        var itemData = DataManager.GetItemData(finalRecipeType);
        var instanceItem = new Item(itemData);
        plate.ItemContainer.ChangeMesh(itemData);
        plate.CurrentIngredients.Clear();
        plate.AddIngredient(instanceItem);
        Debug.Log($"Recipe Created: {instanceItem.Type}!");
    }

    private static bool CheckExactRecipe(Item plateItem, Item cookingContainer)
    {
        var currentIngredientTypes = cookingContainer.CurrentIngredients.Select(itm => 
            new IngredientsInfo(itm.Type, itm.State)).Select(i => i.IngredientType).ToList();
        
        var currentIngredientStates = cookingContainer.CurrentIngredients.Select(itm => 
            new IngredientsInfo(itm.Type, itm.State)).Select(i => i.IngredientState).ToList();
        
        var exactRecipe = false;
        ItemType finalRecipeType = ItemType.Unassigned;
        foreach (var recipe in _levelRecipeList)
        {
            //TODO check if all item states are equal also
            var recipeItemTypes = recipe.IngredientsData.Select(x => x.IngredientType).ToList();
            exactRecipe = currentIngredientTypes.IsEqual(recipeItemTypes);
            var recipeItemStates = recipe.IngredientsData.Select(x => x.IngredientState).ToList();
            exactRecipe = currentIngredientStates.IsEqual(recipeItemStates);
            if (!exactRecipe) continue;
            finalRecipeType = recipe.Type;
            break;
        }
        if (!exactRecipe) return false;
        
        var itemData = DataManager.GetItemData(finalRecipeType);
        // Debug.Log(itemData.name);
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
            if (container.CurrentWorldSpaceTimerBehaviour != null)
            {
                container.ExtendTime(ingredient.InteractDuration);
            }
            else
            {
                AudioManager.instance.Play(SoundType.CookFX);
                Action onDone = () =>
                {
                    ingredient.UpStateByOne();
                    AudioManager.instance.Stop(SoundType.CookFX);
                };
                container.CreateTimerUI(ingredient.InteractDuration,onDone);
            }
        }
        container.AddIngredient(ingredient);

        if(ingredient.ItemContainer != null)
            UnityEngine.Object.Destroy(ingredient.ItemContainer.gameObject);
    }
}