using UnityEngine;
using Patterns.FSM;

namespace Target
{
    public class ShootState : State<TargetController>
    {
        Vector3 directionToTarget;
        float cooldownCounter;

        public ShootState(StateMachine<TargetController> fsm, TargetController character) : base(fsm, character)
        {
        }

        public override void Enter()
        {
            // shooting behaviour is called by the animation event
            // play shoot animation
            character.anim.SetTrigger("Shoot");
            // reset cooldown
            cooldownCounter = 0f;
        }

        public override void LogicUpdate()
        {
            directionToTarget = Camera.main.transform.position - character.transform.position;
            directionToTarget.y = 0f;
            directionToTarget.Normalize();

            if (Vector3.Dot(character.transform.forward, directionToTarget) < character.rotationThreshold)
            {
                character.transform.forward = Vector3.Lerp(character.transform.forward, directionToTarget, Time.deltaTime * 
                    character.rotationScale);
                character.anim.SetBool("IsWalking", true);
            }
            else
            {
                character.anim.SetBool("IsWalking", false);
            }

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

        public override void Exit()
        {
            // reset shoot trigger before exitting
            character.anim.ResetTrigger("Shoot");
        }
    }
}