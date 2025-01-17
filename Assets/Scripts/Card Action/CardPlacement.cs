using System.Collections;
using UnityEngine;

namespace Card
{
    public class CardPlacement : MonoBehaviour
    {
        public float floatSpeed = 0.5f;
        public float positionReachedThreshold = 0.01f;

        // Method to place a card
        public void PlaceCard(Vector3 handPosition, Vector3 offset, Quaternion handRotation, Transform selectedTarget)
        {
            CardObject card = ActionManager.Instance.InstantiateCard(handPosition, handRotation);
            card.HoverCard();
            // if there is a selected target, point towards it
            if (selectedTarget != null) 
                card.transform.up = -(handPosition + offset - selectedTarget.position).normalized;
            // start coroutine to float to float position
            StartCoroutine(FloatToPosition(card, handPosition + offset));
        }

        IEnumerator FloatToPosition(CardObject card, Vector3 targetPos)
        {
            while (Vector3.Distance(card.transform.position, targetPos) > positionReachedThreshold)
            {
                card.transform.position = Vector3.Lerp(card.transform.position, targetPos, Time.deltaTime * floatSpeed);
                yield return null;
            }

            card.transform.position = targetPos;
        }
    }
}
