using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class ShieldState : ComboGestureState<ActionController>
    {
        float angle = 0f;

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
            // activate shield hitbox
            character.cardStaff.ActivateShield();
            // reset angle
            angle = 0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // increment angle
            angle += character.gestureSettings.shield_rotation_speed * Time.deltaTime;
            // check if need to reset angle
            if (angle >= 360) angle = 0f + (angle - 360f);
            // set card staff position
            character.cardStaff.transform.position = character.transform.position;
            character.cardStaff.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, 
                Quaternion.AngleAxis(angle * (character.isRightHand ? 1f : -1f), Camera.main.transform.forward) * 
                Camera.main.transform.up);
        }
    }
}
