using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class IndexPositionTracking : MonoBehaviour
{

    public GameObject cardPrefab;

    private XRHandSubsystem handSubsystem;
    private InputDevice handDevice;

    private GameObject cardObject;
    private Quaternion handRotation;

    // Start is called before the first frame update
    void Start()
    {
        handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
        handDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    // Update is called once per frame
    void Update()
    {
        if (handSubsystem == null || handDevice == null) return;

        XRHand rightHand = handSubsystem.rightHand;

        if (rightHand != null)
        {
            // obtain right index tip
            XRHandJoint rightIndexTip = rightHand.GetJoint(XRHandJointID.IndexTip);
            XRHandJoint rightMiddleTip = rightHand.GetJoint(XRHandJointID.MiddleTip);
            handDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out handRotation);

            if (rightIndexTip.TryGetPose(out Pose rightTipPose) && rightMiddleTip.TryGetPose(out Pose middleTipPose))
            {

                Vector3 cardPos = (rightTipPose.position + middleTipPose.position) / 2;

                if (cardObject == null)
                {
                    cardObject = Instantiate(cardPrefab);
                    cardObject.transform.localScale = cardObject.transform.localScale / 1.25f;
                }
                else
                {
                    Quaternion rotationOffset = Quaternion.Euler(82, 0 , 0);
                    cardObject.transform.SetPositionAndRotation(cardPos, handRotation * rotationOffset);
                }
                
            }
        }  

    }
}
