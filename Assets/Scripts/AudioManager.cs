using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    public enum SoundEffects
    {
        PotionThrow,
        PotionBreak
    }

    [SerializeField] private AudioClip potionThrow;
    [SerializeField] private AudioClip potionBreak;
    private AudioSource audio;

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
        audio = GetComponent<AudioSource>();
    }

    public void PlayEffect(SoundEffects effect)
    {
        switch (effect)
        {
            case SoundEffects.PotionBreak:
                audio.PlayOneShot(potionBreak);
                break;
            case SoundEffects.PotionThrow:
                audio.PlayOneShot(potionThrow);
                break;
        }
    }
}
