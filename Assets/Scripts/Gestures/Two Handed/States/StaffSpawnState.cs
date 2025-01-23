using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class StaffSpawnState : ComboGestureState<ActionController>
    {
        float handDistace = 0f;
        float timeInState = 0f;

        public bool EnterCondition => character.twoHandedGestureManager.Gestures["Hands Up"].active && IsLinedUp && HandInPlace;

        bool IsLinedUp => 
            Vector3.Dot(Vector3.up, (character.transform.position - character.otherHand.transform.position).normalized) <= 
            (1f - character.gestureSettings.hands_to_hor_axis_threshold) && 
            Vector3.Dot(Vector3.up, (character.transform.position - character.otherHand.transform.position).normalized) >= 
            -(1f - character.gestureSettings.hands_to_hor_axis_threshold);
        
        bool HandInPlace =>
            (character.isRightHand && Vector3.Dot((character.transform.position - Camera.main.transform.position).normalized, 
            Camera.main.transform.right) >= 0f) || 
            (!character.isRightHand && Vector3.Dot((character.transform.position - Camera.main.transform.position).normalized, 
            -Camera.main.transform.right) >= 0f);

        public StaffSpawnState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, character.Shield, 
            () => character.StaffSpawn.EnterCondition, 
            () => !character.StaffSpawn.EnterCondition && character.Shield.EnterCondition, 
            character.gestureSettings.card_staff_transition_duration)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // play sfx for bringing out staff
            AudioManager.Instance.PlaySFX(AudioManager.Instance.cardSFX.cardIdle_HoldCard, character.isRightHand);
            // reset staff
            character.cardStaff.gameObject.SetActive(true);
            // reset hand distance and time in state
            handDistace = 0f;
            timeInState = 0f;
            // ensure both hands are in staff spawn state
            if (character.otherHand.currentState == character.otherHand.StaffSpawn) return;
            character.otherHand.SwitchState(character.otherHand.StaffSpawn);
        }

        public override void LogicUpdate()
        {
            handDistace = Vector3.Distance(character.transform.position, character.otherHand.transform.position);
            timeInState += Time.deltaTime;
            
            base.LogicUpdate();

            // only activate for right hand (it is the same)
            if (!character.isRightHand) return;
            
            // generate staff
            character.cardStaff.GenerateStaff(handDistace);
            character.cardStaff.transform.position = (character.transform.position + character.otherHand.transform.position) / 2f;
            character.cardStaff.transform.rotation = Quaternion.LookRotation(
                Vector3.Cross((character.transform.position - character.otherHand.transform.position).normalized, 
                Camera.main.transform.up), Camera.main.transform.up);
        }

        protected override bool CheckTransition()
        {
            // unless longer than transition duration, cannot transition to next state
            if (timeInState < character.gestureSettings.card_staff_transition_duration)
                return false;

            if (character.Shield.EnterCondition)
            {
                fsm.SwitchState(character.Shield);
                return true;
            }

            return false;
        }
    }
}
