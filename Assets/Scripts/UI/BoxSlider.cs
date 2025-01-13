using System;
using UnityEngine;

namespace UI
{
    [ExecuteInEditMode]
    public class BoxSlider : MonoBehaviour
    {
        [SerializeField] Vector3 positionOffset;
        [SerializeField] float maxScale = 1f;
        [SerializeField] bool reverseDirection = false;
        [SerializeField] private Axis axis = Axis.Y;
        private enum Axis
        {
            X, Y, Z
        }

        [SerializeField, Range(0f, 1f)] 
        private float _value = 1f;
        public float value
        {
            get { return _value; }
            set { _value = Mathf.Clamp(value, 0f, maxValue) / maxValue; }
        }

        public float maxValue = 1f;

        private Vector3 axisScale, inverseAxisScale;

        void Start()
        {
            
        }

        void Update()
        {
            SetAxisScale();
            // calculate scale
            inverseAxisScale = Vector3.one - axisScale;
            transform.localScale = ScaleVector(transform.localScale, inverseAxisScale) + 
                (axisScale * value * maxScale);
            // snap to bottom
            transform.localPosition = ScaleVector(positionOffset, inverseAxisScale) + 
                (ScaleVector(positionOffset, axisScale) + (axisScale * 
                ((maxScale / 2f) * (reverseDirection ? 1f : -1f) + 
                ((value * maxScale) / 2f) * (reverseDirection ? -1f : 1f))));
        }

        Vector3 ScaleVector(Vector3 vec, Vector3 scale)
        {
            return new Vector3(vec.x * scale.x, vec.y * scale.y, vec.z * scale.z);
        }

        void SetAxisScale()
        {
            switch (axis)
            {
                case Axis.Y:
                    axisScale = Vector3.up;
                    break;
                case Axis.Z:
                    axisScale = Vector3.forward; 
                    break;
                default:
                    axisScale = Vector3.right;
                    break;
            }
        }
    }
}
