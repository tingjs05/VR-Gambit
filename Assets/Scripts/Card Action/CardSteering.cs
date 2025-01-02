using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class CardSteering : MonoBehaviour
{

    private InputDevice handDevice;
    private XRHandSubsystem handSubsystem;
    private XRHand hand;
    private XRHandJoint indexTip, indexIntermediate;

    private Rigidbody rb;

    public float steerStrength = 1.5f;
    public float maxSteerAngle = 60f;

    public float maxVelocity = 10f;

    private Quaternion handRotation;

    private Vector3 indexDir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Get the InputDevice for the specified hand
        handDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        // check if hand device is valid
        if (!handDevice.isValid)
            Debug.LogWarning("Right hand device is not valid!");

        // get hand subsystem
        handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
        // get hand
        if (handSubsystem == null) return;

        hand = handSubsystem.rightHand;
    }

    // Update is called once per frame
    void Update()
    {


        if (!handDevice.isValid) return;

        // set finger tip joint
        indexTip = hand.GetJoint(XRHandJointID.IndexTip);
        indexIntermediate = hand.GetJoint(XRHandJointID.IndexIntermediate);

        handDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out handRotation);

        if (!indexTip.TryGetPose(out Pose indexTipPose) || !indexIntermediate.TryGetPose(out Pose indexIntermediatePose)) return;

        indexDir = (indexTipPose.position - indexIntermediatePose.position).normalized;

        SteerCard(indexDir, handRotation);

    }


    public void SteerCard(Vector3 indexDir, Quaternion handRotation)
    {
        if (rb == null) return;

        rb.useGravity = false;

        Vector3 steerDir = Vector3.RotateTowards(rb.transform.forward, indexDir, Mathf.Deg2Rad * maxSteerAngle, 0f);

        rb.AddForce(steerDir.normalized * steerStrength, ForceMode.Acceleration);

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(steerDir, handRotation * Vector3.forward), Time.deltaTime * steerStrength));

        if (rb.velocity.magnitude > maxVelocity) rb.velocity = rb.velocity.normalized * maxVelocity;

    }
}
