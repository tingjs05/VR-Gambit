using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSFX : MonoBehaviour
{
    [Header("Idle/Static SFX")]
    public AudioClip cardIdle_HoldCard;
    public AudioClip cardIdle_ReleaseCard;
    public AudioClip cardIdle_Charging;
    public AudioClip cardIdle_FullyCharged;
    
    [Header("Throwing SFX")]
    public AudioClip cardThrow_Normal;
    public AudioClip cardThrow_Charged;

    [Header("Hover SFX")]
    public AudioClip cardHover_Place;
    public AudioClip cardHover_Launch;
}
