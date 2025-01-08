using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance { get; private set; }

    public AudioSource sfxSource;

    public CardSFX cardSFX;

    private float originalVolume;

    private void Awake()
    {
        if (instance != null) return;

        instance = this;

        originalVolume = this.sfxSource.volume;
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

    public void PlayChargingSFX(float sliderValue)
    {
        sfxSource.PlayOneShot(cardSFX.cardIdle_Charging);
        if (sfxSource.isPlaying) sfxSource.volume = Mathf.Clamp01(sliderValue);
        else sfxSource.volume = originalVolume;


    }
}
