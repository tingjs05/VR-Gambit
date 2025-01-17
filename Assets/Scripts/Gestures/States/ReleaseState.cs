using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class ReleaseState : GestureState<ActionController>
    {
        public ReleaseState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, () => character.gestureManager.Gestures["Release"])
        {
        }

        public override void Enter()
        {
            base.Enter();
            // calculate throw speed
            float throwSpeed = (Vector3.Distance(character.hand_position, character.WindUp.wind_up_position) + 
                // add distance of selected target (if available) to distance travelled
                (character.TwoFinger.SelectedTarget == null ? 0f : (character.gestureSettings.selected_target_distance_scale * 
                Vector3.Distance(character.hand_position, character.TwoFinger.SelectedTarget.position)))) *
                // divide by duration in state to find speed
                character.gestureSettings.release_distance_scale / character.WindUp.duration_in_state;
            // throw card
            character.cardThrowingManager.ThrowCard(
                // check if there is a selected target to aim towards
                character.TwoFinger.SelectedTarget != null ? 
                // if so, shoot card towards aimed target
                (character.TwoFinger.SelectedTarget.position - character.transform.position).normalized : 
                // otherwise take direction depending on hand
                ((character.isRightHand ? character.transform.right : -character.transform.right) - character.transform.up).normalized, 
                // pass in other values to instsantiate and launch card
                character.hand_position, character.hand_rotation, throwSpeed, character.TwoFinger.ChargedShot, character.isRightHand);
            // throw sfx
            AudioManager.Instance.PlayVariedSFX(character.TwoFinger.ChargedShot ? 
                AudioManager.Instance.cardSFX.cardThrow_Charged : AudioManager.Instance.cardSFX.cardThrow_Normal, 
                character.isRightHand);
        }
    }
}