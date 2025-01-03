using System.Collections;
using UnityEngine;

namespace Card
{
    [RequireComponent(typeof(Rigidbody), typeof(CardSteering))]
    public class CardObject : MonoBehaviour
    {
        public float maxActiveDuration = 5f;
        public float maxSteerDuration = 5f;
        public TrailRenderer trailRenderer;
        public ParticleSystem glow;
        public GameObject chargedGlow;

        [Header("Animation")]
        public float touchCardLaunchDelay = 0.5f;
        public Animator anim;

        private Rigidbody rb;
        private CardSteering steeringController;
        private float defaultLaunchForce = 500f;
        private float activeDuration = 0f;
        private float steerDuration = 0f;
        private bool launched = false;

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

        private bool can_steer = false;
        public bool canSteer
        {
            get { return can_steer; }

            set
            {
                can_steer = value;
                steeringController.enabled = value;
                if (!can_steer) return;
                steerDuration = 0f;
            }
        }

        public void Initialize(float defaultLaunchForce)
        {
            this.defaultLaunchForce = defaultLaunchForce;
            trailRenderer.enabled = false;
            glow.Stop();
            anim.Play("Default");
            rb = GetComponent<Rigidbody>();
            steeringController = GetComponent<CardSteering>();
            canSteer = false;
            launched = false;
        }

        public void ResetCard(Vector3 position, Quaternion rotation)
        {
            glow.Stop();
            anim.Play("Default");
            transform.position = position;
            transform.rotation = rotation;
            rb.velocity = Vector3.zero;
            trailRenderer.enabled = false;
            canSteer = false;
            launched = false;
        }

        public void HoverCard()
        {
            glow.Play();
            isActive = false;
            rb.isKinematic = true;
            trailRenderer.enabled = false;
        }

        public void LaunchCard(Vector3 forwardDir, float launchForce, bool useGravity = true, bool? fromRightHand = null)
        {
            launched = true;
            isActive = true;
            // rotate card to face direction
            transform.forward = transform.rotation * forwardDir;
            glow.Stop();
            trailRenderer.enabled = true;
            rb.isKinematic = false;
            rb.useGravity = useGravity;
            // add force to launch card
            rb.AddForce(transform.forward * launchForce);
            // check if card is being thrown from hand
            if (fromRightHand == null)
            {
                // play default animation if card is not being thrown
                anim.Play("Default");
                return;
            }
            // set hand for steering card
            steeringController.SetHand((bool) fromRightHand);
            // play animation
            anim.Play((bool) fromRightHand ? "Spin" : "Spin (Reverse)");
        }

        private void Update()
        {
            if (!isActive) return;
            HandleSteering();
            activeDuration += Time.deltaTime;
            if (activeDuration <= maxActiveDuration) return;
            gameObject.SetActive(false);
        }

        private void HandleSteering()
        {
            if (!canSteer) return;
            steerDuration += Time.deltaTime;
            if (steerDuration <= maxSteerDuration) return;
            canSteer = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // hide when hit target
            if (other.CompareTag("Target"))
            {
                gameObject.SetActive(false);
                return;
            }

            if (launched || !isActive || !other.CompareTag("Hand")) return;
            // delay launch to play animation
            isActive = false;
            anim.Play("Transition");
            StartCoroutine(DelayedLaunchCard(touchCardLaunchDelay, Vector3.down, defaultLaunchForce));
        }

        private void OnTriggerExit(Collider other)
        {
            if (launched || !other.CompareTag("Hand")) return;
            isActive = true;
        }

        private IEnumerator DelayedLaunchCard(float duration, Vector3 direction, float force)
        {
            yield return new WaitForSeconds(duration);
            LaunchCard(direction, force);
        }
    }
}
