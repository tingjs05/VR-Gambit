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

        [Header("Card Placement Settings")]
        [SerializeField] Vector3 cardPlacementOffset;

        [Header("Finger Card")]
        [SerializeField] Vector3 placeCardTilt = new Vector3(-80f, 0f, 0f);
        [SerializeField] Quaternion cardRotationOffset = Quaternion.Euler(82f, 0f, 0f);
        [SerializeField] float offsetFloat = 0.0325f;

        // [Header("Card Charging")]
        #endregion

        #region Public Properties
        // transition durations
        public float default_transition_duration => defaultTransitionDuration;
        public float place_transition_duration => placeTransitionDuration;
        public float wind_up_transition_duration => windUpTransitionDuration;

        // basic card throw settings
        public float wind_up_release_window => windUpReleaseWindow;
        public float release_distance_scale => releaseDistanceScale;

        // card placement settings
        public Vector3 card_placement_offset => cardPlacementOffset;

        // finger card
        public Vector3 place_card_tilt => placeCardTilt;
        public Quaternion card_rotation_offset => cardRotationOffset;
        public float offset_float => offsetFloat;
        #endregion
    }
}
