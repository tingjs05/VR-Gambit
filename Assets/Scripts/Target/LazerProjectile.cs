using UnityEngine;

namespace Target
{
    [RequireComponent(typeof(Rigidbody))]
    public class LazerProjectile : MonoBehaviour
    {
        public float shootForce = 1000f;
        public float maxActiveDuration = 5f;
        private float timeElasped = 0f;
        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            timeElasped += Time.deltaTime;

            if (rb.velocity == Vector3.zero || timeElasped > maxActiveDuration)
                gameObject.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Target")) return;
            gameObject.SetActive(false);
            if (!other.TryGetComponent<IDamagable>(out IDamagable damagable)) return;
            damagable.Damage();
        }

        public void ResetObject(Vector3 position, Quaternion rotation)
        {
            timeElasped = 0f;
            rb.velocity = Vector3.zero;
            transform.position = position;
            transform.rotation = rotation;
            gameObject.SetActive(true);
        }

        public void Shoot()
        {
            if (rb == null) Start();
            rb.AddForce(transform.forward * shootForce);
        }
    }
}