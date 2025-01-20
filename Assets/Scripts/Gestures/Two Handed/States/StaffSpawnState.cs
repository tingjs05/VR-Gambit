using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class StaffSpawnState : ComboGestureState<ActionController>
    {
        float handDistance = 0f;
        float prevHandDistance = 0f;
        float dot;

        public bool EnterCondition => 
            character.twoHandedGestureManager.Gestures["Hands Up"].active && 
            Vector3.Distance(character.transform.position, character.otherHand.transform.position) <= 
            character.gestureSettings.hands_together_distance;
        public bool NextStateCondition => prevHandDistance >= character.gestureSettings.min_hands_seperate_distance;
        public bool ShieldCondition => 
            (character.isRightHand && character.twoHandedGestureManager.Gestures["Hands Up"].rightHand && 
            !character.twoHandedGestureManager.Gestures["Hands Up"].leftHand) || 
            (!character.isRightHand && !character.twoHandedGestureManager.Gestures["Hands Up"].rightHand && 
            character.twoHandedGestureManager.Gestures["Hands Up"].leftHand);

        public StaffSpawnState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, character.WindUp, 
            () => character.StaffSpawn.EnterCondition, 
            () => character.StaffSpawn.NextStateCondition && character.StaffSpawn.ShieldCondition, 
            character.gestureSettings.default_transition_duration)
        {
        }

        public override void Enter()
        {
            base.Enter();
            prevHandDistance = Vector3.Distance(character.transform.position, character.otherHand.transform.position);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            handDistance = Vector3.Distance(character.transform.position, character.otherHand.transform.position);
            dot = Vector3.Dot(Vector3.up, (character.transform.position - character.otherHand.transform.position).normalized);
            
            // check if hands move closer, if so, cancel and return to default state
            if (handDistance < prevHandDistance && 
                prevHandDistance - handDistance > character.gestureSettings.min_hands_move_distance)
            {
                fsm.SwitchState(character.Default);
                return;
            }

            // ensure hands are further away, and aligns with horizontal axis
            if (handDistance <= prevHandDistance ||
                dot > (1f - character.gestureSettings.hands_to_hor_axis_threshold) || 
                dot < -(1f - character.gestureSettings.hands_to_hor_axis_threshold)) 
                    return;
            
            prevHandDistance = handDistance;
        }

        public override void Exit()
        {
            base.Exit();
        }

        protected override bool CheckTransition()
        {
            if (!character.StaffSpawn.NextStateCondition)
                return false;

            if (character.StaffSpawn.ShieldCondition)
            {
                fsm.SwitchState(character.Shield);
                return true;
            }

            return false;
        }
    }
}
