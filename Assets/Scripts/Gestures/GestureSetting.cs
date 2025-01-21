using UnityEngine;

namespace Gestures
{
    public class GestureSetting : MonoBehaviour
    {
        #region Inspector Values
        [Header("Transition Durations")]
        [SerializeField] float defaultTransitionDuration = 0.25f;
        [SerializeField] float placeTransitionDuration = 0.5f;
        [SerializeField] float windUpTransitionDuration = 0.5f;

        [Header("Basic Card Throw Settings")]
        [SerializeField] float windUpReleaseWindow = 2f;
        [SerializeField] float releaseDistanceScale = 0.15f;
        [SerializeField] float selectedTargetDistanceScale = 0.01f;

        [Header("Card Placement Settings")]
        [SerializeField] Vector3 cardPlacementOffset;

        [Header("Card Charging")]
        [SerializeField] float cardChargeDuration = 5f;

        [Header("Staff Spawn")]
        [SerializeField] float handsTogetherDistance = 0.05f;
        [SerializeField] float minHandsSeperateDistance = 0.1f;
        [SerializeField] float handsToHorAxisThreshold = 0.8f;

        [Header("Finger Card")]
        [SerializeField] Vector3 placeCardTilt = new Vector3(-80f, 0f, 0f);
        [SerializeField] Quaternion cardRotationOffset = Quaternion.Euler(82f, 0f, 0f);
        [SerializeField] float offsetFloat = 0.0325f;

        [Header("Auto Aim")]
        [SerializeField] private float detectionRange = 15f;
        [SerializeField] private float maxAngle = 75f;
        [SerializeField] private bool useAutoAim = true;
        [SerializeField] private LayerMask targetMask;
        #endregion

        #region Public Properties
        // transition durations
        public float default_transition_duration => defaultTransitionDuration;
        public float place_transition_duration => placeTransitionDuration;
        public float wind_up_transition_duration => windUpTransitionDuration;

        // basic card throw settings
        public float wind_up_release_window => windUpReleaseWindow;
        public float release_distance_scale => releaseDistanceScale;
        public float selected_target_distance_scale => selectedTargetDistanceScale;

        // card placement settings
        public Vector3 card_placement_offset => cardPlacementOffset;

        // card charging
        public float card_charge_duration => cardChargeDuration;

        // staff spawn
        public float hands_together_distance => handsTogetherDistance;
        public float min_hands_seperate_distance => minHandsSeperateDistance;
        public float hands_to_hor_axis_threshold => handsToHorAxisThreshold;

        // finger card
        public Vector3 place_card_tilt => placeCardTilt;
        public Quaternion card_rotation_offset => cardRotationOffset;
        public float offset_float => offsetFloat;

        // auto aim
        public float detection_range => detectionRange;
        public float max_angle => maxAngle;
        public bool use_auto_aim => useAutoAim;
        public LayerMask target_mask => targetMask;
        #endregion
    }
}
