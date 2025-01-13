using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Target
{
    public class TargetsManager : MonoBehaviour
    {
        public int maxTargets = 5;
        public float yPos = 1.5f;

        public TargetSpawnRange[] targetSpawnRange;
        [System.Serializable]
        public struct TargetSpawnRange
        {
            public Vector2 maxPos;
            public Vector2 minPos;
        }

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
            int index = Random.Range(0, targetSpawnRange.Length);
            Vector3 position = new Vector3(Random.Range(targetSpawnRange[index].minPos.x, targetSpawnRange[index].maxPos.x), yPos, 
                Random.Range(targetSpawnRange[index].minPos.y, targetSpawnRange[index].maxPos.y));

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
            if (targetSpawnRange == null || targetSpawnRange.Length == 0) return;

            Vector3 max_pos, min_pos;

            foreach (TargetSpawnRange spawnRange in targetSpawnRange)
            {
                max_pos = new Vector3(spawnRange.maxPos.x, yPos, spawnRange.maxPos.y);
                min_pos = new Vector3(spawnRange.minPos.x, yPos, spawnRange.minPos.y);

                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(max_pos, 0.5f);
                Gizmos.DrawSphere(min_pos, 0.5f);
                Gizmos.DrawSphere(new Vector3(max_pos.x, yPos, min_pos.z), 0.5f);
                Gizmos.DrawSphere(new Vector3(min_pos.x, yPos, max_pos.z), 0.5f);
            }
        }
            
    }
}
