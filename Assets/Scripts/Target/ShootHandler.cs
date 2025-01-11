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
            InstantiateAndShoot(transform.parent.position, transform.parent.rotation, transform);
        }

        public void InstantiateAndShoot(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            foreach (LazerProjectile lazer in lazerPool)
            {
                if (lazer.gameObject.activeSelf) continue;
                if (parent != null) lazer.transform.parent = parent;
                lazer.Reset(position, rotation);
                lazer.Shoot();
                return;
            }

            GameObject obj = Instantiate(lazerPrefab.gameObject, position, rotation);
            if (parent != null) obj.transform.parent = parent;
            lazerPool.Add(obj.GetComponent<LazerProjectile>());
            lazerPool[^1].Shoot();
        }
    }
}
