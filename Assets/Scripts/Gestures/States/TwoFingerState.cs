using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class TwoFingerState : State<ActionController>
    {
        public TwoFingerState(StateMachine<ActionController> fsm, ActionController character) : base(fsm, character)
        {
            this.fsm = fsm;
            this.character = character;
        }
    }
}
