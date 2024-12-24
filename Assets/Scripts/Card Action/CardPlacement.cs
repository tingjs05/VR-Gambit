using UnityEngine;

namespace Card
{
    public class CardPlacement : MonoBehaviour
    {
        // Method to place a card
        public void PlaceCard(Vector3 handPosition, Quaternion handRotation)
        {
            ActionManager.Instance.InstantiateCard(handPosition, handRotation)
                .HoverCard();
        }
    }
}
