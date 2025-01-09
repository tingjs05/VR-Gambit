using Patterns.FSM;

namespace Gestures
{
    public class DefaultState : State<ActionController>
    {
        public DefaultState(StateMachine<ActionController> fsm, ActionController character) : base(fsm, character)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (character.fingerCard != null) 
                character.fingerCard.gameObject.SetActive(false);
            
            if (!character.gestureManager.Gestures["Two Finger"]) return;
            fsm.SwitchState(character.TwoFinger);
        }
    }
}
