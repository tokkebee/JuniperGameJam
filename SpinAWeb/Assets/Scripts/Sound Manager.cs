using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

//To play sounds from SoundType lists
//SoundManager.PlaySound(SoundType.{SOUNDTYPE}, {OPTIONAL FLOAT FOR VOLUME});
//example: SoundManager.PlaySound(SoundType.UINegative);


public enum SoundType //To add more sounds, make more enums below and give type and audio clip in inspector respectfully.
{ 
    SpiderCrawl,
    SpiderStartWeb,
    SpiderWebBug,
    Crawlbugs,
    FlyBugs,
    UIPositive,
    UINegative,
    Music,
    Ambiance
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour

{
    [Header("To add more sounds, make more enums\n in script and give type and audio\n clip in inspector respectfully.")]
    [SerializeField] private SoundList[] soundList;
    public static SoundManager instance;
    private AudioSource audioSource;



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        foreach (SoundList i in instance.soundList)
        {
            if (i.type == sound)
            {
                instance.audioSource.PlayOneShot(i.audioClip, volume);
                return;
            }
        }
        Debug.LogWarning($"Sound {sound} not found.");
    }

    [System.Serializable]
    public class SoundList
    {
        public SoundType type;
        public AudioClip audioClip;
    }
}


