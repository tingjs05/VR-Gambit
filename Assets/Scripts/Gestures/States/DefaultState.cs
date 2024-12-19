using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }
}
