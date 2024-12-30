using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;

public class CardSteering : MonoBehaviour
{

    public GameObject testCard;

    private InputDevice handDevice;
    private XRHand hand;

    // Start is called before the first frame update
    void Start()
    {
        handDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
