using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public SoundType Type;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f,3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource Source;
}

public enum SoundType
{
    None,
    MainMenuBGM,
    InGameBGM,
    ResultBGM,
    ButtonFX,
    ChopFX,
    CookFX,
    OrderCompleteFX,
    WashFX
}
