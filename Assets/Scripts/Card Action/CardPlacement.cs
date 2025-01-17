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
            // start coroutine to float to float position
            StartCoroutine(FloatToPosition(card, handPosition + offset));
            // if selected target is not null, point towards selected target
            if (selectedTarget == null) return;
            card.transform.rotation = Quaternion.LookRotation(handRotation * Vector3.forward, 
                (handPosition + offset - selectedTarget.position).normalized);
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
