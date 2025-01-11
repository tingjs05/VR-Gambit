using System.Collections.Generic;
using UnityEngine;

namespace Target
{
    public class ShootHandler : MonoBehaviour
    {
        public LazerProjectile lazerPrefab;
        private List<LazerProjectile> lazerPool = new List<LazerProjectile>();

        public void Shoot()
        {
            InstantiateAndShoot(transform.parent.position, transform.parent.rotation);
        }

        public void InstantiateAndShoot(Vector3 position, Quaternion rotation)
        {
            foreach (LazerProjectile lazer in lazerPool)
            {
                if (lazer.gameObject.activeSelf) continue;
                lazer.ResetObject(position, rotation);
                lazer.Shoot();
                return;
            }

            GameObject obj = Instantiate(lazerPrefab.gameObject, position, rotation);
            lazerPool.Add(obj.GetComponent<LazerProjectile>());
            lazerPool[^1].Shoot();
        }
    }
}
