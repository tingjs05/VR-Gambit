using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class ActionController : StateMachine<ActionController>
    {
        public GestureManager gestureManager;

        [Header("Settings")]
        public float defaultTransitionDuration = 0.2f;

        #region States
        public DefaultState Default { get; private set; }
        public ReleaseState Release { get; private set; }
        public WindUpState WindUp { get; private set; }
        public TwoFingerState TwoFinger { get; private set; }
        #endregion

        void Awake()
        {
            Default = new DefaultState(this, this);
            Release = new ReleaseState(this, this);
            WindUp = new WindUpState(this, this);
            TwoFinger = new TwoFingerState(this, this);
            Initialize(Default);
        }

        void Start()
        {
            
        }

        new void Update()
        {
            base.Update();
        }
    }
}
