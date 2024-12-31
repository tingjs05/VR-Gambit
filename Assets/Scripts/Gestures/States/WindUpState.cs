using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class WindUpState : ComboGestureState<ActionController>
    {
        public Vector3 wind_up_position { get; private set; }
        public float duration_in_state { get; private set; }

        private Vector3 startPosFromCamera;
        private float maxDist, windUpReleaseTimer = 0f;

        public WindUpState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, character.Release, 
            () => character.gestureManager.Gestures["Wind Up"], 
            () => character.gestureManager.Gestures["Release"], 
            character.gestureSettings.wind_up_transition_duration)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // set initial wind up position
            wind_up_position = character.hand_position;
            // cache starting distance
            startPosFromCamera = character.hand_position - Camera.main.transform.position;
            // reset values
            maxDist = 0f;
            windUpReleaseTimer = 0f;
            duration_in_state = 0f;
            // show slider if charged
            if (!character.TwoFinger.ChargedShot) return;
            character.sliderObject.gameObject.SetActive(true);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // increment duration in state
            duration_in_state += Time.deltaTime;
            // do not update position if not winding up (preparing to transition to next state)
            if (transitionCoroutine != null) return;

            // only update wind up position if distance from start position is greater
            if (Vector3.Distance(character.hand_position - Camera.main.transform.position, startPosFromCamera) < maxDist)
            {
                // if take too long to transition to release state, reset wind up distance
                windUpReleaseTimer += Time.deltaTime;
                if (windUpReleaseTimer <= character.gestureSettings.wind_up_release_window) return;
            }

            // reset wind up position
            wind_up_position = character.hand_position;
            maxDist = Vector3.Distance(character.hand_position - Camera.main.transform.position, startPosFromCamera);
            windUpReleaseTimer = 0f;
            // reset duration when new position is set
            duration_in_state = 0f;
        }

        public override void Exit()
        {
            base.Exit();
            character.ToggleFingerCard(false);
            character.sliderObject.gameObject.SetActive(false);
        }
    }
}
