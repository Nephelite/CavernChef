using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public List<Sound> soundEffects;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return ;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in soundEffects)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play (string name)
    {
        Sound sound = soundEffects.Find(s => s.name == name);
        sound.source.PlayOneShot(sound.clip);
    }

    public void PlayLoop(string name)
    {
        Sound sound = soundEffects.Find(s => s.name == name);
        if (!sound.source.isPlaying)
        {
            sound.source.Play();
        }
    }
}
