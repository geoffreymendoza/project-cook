using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;


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
        Play("");
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: "+ name + " not found!");
            return;
        }
            
        s.Source.Play();
    }
}
