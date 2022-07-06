using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public List<Sound> soundEffects;
    public List<Sound> music;

    public static AudioManager instance;
    public static Sound nowPlaying;

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

        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        if (ExitApplication.gameStarted)
        {
            PlayMusic("MenuTheme");
            ExitApplication.gameStarted = false;
        }
    }

    void Update()
    {
        /*
        int build = SceneManager.GetActiveScene().buildIndex;
        if (build < 3) //Menus
        {
            PlayMusic("MenuTheme");
        }
        else if (build == 3) //Food Select
        {
            PlayMusic("TestTheme");
        }
        else if (build == 4) //Grasslands
        {
            PlayMusic("MenuTheme");
        }
        else if (build == 5) //Caves
        {
            PlayMusic("MenuTheme");
        }
        else if (build == 6) //Flooded Caves
        {
            PlayMusic("MenuTheme");
        }
        else if (build == 7) //Magma
        {
            PlayMusic("MenuTheme");
        }
        else if (build == 8) //Unlock and Upgrades
        {
            PlayMusic("MenuTheme");
        }
        else if (build < 14) //Almanac
        {
            PlayMusic("MenuTheme");
        }
        else if (build == 14) //Game Over
        {
            PlayMusic("MenuTheme");
        }
        else //Should not reach here
        {
            PlayMusic("MenuTheme");
        }
        */
        if (!nowPlaying.source.isPlaying)
        {
            PlayMusic(nowPlaying.name);
        }
    }

    public void StopAllAudio()
    {
        foreach (Sound s in music)
        {
            if (s.source.isPlaying)
            {
                s.source.Stop();
            }
        }

        foreach (Sound s in soundEffects)
        {
            if (s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
    }

    public void PlayMusic(string name)
    {
        Sound sound = music.Find(s => s.name == name);
        nowPlaying = sound;
        if (!sound.source.isPlaying)
        {
            foreach (Sound s in music)
            {
                if (s.source.isPlaying)
                {
                    s.source.Stop();
                }
            }
            sound.source.Play();
        }
        Debug.Log("Playing " + sound.name);
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
