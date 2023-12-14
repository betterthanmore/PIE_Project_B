using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : GenericSingleton<SoundController>
{
    public AudioSource audio;
    public AudioSource bgmSource;
    public Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    public void SoundDataLoad()
    {
        AudioClip[] soundFiles = Resources.LoadAll<AudioClip>("AudioSource");
        foreach (AudioClip sound in soundFiles)
        {
            soundDictionary[sound.name] = sound;
        }
    }
    public void PlaySound(string name)
    {
        if (audio == null)
        {
            audio = GetComponent<AudioSource>();
        }

        AudioClip clip = soundDictionary[name];
        if (clip != null && audio != null)
        {
            audio.clip = clip;
            audio.Play();
        }
    }
    public void BGM(bool onOff)
    {
        if (bgmSource == null)
        {
            bgmSource = GameObject.Find("Bgm").GetComponent<AudioSource>();
        }

        bgmSource.mute = onOff;
    }
}
