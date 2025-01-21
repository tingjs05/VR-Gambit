using Patterns.FSM;

namespace Gestures
{
    public class ShieldState : GestureState<ActionController>
    {
        public ShieldState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, () => 
                (character.isRightHand && character.twoHandedGestureManager.Gestures["Hands Up"].rightHand) || 
                (!character.isRightHand && character.twoHandedGestureManager.Gestures["Hands Up"].leftHand))
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
    }
}
