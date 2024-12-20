using Patterns.FSM;

namespace Gestures
{
    public class ReleaseState : GestureState<ActionController>
    {
        public ReleaseState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, () => character.gestureManager.Gestures["Release"], 
            character.gestureSettings.release_transition_duration)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.cardThrowingManager.ThrowCard(character.isRightHand);
        }
    }
}