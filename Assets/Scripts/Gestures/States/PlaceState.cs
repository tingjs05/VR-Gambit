using Patterns.FSM;

namespace Gestures
{
    public class PlaceState : ComboGestureState<ActionController>
    {
        public PlaceState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, character.Snap, 
            () => character.gestureManager.Gestures["Place"], 
            () => character.gestureManager.Gestures["Snap"], 
            character.gestureSettings.default_transition_duration)
        {
        }

        public override void Exit()
        {
            base.Exit();
            character.SetFingerCard(false);
        }
    }
}
