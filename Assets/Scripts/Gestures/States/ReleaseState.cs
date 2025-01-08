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
            float throwSpeed = Vector3.Distance(character.hand_position, character.WindUp.wind_up_position) *
                character.gestureSettings.release_distance_scale / character.WindUp.duration_in_state;
            // throw card
            character.cardThrowingManager.ThrowCard((character.isRightHand ? Vector3.right : Vector3.left), 
                character.hand_position, character.hand_rotation, throwSpeed, character.TwoFinger.ChargedShot, character.isRightHand);
            // throw sfx
            AudioManager.instance.PlayVariedSFX(character.TwoFinger.ChargedShot ? AudioManager.instance.cardSFX.cardThrow_Charged : AudioManager.instance.cardSFX.cardThrow_Normal);
        }
    }
}