namespace Patterns.FSM
{
    public class GestureState<T> : State<T>
    {
        public delegate bool Condition();
        protected Condition currentStateCondition;
        protected State<T> defaultState;
        protected float transitionWindow;

        public GestureState(StateMachine<T> fsm, T character, 
            State<T> defaultState, Condition currentStateCondition, 
            float transitionWindow) : base(fsm, character)
        {
            this.fsm = fsm;
            this.character = character;
            this.defaultState = defaultState;
            this.currentStateCondition = currentStateCondition;
            this.transitionWindow = transitionWindow;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (currentStateCondition.Invoke()) return;
            fsm.SwitchState(defaultState);
        }
    }
}
