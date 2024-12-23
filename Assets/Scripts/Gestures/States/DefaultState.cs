using Patterns.FSM;

namespace Gestures
{
    public class DefaultState : State<ActionController>
    {
        public DefaultState(StateMachine<ActionController> fsm, ActionController character) : base(fsm, character)
        {
            this.fsm = fsm;
            this.character = character;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (character.gestureManager.Gestures["Two Finger"])
            {
                fsm.SwitchState(character.TwoFinger);
                return;
            }

            // if (character.gestureManager.Gestures["Place"])
            // {
            //     fsm.SwitchState(character.Place);
            //     return;
            // }
        }
    }
}
