using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class TwoFingerState : ComboGestureState<ActionController>
    {
        public bool ChargedShot => character.sliderUI.value >= 1f;

        public TwoFingerState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, character.WindUp, 
            () => character.gestureManager.Gestures["Two Finger"], 
            () => character.gestureManager.Gestures["Wind Up"] || character.gestureManager.Gestures["Place"], 
            character.gestureSettings.default_transition_duration)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.ToggleFingerCard(true);
            character.glow.Play();
            character.fire.Play();
            character.ToggleChargedParticles(false);
            // show UI to indicate charge
            character.sliderUI.gameObject.SetActive(true);
            character.sliderUI.value = 0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            character.sliderUI.value = (character.sliderUI.maxValue * character.sliderUI.value) + Time.deltaTime;
            AudioManager.Instance.PlayChargingSFX(ChargedShot, character.sliderUI.value / character.sliderUI.maxValue, character.isRightHand);
            if (!ChargedShot || character.chargedFire.isPlaying) return;
            character.ToggleChargedParticles(true);
        }

        public override void Exit()
        {
            base.Exit();
            
            if (character.chargedFire.isPlaying)
                character.ToggleChargedParticles(false);
            
            character.sliderUI.gameObject.SetActive(false);
        }

        protected override bool CheckTransition()
        {
            if (character.gestureManager.Gestures["Place"])
            {
                fsm.SwitchState(character.Place);
                return true;
            }
            else if (character.gestureManager.Gestures["Wind Up"])
            {
                fsm.SwitchState(nextState);
                return true;
            }
            
            return false;
        }
    }
}
