using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CardThrowing : MonoBehaviour
{
    // Reference to card prefab
    public GameObject cardPrefab;
    
    private InputDevice rightHandDevice;
    private InputDevice leftHandDevice;

    private Vector3 leftHandPosition;
    private Quaternion leftHandRotation;

    private Vector3 rightHandPosition;
    private Quaternion rightHandRotation;

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
        }

        // Update right hand position and rotation
        if (rightHandDevice.isValid)
        {
            rightHandDevice.TryGetFeatureValue(CommonUsages.devicePosition, out rightHandPosition);
            rightHandDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out rightHandRotation);
        }
    }

    // Method to throw a card
    public void ThrowCard(bool isRightHand = true)
    {
        if (cardPrefab == null)
        {
            Debug.LogWarning("Card Prefab is not assigned!");
            return;
        }

        // Determine which hand to use
        Vector3 handPosition = isRightHand ? rightHandPosition : leftHandPosition;
        Quaternion handRotation = isRightHand ? rightHandRotation : leftHandRotation;

        GameObject thrownCard = Instantiate(cardPrefab, handPosition, handRotation);
        Rigidbody cardRb = thrownCard.GetComponent<Rigidbody>();

        Vector3 forwardDir = handRotation * Vector3.down;
        float throwForce = 500f;
        cardRb.AddForce(forwardDir * throwForce);
    }
}
