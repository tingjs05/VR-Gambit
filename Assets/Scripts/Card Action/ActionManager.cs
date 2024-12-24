using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    public class ActionManager : MonoBehaviour
    {
        public CardObject cardPrefab;
        public float defaultLaunchForce = 500f;

        public static ActionManager Instance { get; private set; }

        private List<CardObject> cardPool = new List<CardObject>();

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
                card.transform.position = position;
                card.transform.rotation = rotation;
                if (parent != null) card.transform.parent = parent;
                card.gameObject.SetActive(true);
                return card;
            }

            GameObject obj = Instantiate(cardPrefab.gameObject, position, rotation);
            if (parent != null) obj.transform.parent = parent;
            cardPool.Add(obj.GetComponent<CardObject>());
            cardPool[^1].Initialize(defaultLaunchForce);
            return cardPool[^1];
        }
    }
}
