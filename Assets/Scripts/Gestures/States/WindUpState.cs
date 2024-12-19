using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class WindUpState : ComboGestureState<ActionController>
    {
        public WindUpState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, character.Release, 
            () => character.gestureManager.Gestures["Wind Up"], 
            () => character.gestureManager.Gestures["Release"], 
            character.defaultTransitionDuration)
        {
        }
    }
}
