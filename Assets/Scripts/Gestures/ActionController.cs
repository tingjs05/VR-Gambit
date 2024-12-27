using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
using TMPro;
using Patterns.FSM;
using Card;

namespace Gestures
{
    public class ActionController : StateMachine<ActionController>
    {
        public GestureManager gestureManager;
        public GestureSetting gestureSettings;
        public TextMeshProUGUI tempText;
        public bool isRightHand = true;

        [Header("Finger Card")]
        public Transform fingerCard;
        public Vector3 placeCardTilt = new Vector3(-80f, 0f, 0f);
        public Quaternion cardRotationOffset = Quaternion.Euler(82f, 0f, 0f);
        public ParticleSystem glow, fire, charargedFire;

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
        private XRHandJoint indexTip, middleTip;

        private Vector3 handPosition;
        public Vector3 hand_position => handPosition;

        private Quaternion handRotation;
        public Quaternion hand_rotation => handRotation;
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
            tempText.text = current_state_name;

            // Update hand position and rotation
            if (!handDevice.isValid) return;
            handDevice.TryGetFeatureValue(CommonUsages.devicePosition, out handPosition);
            handDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out handRotation);

            transform.position = handPosition;
            transform.rotation = handRotation;

            // set finger tip joint
            indexTip = hand.GetJoint(XRHandJointID.IndexTip);
            middleTip = hand.GetJoint(XRHandJointID.MiddleTip);

            // set finger card
            if (fingerCard == null || !fingerCard.gameObject.activeInHierarchy || 
                handSubsystem == null || indexTip == null || middleTip == null || 
                !indexTip.TryGetPose(out Pose indexTipPose) || !middleTip.TryGetPose(out Pose middleTipPose)) 
                    return;

            fingerCard.SetPositionAndRotation((indexTipPose.position + middleTipPose.position) / 2, 
                handRotation * cardRotationOffset);
        }

        public void SetFingerCard(bool active)
        {
            if (fingerCard == null) return;
            fingerCard.gameObject.SetActive(active);
        }
    }
}
