using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gestures
{
    public class GestureSetting : MonoBehaviour
    {
        #region Inspector Values
        [SerializeField] float defaultTransitionDuration = 1.5f;
        [SerializeField] float windUpReleaseWindow = 2f;
        [SerializeField] float releaseDistanceScale = 0.15f;
        #endregion

        #region Public Properties
        public float default_transition_duration => defaultTransitionDuration;
        public float wind_up_release_window => windUpReleaseWindow;
        public float release_distance_scale => releaseDistanceScale;
        #endregion
    }
}
