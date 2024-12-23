using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CardPlacement : MonoBehaviour
{
    // Reference to card prefab
    public GameObject cardPrefab;

    public GameObject leftHandCollider;
    public GameObject rightHandCollider;

    private InputDevice rightHandDevice;
    private InputDevice leftHandDevice;

    private Vector3 leftHandPosition;
    private Quaternion leftHandRotation;

    private Vector3 rightHandPosition;
    private Quaternion rightHandRotation;

    private List<GameObject> placedCards = new List<GameObject>();
    public int maxCards = 5;

    void Start()
    {
        // Get the InputDevice for the specified hand
        rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (!rightHandDevice.isValid)
        {
            Debug.LogWarning($"Right hand device is not valid!");
        }

        if (!leftHandDevice.isValid)
        {
            Debug.LogWarning($"Left hand device is not valid!");
        }
    }

    void Update()
    {
        // Update left hand position and rotation
        if (leftHandDevice.isValid)
        {
            leftHandDevice.TryGetFeatureValue(CommonUsages.devicePosition, out leftHandPosition);
            leftHandDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out leftHandRotation);

            leftHandCollider.transform.SetPositionAndRotation(leftHandPosition, leftHandRotation);
        }

        // Update right hand position and rotation
        if (rightHandDevice.isValid)
        {
            rightHandDevice.TryGetFeatureValue(CommonUsages.devicePosition, out rightHandPosition);
            rightHandDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out rightHandRotation);

            rightHandCollider.transform.SetPositionAndRotation(rightHandPosition, rightHandRotation);
        }
    }

    // Method to throw a card
    public void PlaceCard(bool isRightHand = true)
    {
        if (cardPrefab != null)
        {

            // determine which hand to use
            Vector3 handPosition = isRightHand ? rightHandPosition : leftHandPosition;
            Quaternion handRotation = isRightHand ? rightHandRotation : leftHandRotation;

            GameObject spawnedCard = Instantiate(cardPrefab, handPosition, handRotation);
            spawnedCard.GetComponent<CardObject>().HoverCard();

            placedCards.Add(spawnedCard);

            // check if the number of cards exceeds the maximum
            if (placedCards.Count > maxCards)
            {
                // remove and destroy the oldest card
                GameObject oldestCard = placedCards[0];
                placedCards.RemoveAt(0);
                Destroy(oldestCard);
            }

        }
        else
        {
            Debug.LogWarning("Card Prefab is not assigned!");
        }
    }
}
