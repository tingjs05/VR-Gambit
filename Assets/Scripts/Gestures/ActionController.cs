using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class ActionController : StateMachine<ActionController>
    {
        #region States

        public IdleState Idle { get; private set; }
        public TwoFingerState TwoFinger { get; private set; }

        #endregion

        void Awake()
        {
            Idle = new IdleState(this, this);
            TwoFinger = new TwoFingerState(this, this);
            Initialize(Idle);
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
