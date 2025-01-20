using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gestures
{
    public class TwoHandedGestureManager : MonoBehaviour
    {
        public Dictionary<string, TwoHandedGesture> Gestures { get; private set; } = new Dictionary<string, TwoHandedGesture>();
        public class TwoHandedGesture
        {
            public bool rightHand;
            public bool leftHand;
            public bool active => rightHand && leftHand;

            public TwoHandedGesture(bool rightHand, bool leftHand)
            {
                this.rightHand = rightHand;
                this.leftHand = leftHand;
            }
        }

        [SerializeField] string[] gestureNames;

        void Awake()
        {
            // check if there are gesture names to add
            if (gestureNames == null || gestureNames.Length <= 0) return;
            // add gesture for each name
            foreach (string name in gestureNames)
                Gestures.Add(name, new TwoHandedGesture(false, false));
        }

        public void GestureActiveRight(string name)
        {
            SetBool(name, true, true);
        }

        public void GestureActiveLeft(string name)
        {
            SetBool(name, true, false);
        }

        public void GestureInactiveRight(string name)
        {
            SetBool(name, false, true);
        }

        public void GestureInactiveLeft(string name)
        {
            SetBool(name, false, false);
        }

        public void SetBool(string name, bool value, bool isRightHand)
        {
            if (!Gestures.Keys.Contains(name))
            {
                Debug.LogWarning($"GestureManager.cs (line 16): A gesture with the name '{name}' could not be found. ");
                return;
            }

            if (isRightHand)
                Gestures[name].rightHand = value;
            else
                Gestures[name].leftHand = value;
        }
    }
}
