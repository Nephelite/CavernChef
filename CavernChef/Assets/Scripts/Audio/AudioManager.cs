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

    public AudioMixerGroup musicMixer, sfxMixer;
    public AudioMixer masterMixer;

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
            s.source.outputAudioMixerGroup = sfxMixer;
        }

        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = musicMixer;
        }

        if (ExitApplication.gameStarted)
        {
            GlobalVariables.settings = SaveSystem.LoadSettings();
            PlayMusic("MenuTheme");
            ExitApplication.gameStarted = false;
        }
    }

    public Sound Clone(Sound sound)
    {
        if (sound != null)
        {
            Sound clone = new Sound();
            clone.name = sound.name;
            clone.volume = sound.volume;
            clone.pitch = sound.pitch;
            clone.clip = Instantiate(sound.clip) as AudioClip;
            clone.source = gameObject.AddComponent<AudioSource>();
            clone.source.clip = clone.clip;
            clone.source.volume = sound.volume;
            clone.source.pitch = sound.pitch;
            clone.source.outputAudioMixerGroup = sound.source.outputAudioMixerGroup;
            return clone;
        }
        else
        {
            return null;
        }
    }

    void Update()
    {
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
            sound.source.volume *= GlobalVariables.settings.musicVol;
            sound.source.Play();
        }
        Debug.Log("Playing " + sound.name);
    }

    public void UpdateNewMusicVolume()
    {
        Debug.Log(Mathf.Log10(GlobalVariables.settings.musicVol) * 20);
        masterMixer.SetFloat("MusicVol", Mathf.Log10 (GlobalVariables.settings.musicVol) * 20);
    }

    public void UpdateNewSFXVolume()
    {
        masterMixer.SetFloat("SFXVol", Mathf.Log10(GlobalVariables.settings.SFXVol) * 20);
    }

    public void Play (string name)
    {
        Sound sound = soundEffects.Find(s => s.name == name);
        sound.source.volume *= GlobalVariables.settings.SFXVol;
        sound.source.PlayOneShot(sound.clip);
    }

    public Sound PlayLoop(string name)
    {
        Sound sound = Clone(soundEffects.Find(s => s.name == name));
        if (!sound.source.isPlaying)
        {
            sound.source.Play();
        }
        return sound;
    }

    public Sound GetSound(string name)
    {
        return Clone(soundEffects.Find(s => s.name == name));
    }

    public void StopSFX(Sound sfx)
    {
        sfx.source.Stop();
    }
}
