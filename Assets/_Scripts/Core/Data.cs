using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public const string ANIM_NAVIGATION_SPEED = "nav_speed";
    public const string INPUT_MOVE = "Move";
    public const string INPUT_DASH = "Dash";
    public const string INPUT_GRAB = "Grab";
    public const string INPUT_INTERACT = "Interact";
    
}

public enum EntityType
{
    Unassigned,
    PlayableCharacter,
    
}
