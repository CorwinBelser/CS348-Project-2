using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    public enum SoundEffects
    {
        PotionThrow,
        PotionBreak,
        ValidWord,
        InvalidWord,
        ReusedWord,
        UndoButton,
        MenuButton,
        SmokeBlowing
    }

    [SerializeField] private AudioClip potionThrow;
    [SerializeField] private AudioClip potionDrink;
    [SerializeField] private AudioClip potionBreak;
    [SerializeField] private AudioClip validWordA;
    [SerializeField] private AudioClip validWordB;
    [SerializeField] private AudioClip invalidWord;
    [SerializeField] private AudioClip reusedWord;
    [SerializeField] private AudioClip catMeow;
    [SerializeField] private AudioClip wolfHowl;
    [SerializeField] private AudioClip smokeBlowing;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("Duplicate AudioManager found. Destroying!");
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayEffect(SoundEffects effect)
    {
        switch (effect)
        {
            case SoundEffects.PotionBreak:
                audioSource.PlayOneShot(potionBreak);
                audioSource.PlayOneShot(potionDrink);
                break;
            case SoundEffects.PotionThrow:
                audioSource.PlayOneShot(potionThrow);
                break;
            case SoundEffects.ValidWord:
                if (Random.Range(0f, 1f) > .5f)
                    audioSource.PlayOneShot(validWordA);
                else
                    audioSource.PlayOneShot(validWordB);
                break;
            case SoundEffects.InvalidWord:
                audioSource.PlayOneShot(invalidWord);
                break;
            case SoundEffects.ReusedWord:
                audioSource.PlayOneShot(reusedWord);
                break;
            case SoundEffects.MenuButton:
                audioSource.PlayOneShot(wolfHowl);
                break;
            case SoundEffects.UndoButton:
                audioSource.PlayOneShot(catMeow);
                break;
            case SoundEffects.SmokeBlowing:
                audioSource.PlayOneShot(smokeBlowing);
                break;
        }
    }
}
