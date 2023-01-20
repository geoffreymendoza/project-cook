using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    private List<Sound> _currentSoundList = new List<Sound>();


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.clip;

            s.Source.volume = s.volume;
            s.Source.pitch = s.pitch;
            s.Source.loop = s.loop; 
        }
    }

    private void Start()
    {
        // Play("");
    }

    public Sound Play (SoundType type)
    {
        Sound s = GetSound(type);
        if (s == null)
        {
            // Debug.LogWarning("Sound: "+ type + " not found!");
            return null;
        }
            
        s.Source.Play();
        _currentSoundList.Add(s);
        return s;
    }

    public void Stop(SoundType type)
    {
        Sound s = GetSound(type);
        if (s == null)
        {
            // Debug.LogWarning("Sound: "+ type + " not found!");
            return;
        }
        s.Source.Stop();
        _currentSoundList.Remove(s);
    }

    public bool CheckIfPlaying(SoundType type)
    { 
        Sound s = GetSound(type);
        return s.Source.isPlaying;
    }
    
    private Sound GetSound(SoundType type)
    {
        Sound s = Array.Find(sounds, sound => sound.Type == type);
        return s;
    }

    public void StopAll()
    {
        foreach (var s in _currentSoundList)
        {
            s.Source.Stop();
        }
        _currentSoundList.Clear();
    }
}
