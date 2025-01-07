using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Card
{
    public class ActionManager : MonoBehaviour
    {
        public CardObject cardPrefab;
        public ParticleSystem cardHitParticle;
        public float defaultLaunchForce = 500f;

        public static ActionManager Instance { get; private set; }

        private List<CardObject> cardPool = new List<CardObject>();
        private List<ParticleSystem> hitParticlePool = new List<ParticleSystem>();

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                gameObject.SetActive(false);
        }

        public CardObject InstantiateCard(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            foreach (CardObject card in cardPool)
            {
                if (card.gameObject.activeSelf) continue;
                if (parent != null) card.transform.parent = parent;
                card.ResetCard(position, rotation);
                card.gameObject.SetActive(true);
                return card;
            }

            GameObject obj = Instantiate(cardPrefab.gameObject, position, rotation);
            if (parent != null) obj.transform.parent = parent;
            cardPool.Add(obj.GetComponent<CardObject>());
            cardPool[^1].Initialize(defaultLaunchForce);
            return cardPool[^1];
        }

        public void SpawnHitParticle(Vector3 position)
        {
            foreach (ParticleSystem particleSystem in hitParticlePool)
            {
                if (particleSystem.isPlaying) continue;
                particleSystem.transform.position = position;
                particleSystem.Play();
                return;
            }

            GameObject obj = Instantiate(cardHitParticle.gameObject);
            hitParticlePool.Add(obj.GetComponent<ParticleSystem>());
            hitParticlePool[^1].transform.position = position;
            hitParticlePool[^1].Play();
        }
    }
}
