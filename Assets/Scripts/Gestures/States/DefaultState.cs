using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class DefaultState : State<ActionController>
    {
        public DefaultState(StateMachine<ActionController> fsm, ActionController character) : base(fsm, character)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.ToggleFingerCard(false);
            character.autoAimIndicator.gameObject.SetActive(false);
            // do not deactivate staff if other hand is using it
            if (character.otherHand.currentState == character.otherHand.Shield) return;
            character.cardStaff.gameObject.SetActive(false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (character.gestureManager.Gestures["Two Finger"])
            {
                fsm.SwitchState(character.TwoFinger);
                return;
            }
            
            if (character.otherHand.currentState != character.otherHand.Default || !character.StaffSpawn.EnterCondition || 
                Vector3.Distance(character.transform.position, character.otherHand.transform.position) > 
                character.gestureSettings.hands_together_distance)
                    return;
            
            fsm.SwitchState(character.StaffSpawn);
        }
    }
}
