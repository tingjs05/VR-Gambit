using UnityEngine;
using UnityEngine.XR;
using Patterns.FSM;
using TMPro;

namespace Gestures
{
    public class ActionController : StateMachine<ActionController>
    {
        public GestureManager gestureManager;
        public GestureSetting gestureSettings;
        public TextMeshProUGUI tempText;
        public bool isRightHand = true;

        [Header("Action Managers")]
        public CardThrowing cardThrowingManager;

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
            TwoFinger = new TwoFingerState(this, this);
            Snap = new SnapState(this, this);
            Place = new PlaceState(this, this);
            Initialize(Default);
        }

        void Start()
        {
            // Get the InputDevice for the specified hand
            handDevice = InputDevices.GetDeviceAtXRNode(isRightHand ? XRNode.RightHand : XRNode.LeftHand);
            // check if hand device is valid
            if (!handDevice.isValid)
                Debug.LogWarning((isRightHand ? "Right" : "Left") + " hand device is not valid!");
        }

        new void Update()
        {
            base.Update();
            tempText.text = current_state_name;

            // Update hand position and rotation
            if (!handDevice.isValid) return;
            handDevice.TryGetFeatureValue(CommonUsages.devicePosition, out handPosition);
            handDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out handRotation);
        }
    }
}
