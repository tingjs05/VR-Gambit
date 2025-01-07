using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance { get; private set; }

    public AudioSource sfxSource;

    public CardSFX cardSFX;

    private void Awake()
    {
        if (instance != null) return;

        instance = this;
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxSource.isPlaying) sfxSource.Stop();
        sfxSource.PlayOneShot(sfxClip);
    }

    public void PlayVariedSFX(AudioClip sfxClip)
    {
        if (sfxSource.isPlaying) sfxSource.Stop();
        sfxSource.volume = Random.Range(0.95f, 1.0f);
        sfxSource.pitch = Random.Range(0.8f, 1.0f);
        sfxSource.PlayOneShot(sfxClip);
    }

    public void CutSFX()
    {
        sfxSource.Stop();
    }
}
