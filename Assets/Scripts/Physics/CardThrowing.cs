using UnityEngine;

public class CardThrowing : MonoBehaviour
{
    // Reference to card prefab
    public GameObject cardPrefab;
    public float throwScale = 500f;

    // Method to throw a card
    public void ThrowCard(Vector3 outVector, Vector3 handPosition, Quaternion handRotation, float throwSpeed)
    {
        if (cardPrefab == null)
        {
            Debug.LogWarning("Card Prefab is not assigned!");
            return;
        }

        GameObject thrownCard = Instantiate(cardPrefab, handPosition, handRotation);
        Rigidbody cardRb = thrownCard.GetComponent<Rigidbody>();

        Vector3 forwardDir = handRotation * (Vector3.down + outVector).normalized;
        cardRb.AddForce(forwardDir * throwScale * throwSpeed);
    }
}
