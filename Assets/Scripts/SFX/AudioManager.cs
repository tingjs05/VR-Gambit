using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioSource sfxSource;

    private void Awake()
    {
        if (instance != null) return;

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip);
    }



}
