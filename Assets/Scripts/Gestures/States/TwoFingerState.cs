using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class TwoFingerState : ComboGestureState<ActionController>
    {
        public TwoFingerState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, character.WindUp, 
            () => character.gestureManager.Gestures["Two Finger"], 
            () => character.gestureManager.Gestures["Wind Up"], 
            character.gestureSettings.default_transition_duration)
        {
        }
    }
}
