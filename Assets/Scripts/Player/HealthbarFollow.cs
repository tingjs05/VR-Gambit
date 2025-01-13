using UnityEngine;

namespace Player
{
    public class HealthbarFollow : MonoBehaviour
    {
        public float rotationScale = 1f;
        public Transform canvas;

        void Update()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Camera.main.transform.rotation, Time.deltaTime * rotationScale);
            canvas.transform.forward = Camera.main.transform.forward;
        }
    }
}
