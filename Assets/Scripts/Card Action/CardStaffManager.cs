using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    [RequireComponent(typeof(BoxCollider))]
    public class CardStaffManager : MonoBehaviour
    {
        public float defaultColliderSize = 0.1f;
        public float frequency = 0.1f;
        public float maxLength = 1.5f;
        public int minLength = 3;
        public Vector3 rotation = new Vector3(-90f, -90f, 0f);
        public GameObject cardPrefab;

        public List<GameObject> cardChildPool = new List<GameObject>();
        BoxCollider col;

        void Awake()
        {
            col = GetComponent<BoxCollider>();
        }

        public void GenerateStaff(float length)
        {
            // check max length, ensure within range
            if (length > maxLength) length = maxLength;


            // hide all cards
            foreach (GameObject card in cardChildPool)
                card.SetActive(false);
            
            // generate collider of staff
            col.size = new Vector3(length, defaultColliderSize, defaultColliderSize);
            // create staff based on length
            float startDist = 0f;

            while (startDist <= length)
            {
                GetCard().transform.localPosition = Vector3.right * (startDist - (length / 2f));
                startDist += frequency;
            }
        }

        public void ActivateShield()
        {
            col.size = new Vector3(col.size.x, col.size.x, col.size.z);
        }

        public GameObject GetCard()
        {
            foreach (GameObject card in cardChildPool)
            {
                if (card.activeSelf) continue;
                card.SetActive(true);
                return card;
            }

            cardChildPool.Add(Instantiate(cardPrefab, transform));
            cardChildPool[^1].transform.localRotation = Quaternion.Euler(rotation);
            return cardChildPool[^1];
        }
    }
}
