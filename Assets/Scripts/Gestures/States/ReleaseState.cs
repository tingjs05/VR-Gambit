using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class ReleaseState : GestureState<ActionController>
    {
        public ReleaseState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, () => character.gestureManager.Gestures["Release"], 
            character.gestureSettings.default_transition_duration)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
    }
}