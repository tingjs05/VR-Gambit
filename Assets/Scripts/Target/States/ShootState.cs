using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.FSM;

namespace Target
{
    public class ShootState : State<TargetController>
    {
        float cooldownCounter;

        public ShootState(StateMachine<TargetController> fsm, TargetController character) : base(fsm, character)
        {
        }

        public override void Enter()
        {
            // shoot
            // reset cooldown
            cooldownCounter = 0f;
        }

        public override void LogicUpdate()
        {
            cooldownCounter += Time.deltaTime;
            if (cooldownCounter <= character.shootCooldown) return;

            // check if need to transition to charged state, if not restart shoot state
            if (Vector3.Distance(Camera.main.transform.position, character.transform.position) < character.minDistanceFromTarget)
            {
                Enter();
                return;
            }

            // change to charge state
            fsm.SwitchState(character.Charge);
        }
    }
}