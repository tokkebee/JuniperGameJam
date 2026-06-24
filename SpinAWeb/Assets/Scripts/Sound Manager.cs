using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

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
    MenuMusic,
    GameMusic
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour

{
    [Header("To add more sounds, make more enums\n in script and give type and audio\n clip in inspector respectfully.")]
    [SerializeField] private SoundList[] soundList;
    public static SoundManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioSource loopingAudioSource;
    private float transitionTime = .333f; //.333f because thats how long it takes for transition to play



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
        Debug.LogWarning($"Sound: {sound} not found.");
    }

    #region GameMusic //How to use: StartCoroutine(SoundManager.MusicTransition(SoundType.{MUSIC ENUM}));
    public static IEnumerator MusicTransition(SoundType music) 
    {
        instance.loopingAudioSource.loop = true;
        foreach (SoundList i in instance.soundList)
        {
            if (i.type == music) //finds music
            {
                if (instance.loopingAudioSource.clip != i.audioClip) //checks if its not already playing
                {
                    instance.loopingAudioSource.clip = i.audioClip;
                    instance.loopingAudioSource.volume = Mathf.Lerp(1, 0, instance.transitionTime);
                    yield return new WaitForSeconds(instance.transitionTime);
                    instance.loopingAudioSource.volume = Mathf.Lerp(0, 1, instance.transitionTime);
                    instance.loopingAudioSource.Play();
                    yield break;
                }
                else 
                {
                    yield break;
                }
            }
        }
        Debug.LogWarning($"Music: {music} not found.");
    }
    #endregion

    [System.Serializable]
    public class SoundList
    {
        public SoundType type;
        public AudioClip audioClip;
    }
}


