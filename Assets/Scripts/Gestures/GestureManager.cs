using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gestures
{
    public class GestureManager : MonoBehaviour
    {
        public Dictionary<string, bool> Gestures { get; private set; } = new Dictionary<string, bool>();
        [SerializeField] string[] gestureNames;

        void Awake()
        {
            // check if there are gesture names to add
            if (gestureNames == null || gestureNames.Length <= 0) return;
            // add gesture for each name
            foreach (string name in gestureNames)
                Gestures.Add(name, false);
        }

        void LateUpdate()
        {
            // reset all gestures after each frame
            foreach (string key in Gestures.Keys)
            {
                Gestures[key] = false;
            }
        }

        public void GestureActive(string name)
        {
            SetBool(name, true);
        }

        public void GestureInactive(string name)
        {
            SetBool(name, false);
        }

        public void SetBool(string name, bool value)
        {
            if (!Gestures.Keys.Contains(name))
            {
                Debug.LogWarning($"GestureManager.cs (line 16): A gesture with the name '{name}' could not be found. ");
                return;
            }

            Gestures[name] = value;
        }
    }
}
