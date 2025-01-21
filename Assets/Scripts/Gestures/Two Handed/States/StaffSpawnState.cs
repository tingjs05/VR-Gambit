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
            
            // ensure hands align with horizontal axis, if not, cancel and return to default state
            if (dot > (1f - character.gestureSettings.hands_to_hor_axis_threshold) || 
                dot < -(1f - character.gestureSettings.hands_to_hor_axis_threshold))
            {
                fsm.SwitchState(character.Default);
                return;
            }

            prevHandDistance = handDistance;

            // check if required hand distance is met, only activate for right hand (it is the same)
            if (!character.isRightHand || !NextStateCondition) 
            {
                character.cardStaff.gameObject.SetActive(false);
                return;
            }
            
            // generate staff
            character.cardStaff.gameObject.SetActive(true);
            character.cardStaff.GenerateStaff(prevHandDistance);
            character.cardStaff.transform.position = (character.hand_position + character.otherHand.hand_position) / 2f;
            character.cardStaff.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
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
