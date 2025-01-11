using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Target
{
    public class TargetsManager : MonoBehaviour
    {
        public int maxTargets = 5;
        public float yPos = 1.5f;
        public Vector2 maxPos;
        public Vector2 minPos;

        public TargetController targetPrefab;
        private List<TargetController> targetPool = new List<TargetController>();

        public static TargetsManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                gameObject.SetActive(false);
        }

        void Start()
        {
            for (int i = 0; i < maxTargets; i++)
                InstantiateTarget();
        }

        public void OnDeath()
        {
            if (targetPool.Where(x => x.gameObject.activeInHierarchy).ToList().Count >= maxTargets) return;
            InstantiateTarget();
        }

        void InstantiateTarget()
        {
            Vector3 position = new Vector3(Random.Range(minPos.x, maxPos.x), yPos, Random.Range(minPos.y, maxPos.y));

            Vector3 forward = Camera.main.transform.position - position;
            forward.y = 0f;
            forward.Normalize();

            foreach (TargetController target in targetPool)
            {
                if (target.gameObject.activeSelf) continue;
                target.transform.position = position;
                target.transform.forward = forward;
                target.gameObject.SetActive(true);
                return;
            }

            GameObject obj = Instantiate(targetPrefab.gameObject, position, Quaternion.identity);
            targetPool.Add(obj.GetComponent<TargetController>());
            targetPool[^1].transform.forward = forward;
        }

        void OnDrawGizmosSelected() 
        {
            Vector3 max_pos = new Vector3(maxPos.x, yPos, maxPos.y);
            Vector3 min_pos = new Vector3(minPos.x, yPos, minPos.y);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(max_pos, 0.5f);
            Gizmos.DrawSphere(min_pos, 0.5f);
            Gizmos.DrawSphere(new Vector3(max_pos.x, yPos, min_pos.z), 0.5f);
            Gizmos.DrawSphere(new Vector3(min_pos.x, yPos, max_pos.z), 0.5f);
        }
    }
}
