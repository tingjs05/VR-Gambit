using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class ShieldState : GestureState<ActionController>
    {
        public ShieldState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, () => 
                (character.isRightHand && character.twoHandedGestureManager.Gestures["Hands Up"].rightHand) || 
                (!character.isRightHand && character.twoHandedGestureManager.Gestures["Hands Up"].leftHand))
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            character.cardStaff.transform.position = (character.hand_position + character.otherHand.hand_position) / 2f;
            character.cardStaff.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        }
    }
}
