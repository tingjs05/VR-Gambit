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

            float currDot, selectedDot;

            // filter out targets behind the player, then sort by distance and angle to direction player is pointing
            for (int i = 0; i < cols.Length; i++)
            {
                if (Vector3.Dot(GetHorizontalVector(Camera.main.transform.forward), 
                    GetHorizontalVector((cols[i].transform.position - Camera.main.transform.position).normalized)) < 0)
                        continue;
                
                // if selected target is null, set current collider as selected target
                if (SelectedTarget == null)
                {
                    SelectedTarget = cols[i].transform;
                    continue;
                }

                // if current target is outside max angle, do not check
                if (Mathf.Abs(Vector3.Angle(
                    GetHorizontalVector((character.hand_position - Camera.main.transform.forward).normalized), 
                    GetHorizontalVector((cols[i].transform.position - Camera.main.transform.position).normalized))) < 
                    character.gestureSettings.max_angle)
                        continue;

                // calculate dot of direction of target to direction to hand
                currDot = Vector3.Dot(GetHorizontalVector((character.hand_position - Camera.main.transform.position).normalized), 
                    GetHorizontalVector((cols[i].transform.position - Camera.main.transform.position).normalized));
                selectedDot = Vector3.Dot(GetHorizontalVector((character.hand_position - Camera.main.transform.position).normalized), 
                    GetHorizontalVector((SelectedTarget.position - Camera.main.transform.position).normalized));

                // if both dots are the same, check which is nearer
                if (currDot == selectedDot && (
                    Vector3.Distance(cols[i].transform.position, Camera.main.transform.position) >= 
                    Vector3.Distance(SelectedTarget.position, Camera.main.transform.position)))
                        continue;

                // do not replace selected target if current one has a wider angle
                if (currDot < selectedDot) continue;
                // replace selected target
                SelectedTarget = cols[i].transform;
            }

            return SelectedTarget != null;
        }

        Vector3 GetHorizontalVector(Vector3 vec)
        {
            vec.y = 0f;
            return vec.normalized;
        }
    }
}
