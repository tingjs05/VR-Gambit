using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gestures
{
    public class GestureSetting : MonoBehaviour
    {
        #region Inspector Values
        [SerializeField] float defaultTransitionDuration = 1.5f;
        #endregion

        #region Public Properties
        public float default_transition_duration => defaultTransitionDuration;
        #endregion
    }
}
