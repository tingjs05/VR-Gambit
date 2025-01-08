using UnityEngine;
using Patterns.FSM;

namespace Target
{
    public class ChargeState : State<TargetController>
    {
        Vector3 directionToTarget;
        float timeInState;

        public ChargeState(StateMachine<TargetController> fsm, TargetController character) : base(fsm, character)
        {
        }

        public override void Enter()
        {
            timeInState = 0f;
        }

        public override void LogicUpdate()
        {
            directionToTarget = Camera.main.transform.position - character.transform.position;
            directionToTarget.y = 0f;
            directionToTarget.Normalize();

            // check if exceeded time in state
            if (timeInState >= character.maxMoveDuration)
            {
                fsm.SwitchState(character.Shoot);
                return;
            }

            // increment time in state
            timeInState += Time.deltaTime;

            // rotate towards target if not facing forward
            if (Vector3.Dot(character.transform.forward, directionToTarget) < character.rotationThreshold)
            {
                character.transform.forward = Vector3.Lerp(character.transform.forward, directionToTarget, Time.deltaTime * character.rotationScale);
                return;
            }
            
            // walk forward
            character.rb.velocity = character.transform.forward * character.movementSpeed;
        }
    }
}
