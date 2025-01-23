using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class StaffSpawnState : ComboGestureState<ActionController>
    {
        float handDistace = 0f;

        public bool EnterCondition => character.twoHandedGestureManager.Gestures["Hands Up"].active && IsLinedUp && HandInPlace;
        public bool NextStateCondition => handDistace >= character.gestureSettings.min_hands_seperate_distance;

        bool IsLinedUp => 
            Vector3.Dot(Vector3.up, (character.hand_position - character.otherHand.hand_position).normalized) <= 
            (1f - character.gestureSettings.hands_to_hor_axis_threshold) && 
            Vector3.Dot(Vector3.up, (character.hand_position - character.otherHand.hand_position).normalized) >= 
            -(1f - character.gestureSettings.hands_to_hor_axis_threshold);
        
        bool HandInPlace =>
            (character.isRightHand && Vector3.Dot((character.hand_position - Camera.main.transform.position).normalized, 
            Camera.main.transform.right) >= 0f) || 
            (!character.isRightHand && Vector3.Dot((character.hand_position - Camera.main.transform.position).normalized, 
            -Camera.main.transform.right) >= 0f);

        public StaffSpawnState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, character.Shield, 
            () => character.StaffSpawn.EnterCondition, 
            () => !character.StaffSpawn.EnterCondition && character.StaffSpawn.NextStateCondition && 
                character.Shield.EnterCondition, 
            character.gestureSettings.card_staff_transition_duration)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // play sfx for bringing out staff
            AudioManager.Instance.PlaySFX(AudioManager.Instance.cardSFX.cardIdle_HoldCard, character.isRightHand);
            // reset staff
            character.cardStaff.gameObject.SetActive(false);
            character.cardStaff.GenerateStaff(0f);
            character.cardStaff.transform.position = (character.hand_position + character.otherHand.hand_position) / 2f;
            character.cardStaff.transform.rotation = Quaternion.LookRotation(
                Vector3.Cross((character.hand_position - character.otherHand.hand_position).normalized, Camera.main.transform.up), 
                Camera.main.transform.up);
        }

        public override void LogicUpdate()
        {
            handDistace = Vector3.Distance(character.hand_position, character.otherHand.hand_position);

            base.LogicUpdate();

            // check if required hand distance is met, if not, dont show card staff
            if (!NextStateCondition) 
            {
                character.cardStaff.gameObject.SetActive(false);
                return;
            }

            // only activate for right hand (it is the same)
            if (!character.isRightHand) return;
            
            // generate staff
            character.cardStaff.gameObject.SetActive(true);
            character.cardStaff.GenerateStaff(handDistace);
            character.cardStaff.transform.position = (character.hand_position + character.otherHand.hand_position) / 2f;
            character.cardStaff.transform.rotation = Quaternion.LookRotation(
                Vector3.Cross((character.hand_position - character.otherHand.hand_position).normalized, Camera.main.transform.up), 
                Camera.main.transform.up);
        }

        protected override bool CheckTransition()
        {
            if (!character.StaffSpawn.NextStateCondition)
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
