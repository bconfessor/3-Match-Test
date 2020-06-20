using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;


    public AudioClip mainSongClip;

    //played when player completes a 3-chain or higher
    public AudioClip gemClearClip;

    //played when player selects a gem with the mouse
    public AudioClip gemSelectClip;

    //played when player swaps two gems' places
    public AudioClip gemSwapClip;


    //holds GO that contains the audio source that will continue playing the BGM
    public GameObject bgmAudioGO;
    public AudioSource bgmSource;

    //holds GO that contains the audio source that plays the sound effects
    public GameObject sfxAudioGO;
    public AudioSource sfxSource;


    public void PlaySelectGemOneShot()
    {
        sfxSource.PlayOneShot(gemSelectClip);
    }

    public void PlaySwapGemOneShot()
    {
        sfxSource.PlayOneShot(gemSwapClip);
    }

    public void PlayClearGemOneShot()
    {
        sfxSource.PlayOneShot(gemClearClip);
    }



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Error: More than one instance of Sound Manager script in action in " + this.gameObject.name);
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bgmSource = bgmAudioGO.GetComponent<AudioSource>();
        sfxSource = sfxAudioGO.GetComponent<AudioSource>();
    }
    
}
