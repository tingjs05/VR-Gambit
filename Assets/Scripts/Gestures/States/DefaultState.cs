using Patterns.FSM;

namespace Gestures
{
    public class DefaultState : State<ActionController>
    {
        public DefaultState(StateMachine<ActionController> fsm, ActionController character) : base(fsm, character)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.ToggleFingerCard(false);
            character.cardStaff.gameObject.SetActive(false);
            character.autoAimIndicator.gameObject.SetActive(false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (character.gestureManager.Gestures["Two Finger"])
            {
                fsm.SwitchState(character.TwoFinger);
                return;
            }
            
            if (!character.StaffSpawn.EnterCondition) return;
            fsm.SwitchState(character.StaffSpawn);
        }
    }
}
