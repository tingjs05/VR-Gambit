using UnityEngine;
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
        #endregion

        void Awake()
        {
            Default = new DefaultState(this, this);
            Release = new ReleaseState(this, this);
            WindUp = new WindUpState(this, this);
            TwoFinger = new TwoFingerState(this, this);
            Initialize(Default);
        }

        void Start()
        {
            
        }

        new void Update()
        {
            base.Update();
            tempText.text = current_state_name;
        }
    }
}
