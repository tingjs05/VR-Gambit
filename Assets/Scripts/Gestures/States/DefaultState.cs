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

        public override void Enter()
        {
            base.Enter();
            character.fingerCard.gameObject.SetActive(false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!character.gestureManager.Gestures["Two Finger"]) return;
            fsm.SwitchState(character.TwoFinger);
        }
    }
}
