using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
using Patterns.FSM;
using Card;
using UI;

namespace Gestures
{
    public class ActionController : StateMachine<ActionController>
    {
        public GestureManager gestureManager;
        public GestureSetting gestureSettings;
        public bool isRightHand = true;

        [Header("Finger Card")]
        public Transform fingerCard;
        public ParticleSystem glow, fire, chargedFire, lightning;
        public BoxSlider sliderUI;

        [Header("Action Managers")]
        public CardThrowing cardThrowingManager;
        public CardPlacement cardPlacementManager;

        #region States
        public DefaultState Default { get; private set; }
        public ReleaseState Release { get; private set; }
        public WindUpState WindUp { get; private set; }
        public TwoFingerState TwoFinger { get; private set; }
        public SnapState Snap { get; private set; }
        public PlaceState Place { get; private set; }
        #endregion

        #region Hand Management
        private InputDevice handDevice;
        private XRHandSubsystem handSubsystem;
        private XRHand hand;
        private XRHandJoint indexTip, indexIntermediate, middleTip;

        private Vector3 handPosition;
        public Vector3 hand_position => handPosition;

        private Quaternion handRotation;
        public Quaternion hand_rotation => handRotation;
        #endregion

        #region Finger Card
        private Vector3 indexDir, antiClipOffset;
        public bool rotateCardToFinger = true;
        #endregion

        void Awake()
        {
            Default = new DefaultState(this, this);
            Release = new ReleaseState(this, this);
            WindUp = new WindUpState(this, this);
            Snap = new SnapState(this, this);
            Place = new PlaceState(this, this);
            TwoFinger = new TwoFingerState(this, this);
            Initialize(Default);
        }

        void Start()
        {
            // reset card charging
            sliderUI.maxValue = gestureSettings.card_charge_duration;
            ToggleChargedParticles(false);

            // Get the InputDevice for the specified hand
            handDevice = InputDevices.GetDeviceAtXRNode(isRightHand ? XRNode.RightHand : XRNode.LeftHand);
            // check if hand device is valid
            if (!handDevice.isValid)
                Debug.LogWarning((isRightHand ? "Right" : "Left") + " hand device is not valid!");

            // get hand subsystem
            handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
            // get hand
            if (handSubsystem == null) return;
            hand = isRightHand ? handSubsystem.rightHand : handSubsystem.leftHand;
        }

        new void Update()
        {
            base.Update();

            // check if hand is found, and disable finger card if hand is not detected
            if (!handDevice.isValid)
            {
                if (fingerCard != null) 
                    fingerCard.gameObject.SetActive(false);
                
                return;
            }

            // Update hand position and rotation
            handDevice.TryGetFeatureValue(CommonUsages.devicePosition, out handPosition);
            handDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out handRotation);

            transform.position = handPosition;
            transform.rotation = handRotation;

            // move objects according to hand position and rotation
            MoveFingerCard();
        }

        void MoveFingerCard()
        {
            // set finger tip joint
            indexTip = hand.GetJoint(XRHandJointID.IndexTip);
            indexIntermediate = hand.GetJoint(XRHandJointID.IndexIntermediate);
            middleTip = hand.GetJoint(XRHandJointID.MiddleTip);

            // set finger card
            if (fingerCard == null || !fingerCard.gameObject.activeInHierarchy || 
                handSubsystem == null || indexTip == null || middleTip == null || 
                !indexTip.TryGetPose(out Pose indexTipPose) || !middleTip.TryGetPose(out Pose middleTipPose) || 
                !indexIntermediate.TryGetPose(out Pose indexIntermediatePose))
                    return;

            // directional vectors
            indexDir = (indexTipPose.position - indexIntermediatePose.position).normalized;
            antiClipOffset = (isRightHand ? (handRotation * Vector3.left) : (handRotation * Vector3.right)) * 
                (gestureSettings.offset_float * Mathf.Clamp01(1f - Vector3.Angle(indexDir, (handRotation * Vector3.down).normalized) / 90f));
            
            // rotate card based on index finger direction
            fingerCard.SetPositionAndRotation(
                (indexTipPose.position + middleTipPose.position) / 2 + 
                (rotateCardToFinger ? antiClipOffset : Vector3.zero), 
                rotateCardToFinger ? Quaternion.LookRotation(indexDir, handRotation * Vector3.forward) : 
                (handRotation * gestureSettings.card_rotation_offset));
        }

        public void ToggleChargedParticles(bool play)
        {
            if (play)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.cardSFX.cardIdle_FullyCharged, isRightHand);
                if (AudioManager.Instance.GetAudioSource(isRightHand).isPlaying) AudioManager.Instance.GetAudioSource(isRightHand).Stop();
                chargedFire.Play();
                lightning.Play();
                return;
            }

            chargedFire.Stop();
            lightning.Stop();
        }

        public void ToggleFingerCard(bool active, bool overrideReleaseSFX = false)
        {
            if (fingerCard == null) return;
            fingerCard.gameObject.SetActive(active);
            if (AudioManager.Instance == null) return;
            if (active) 
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.cardSFX.cardIdle_HoldCard, isRightHand);
                return;
            };

            if (!overrideReleaseSFX) AudioManager.Instance.PlaySFX(AudioManager.Instance.cardSFX.cardIdle_ReleaseCard, isRightHand);
 
        }
    }
}
