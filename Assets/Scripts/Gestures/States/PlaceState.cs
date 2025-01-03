using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class PlaceState : ComboGestureState<ActionController>
    {
        public PlaceState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, character.Snap, 
            () => character.gestureManager.Gestures["Place"], 
            () => character.gestureManager.Gestures["Snap"], 
            character.gestureSettings.place_transition_duration)
        {
        }

        public override void Enter()
        {
            base.Enter();
            RotateCard(character.gestureSettings.place_card_tilt);
            character.rotateCardToFinger = false;
            // reset charged card (can't charge card to place)
            character.sliderUI.gameObject.SetActive(false);
        }

        public override void Exit()
        {
            base.Exit();
            RotateCard(-character.gestureSettings.place_card_tilt);
            character.ToggleFingerCard(false);
            character.rotateCardToFinger = true;
        }

        void RotateCard(Vector3 eulerRotation)
        {
            foreach (Transform child in character.fingerCard)
            {
                child.Rotate(eulerRotation.x, eulerRotation.y, eulerRotation.z);
            }
        }
    }
}
