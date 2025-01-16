using System.Linq;
using UnityEngine;
using Patterns.FSM;

namespace Gestures
{
    public class TwoFingerState : ComboGestureState<ActionController>
    {
        // charged shot
        public bool ChargedShot => character.sliderUI.value >= 1f;

        // audo aim
        public Transform SelectedTarget { get; private set; } = null;
        Vector3 camHorForward;
        Collider[] cols;

        public TwoFingerState(StateMachine<ActionController> fsm, ActionController character) : 
            base(fsm, character, character.Default, character.WindUp, 
            () => character.gestureManager.Gestures["Two Finger"], 
            () => character.gestureManager.Gestures["Wind Up"] || character.gestureManager.Gestures["Place"], 
            character.gestureSettings.default_transition_duration)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.ToggleFingerCard(true);
            character.glow.Play();
            character.fire.Play();
            character.ToggleChargedParticles(false);
            // show UI to indicate charge
            character.sliderUI.gameObject.SetActive(true);
            character.sliderUI.value = 0f;
            // reset selected target from auto aim
            SelectedTarget = null;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // check for auto aim targets and set indicator
            character.autoAimIndicator.gameObject.SetActive(AutoAim());
            // update charging UI
            character.sliderUI.value = (character.sliderUI.maxValue * character.sliderUI.value) + Time.deltaTime;
            //AudioManager.Instance.HandleChargingVolume(ChargedShot, character.sliderUI.value / character.sliderUI.maxValue, character.isRightHand);
            if (!ChargedShot || character.chargedFire.isPlaying) return;
            character.ToggleChargedParticles(true);
        }

        public override void Exit()
        {
            base.Exit();
            
            if (character.chargedFire.isPlaying)
                character.ToggleChargedParticles(false);
            
            character.sliderUI.gameObject.SetActive(false);
        }

        protected override bool CheckTransition()
        {
            if (character.gestureManager.Gestures["Place"])
            {
                fsm.SwitchState(character.Place);
                return true;
            }
            else if (character.gestureManager.Gestures["Wind Up"])
            {
                fsm.SwitchState(nextState);
                return true;
            }
            
            return false;
        }

        bool AutoAim()
        {
            if (!character.gestureSettings.use_auto_aim) return false;
            // detect targets
            cols = Physics.OverlapSphere(Camera.main.transform.position, character.gestureSettings.detection_range, 
                character.gestureSettings.target_mask);
            // check if any targets are detected
            if (cols == null || cols.Length <= 0) return false;
            // get horizontal forward vector of camera/player
            camHorForward = GetHorizontalVector(Camera.main.transform.forward);
            // filter out targets behind the player and sort by distance
            cols = cols
                .Where(x => Vector3.Dot(camHorForward, 
                    GetHorizontalVector((x.transform.position - Camera.main.transform.position).normalized)) >= 0)
                .OrderBy(x => 
                    Vector3.Distance(x.transform.position, Camera.main.transform.position) + Mathf.Abs(Vector3.Angle(camHorForward, 
                    GetHorizontalVector((x.transform.position - Camera.main.transform.position).normalized))))
                .ToArray();
            // set selected target
            if (cols.Length <= 0) return false;
            SelectedTarget = cols[0].transform;
            // set indicator
            character.autoAimIndicator.position = SelectedTarget.position;
            return true;
        }

        Vector3 GetHorizontalVector(Vector3 vec)
        {
            vec.y = 0f;
            return vec.normalized;
        }
    }
}
