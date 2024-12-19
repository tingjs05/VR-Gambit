using System.Collections;
using UnityEngine;

namespace Patterns.FSM
{
    public class GestureState<T> : State<T>
    {
        public delegate bool Condition();
        protected Condition currentStateCondition, nextStateCondition;

        protected State<T> defaultState, nextState;
        protected float transitionWindow;

        Coroutine transitionCoroutine;

        public GestureState(StateMachine<T> fsm, T character, 
            State<T> defaultState, State<T> nextState, 
            Condition currentStateCondition, Condition nextStateCondition, 
            float transitionWindow) : base(fsm, character)
        {
            this.fsm = fsm;
            this.character = character;
            this.defaultState = defaultState;
            this.nextState = nextState;
            this.currentStateCondition = currentStateCondition;
            this.nextStateCondition = nextStateCondition;
            this.transitionWindow = transitionWindow;
        }

        public override void Enter()
        {
            base.Enter();

            if (!currentStateCondition.Invoke()) return;
            fsm.SwitchState(defaultState);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (currentStateCondition.Invoke())
            {
                CheckTransitionCoroutine();
                return;
            }

            if (transitionCoroutine != null) return;
            transitionCoroutine = fsm.StartCoroutine(Transition());
        }

        public override void Exit()
        {
            base.Exit();
            CheckTransitionCoroutine();
        }

        void CheckTransitionCoroutine()
        {
            if (transitionCoroutine == null) return;
            fsm.StopCoroutine(transitionCoroutine);
            transitionCoroutine = null;
        }

        IEnumerator Transition()
        {
            float timeElasped = 0f;

            while (timeElasped < transitionWindow)
            {
                if (nextStateCondition.Invoke())
                {
                    fsm.SwitchState(nextState);
                    break;
                }

                timeElasped += Time.deltaTime;
                yield return null;
            }

            fsm.SwitchState(defaultState);
            transitionCoroutine = null;
        }
    }
}
