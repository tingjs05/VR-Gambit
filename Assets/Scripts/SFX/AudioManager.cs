using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxSourceRight;
    public AudioSource sfxSourceLeft;
    public CardSFX cardSFX;
    private float originalVolume;

    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            gameObject.SetActive(false);
    }

    void Start()
    {
        originalVolume = sfxSourceRight.volume;
    }

    public void PlaySFX(AudioClip sfxClip, bool isRightHand)
    {
        AudioSource sfxSource = isRightHand ? sfxSourceRight : sfxSourceLeft;
        if (sfxSource.isPlaying) sfxSource.Stop();
        sfxSource.volume = originalVolume;
        sfxSource.PlayOneShot(sfxClip);
    }

    public void PlayVariedSFX(AudioClip sfxClip, bool isRightHand)
    {
        AudioSource sfxSource = isRightHand ? sfxSourceRight : sfxSourceLeft;
        if (sfxSource.isPlaying) sfxSource.Stop();
        sfxSource.volume = Random.Range(0.95f, 1.0f);
        sfxSource.pitch = Random.Range(0.8f, 1.0f);
        sfxSource.PlayOneShot(sfxClip);
    }

    public void CutSFX(bool isRightHand)
    {
        AudioSource sfxSource = isRightHand ? sfxSourceRight : sfxSourceLeft;
        sfxSource.Stop();
    }

    public void PlayChargingSFX(bool charged, float sliderValue, bool isRightHand)
    {
        AudioSource sfxSource = isRightHand ? sfxSourceRight : sfxSourceLeft;

       if (!charged)
       {
            sfxSource.clip = cardSFX.cardIdle_Charging;
            sfxSource.Play();
            sfxSource.volume = Mathf.Clamp01(sliderValue);
            return;
        }

        if (sfxSource.clip == cardSFX.cardIdle_Charging) sfxSource.Stop();
        sfxSource.volume = originalVolume;
        sfxSource.PlayOneShot(cardSFX.cardIdle_FullyCharged);
    }
}
