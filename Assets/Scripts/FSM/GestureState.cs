namespace Patterns.FSM
{
    public class GestureState<T> : State<T>
    {
        public delegate bool Condition();
        protected Condition currentStateCondition;
        protected State<T> defaultState;

        public GestureState(StateMachine<T> fsm, T character, 
                State<T> defaultState, Condition currentStateCondition) : 
            base(fsm, character)
        {
            this.defaultState = defaultState;
            this.currentStateCondition = currentStateCondition;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (currentStateCondition.Invoke()) return;
            fsm.SwitchState(defaultState);
        }
    }
}
