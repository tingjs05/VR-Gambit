using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class ShieldState : ComboGestureState<ActionController>
    {
        public bool EnterCondition => 
            (character.isRightHand && character.twoHandedGestureManager.Gestures["Hands Up"].rightHand) || 
            (!character.isRightHand && character.twoHandedGestureManager.Gestures["Hands Up"].leftHand);
        
        public ShieldState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, character.Default, 
                () => character.Shield.EnterCondition, () => !character.Shield.EnterCondition, 
                character.gestureSettings.default_transition_duration)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // ensure staff is active
            if (!character.cardStaff.gameObject.activeSelf) character.cardStaff.gameObject.SetActive(true);
            // set card staff position
            character.cardStaff.transform.position = character.hand_position;
            character.cardStaff.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        }

        public override void Exit()
        {
            base.Exit();
            // ensure staff is inactive when exitting
            character.cardStaff.gameObject.SetActive(false);
        }
    }
}
