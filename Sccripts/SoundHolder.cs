using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum SoundsSFX
{
    EXPLOSION,
    JUMP,
    WEPON_PLAYER,
}
public enum SoundsMSK
{
    GAME,
    WIN,
    LOSE
}
[Serializable]
public struct SoundContainer
{
    public SoundsSFX key;
    public AudioClip clip;
}
[Serializable]
public struct MusikContainer
{
    public SoundsMSK key;
    public AudioClip clip;
}

public class SoundHolder : MonoBehaviour
{
    private static SoundHolder instace;

    [SerializeField] AudioSource sfx;
    [SerializeField] AudioSource musik;

    [SerializeField] List<SoundContainer> sfxContainer = new List<SoundContainer>();
    [SerializeField] List<MusikContainer> musikContainer = new List<MusikContainer>();

    private Dictionary<SoundsSFX, AudioClip> sfxDictionary = new Dictionary<SoundsSFX, AudioClip>();
    private Dictionary<SoundsMSK, AudioClip> musikDictionary = new Dictionary<SoundsMSK, AudioClip>();

    private void Awake()
    {
        if(instace == null)
        {
            instace = this;
        }
        DontDestroyOnLoad(this);
    }

    public static SoundHolder Instance => instace;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i< sfxContainer.Count; i++)
            sfxDictionary.Add(sfxContainer[i].key, sfxContainer[i].clip);
        for (int i = 0; i < musikContainer.Count; i++)
            musikDictionary.Add(musikContainer[i].key, musikContainer[i].clip);
    }

    public void PlaySFx(SoundsSFX sound)
    {
        sfx.clip = sfxDictionary[sound];
        sfx.Play();
    }
    public void PlayMusik(SoundsMSK sound, bool looping)
    {
        musik.Stop();
        musik.clip = musikDictionary[sound];
        musik.loop = looping;
        musik.Play();
    }
    public void ChangeSFXVolume (float volume)
    {
        Debug.Log(volume);
        sfx.volume = volume;
    }
    public void ChangeMusikVolume(float volume)
    {
        musik.volume = volume;
    }

    public void SetMuteSFX(bool mute)
    {
        sfx.mute = mute;
    }

    public void SetMuteMusik(bool mute)
    {
        musik.mute = mute;
    }

}
