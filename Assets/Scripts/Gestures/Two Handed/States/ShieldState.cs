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

        public override void Enter()
        {
            base.Enter();
            character.cardStaff.ActivateShield();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // set card staff position
            character.cardStaff.transform.position = character.transform.position;
            character.cardStaff.transform.rotation = Quaternion.LookRotation(
                // Quaternion.AngleAxis(character.gestureSettings.shield_rotation_speed * Time.deltaTime * 
                // (character.isRightHand ? 1f : -1f), Camera.main.transform.up) * 
                Camera.main.transform.forward, Camera.main.transform.up);
        }
    }
}
