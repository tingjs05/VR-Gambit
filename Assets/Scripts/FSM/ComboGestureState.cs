using System.Collections;
using UnityEngine;

namespace Patterns.FSM
{
    public class ComboGestureState<T> : GestureState<T>
    {
        protected Condition nextStateCondition;
        protected State<T> nextState;
        protected Coroutine transitionCoroutine;
        protected float transitionWindow, transitionDuration;

        public ComboGestureState(StateMachine<T> fsm, T character, 
                State<T> defaultState, State<T> nextState, 
                Condition currentStateCondition, Condition nextStateCondition, 
                float transitionWindow) : 
            base(fsm, character, defaultState, 
                currentStateCondition)
        {
            this.nextState = nextState;
            this.nextStateCondition = nextStateCondition;
            this.transitionWindow = transitionWindow;
        }

        public override void Enter()
        {
            base.Enter();
            transitionDuration = 0f;
            if (currentStateCondition.Invoke()) return;
            fsm.SwitchState(defaultState);
        }

        public override void LogicUpdate()
        {
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

        protected virtual bool CheckTransition()
        {
            if (!nextStateCondition.Invoke()) return false;
            fsm.SwitchState(nextState);
            return true;
        }

        void CheckTransitionCoroutine()
        {
            if (transitionCoroutine == null) return;
            fsm.StopCoroutine(transitionCoroutine);
            transitionCoroutine = null;
        }

        IEnumerator Transition()
        {
            transitionDuration = 0f;

            while (transitionDuration < transitionWindow)
            {
                transitionDuration += Time.deltaTime;
                
                if (CheckTransition())
                {
                    transitionCoroutine = null;
                    break;
                }

                yield return null;
            }

            // do not reset if coroutine was ended prematurely (transition occured)
            if (transitionCoroutine != null)
            {
                fsm.SwitchState(defaultState);
                transitionCoroutine = null;
            }
        }
    }
}
