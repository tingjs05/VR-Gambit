using Patterns.FSM;

namespace Gestures
{
    public class SnapState : GestureState<ActionController>
    {
        public SnapState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, () => character.gestureManager.Gestures["Snap"])
        {
        }
    }
}
