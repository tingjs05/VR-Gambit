using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gestures
{
    public class GestureSetting : MonoBehaviour
    {
        #region Inspector Values
        [SerializeField] float defaultTransitionDuration = 0.25f;
        [SerializeField] float placeTransitionDuration = 0.5f;
        [SerializeField] float windUpTransitionDuration = 0.5f;
        [SerializeField] float windUpReleaseWindow = 2f;
        [SerializeField] float releaseDistanceScale = 0.15f;
        #endregion

        #region Public Properties
        public float default_transition_duration => defaultTransitionDuration;
        public float place_transition_duration => placeTransitionDuration;
        public float wind_up_transition_duration => windUpTransitionDuration;
        public float wind_up_release_window => windUpReleaseWindow;
        public float release_distance_scale => releaseDistanceScale;
        #endregion
    }
}
