using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class ShieldState : GestureState<ActionController>
    {
        public ShieldState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, () => character.gestureManager.Gestures["Release"])
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
    }
}
