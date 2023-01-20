using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    //ANIMATIONS
    public const string ANIM_NAVIGATION_SPEED = "nav_speed";
    
    //INPUT CONTEXT
    public const string INPUT_MOVE = "Move";
    public const string INPUT_DASH = "Dash";
    public const string INPUT_GRAB = "Grab";
    public const string INPUT_INTERACT = "Interact";

    //RESOURCES PATH
    public const string ITEM_BAGS_PATH = "Item_Bags"; 
    public const string INTERACT_BAGS_PATH = "Interact_Bags"; 
    public const string RECIPE_BAGS_PATH = "Recipe_Bags"; 
    public const string UI_BAGS_PATH = "UI_Bags";
    public const string LEVEL_BAGS_PATH = "Level_Bags";
    public const string SPAWN_BAGS_PATH = "Spawn_Bags";
    
    //SCENE NAMES
    public const string MAIN_MENU_SCENE = "MainMenu";
    public const string LOBBY_SCENE = "lobby";
    public const string GAME_SCENE = "game";
    public const string RESULTS_SCENE = "results";

    //LAYER MASKS
    private static readonly int InteractableLayer = LayerMask.NameToLayer("Interactable");
    public static readonly int InteractableLayerMask = 1 << InteractableLayer;
}

public enum EntityType
{
    Unassigned,
    PlayableCharacter,
    
}

public enum SpawnType
{
    None,
    TimerBehaviour,
    PlayerEntity,
    CharacterManager,
    PlayerInputManager,
    AudioManager,
    
}
