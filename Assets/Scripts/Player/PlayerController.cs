using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerController : MonoBehaviour, IDamagable
    {
        public float maxHealth = 50f;
        public Slider healthBar;

        void Start()
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }

        void Update()
        {
            transform.position = Camera.main.transform.position;
            if (healthBar.value >= maxHealth) return;
            healthBar.value += Time.deltaTime;
        }
        
        public void Damage()
        {
            if (healthBar.value <= 0f) return;
            healthBar.value -= 1f;
        }
    }
}
