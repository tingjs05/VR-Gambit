using UnityEngine;

namespace Card
{
    [RequireComponent(typeof(Rigidbody), typeof(CardSteering))]
    public class CardObject : MonoBehaviour
    {
        public float maxActiveDuration = 5f;
        public TrailRenderer trailRenderer;
        public ParticleSystem glow;

        private Rigidbody rb;
        private float defaultLaunchForce = 500f;
        private float activeDuration = 0f;

        private bool is_active = false;
        public bool isActive
        {
            get { return is_active; }

            private set 
            {
                is_active = value;
                if (!is_active) return;
                activeDuration = 0f;
            }
        }

        public CardSteering steeringController { get; private set; }

        public void Initialize(float defaultLaunchForce)
        {
            this.defaultLaunchForce = defaultLaunchForce;
            trailRenderer.enabled = false;
            rb = GetComponent<Rigidbody>();
            steeringController = GetComponent<CardSteering>();
            steeringController.enabled = false;
        }

        public void ResetCard(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
            rb.velocity = Vector3.zero;
            trailRenderer.enabled = false;
            steeringController.enabled = false;
        }

        public void HoverCard()
        {
            glow.Play();
            isActive = false;
            rb.isKinematic = true;
            trailRenderer.enabled = false;
        }

        public void LaunchCard(Vector3 forwardDir, float launchForce)
        {
            activeDuration = 0f;
            isActive = true;
            glow.Stop();
            trailRenderer.enabled = true;
            rb.isKinematic = false;
            rb.AddForce(transform.rotation * forwardDir * launchForce);
        }

        private void Update()
        {
            if (!isActive) return;
            activeDuration += Time.deltaTime;
            if (activeDuration <= maxActiveDuration) return;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            // hide when hit target
            if (other.CompareTag("Target"))
            {
                gameObject.SetActive(false);
                return;
            }

            if (!isActive || !other.CompareTag("Hand")) return;
            LaunchCard(Vector3.down, defaultLaunchForce);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Hand")) return;
            isActive = true;
        }
    }
}
