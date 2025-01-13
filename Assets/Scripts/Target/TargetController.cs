using UnityEngine;
using Patterns.FSM;

namespace Target
{
    [RequireComponent(typeof(Rigidbody))]
    public class TargetController : StateMachine<TargetController>, IDamagable
    {
        [Header("Movement")]
        public float movementSpeed = 7f;

        [Header("Rotation")]
        public float rotationScale = 1f;
        [Range(0f, 1f)] public float rotationThreshold = 0.995f; 

        [Header("Settings")]
        public float maxMoveDuration = 2f;
        public float minDistanceFromTarget = 5f;
        public float shootCooldown = 1f;

        [Header("Animation")]
        public Animator anim;

        public ChargeState Charge { get; private set; }
        public ShootState Shoot { get; private set; }
        public Rigidbody rb { get; private set; }

        void Awake()
        {
            Charge = new ChargeState(this, this);
            Shoot = new ShootState(this, this);
            Initialize(Charge);

            rb = GetComponent<Rigidbody>();
        }

        public void Damage()
        {
            gameObject.SetActive(false);
            TargetsManager.Instance.OnDeath();
        }
    }
}
