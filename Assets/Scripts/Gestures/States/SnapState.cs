using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class SnapState : GestureState<ActionController>
    {
        public SnapState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, () => character.gestureManager.Gestures["Snap"])
        {
        }

        public override void Enter()
        {
            base.Enter();
            // calculate card offset
            Vector3 cardOffset = 
                (Camera.main.transform.forward * character.gestureSettings.card_placement_offset.x) + 
                (Camera.main.transform.right * character.gestureSettings.card_placement_offset.z) + 
                (Camera.main.transform.up * character.gestureSettings.card_placement_offset.y);
            // hover card
            character.cardPlacementManager.PlaceCard(character.hand_position, cardOffset, 
                character.hand_rotation, character.TwoFinger.SelectedTarget);
            // play place card sound effect
            AudioManager.Instance.PlaySFX(AudioManager.Instance.cardSFX.cardHover_Place, character.isRightHand);
        }
    }
}
