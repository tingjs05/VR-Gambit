using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class ShieldState : GestureState<ActionController>
    {
        public bool EnterCondition => 
            (character.isRightHand && character.twoHandedGestureManager.Gestures["Hands Up"].rightHand) || 
            (!character.isRightHand && character.twoHandedGestureManager.Gestures["Hands Up"].leftHand);
        
        public ShieldState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, () => character.Shield.EnterCondition)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            character.cardStaff.transform.position = character.hand_position;
            character.cardStaff.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        }
    }
}
