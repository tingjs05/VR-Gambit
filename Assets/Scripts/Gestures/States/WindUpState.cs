using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class WindUpState : ComboGestureState<ActionController>
    {
        public Vector3 wind_up_position { get; private set; }
        public float duration_in_wind_up { get; private set; }

        public WindUpState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, character.Release, 
            () => character.gestureManager.Gestures["Wind Up"], 
            () => character.gestureManager.Gestures["Release"], 
            character.gestureSettings.default_transition_duration)
        {
        }

        public override void Enter()
        {
            base.Enter();
            duration_in_wind_up = 0f;
            // wind_up_position = character
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            duration_in_wind_up += Time.deltaTime;
        }
    }
}
