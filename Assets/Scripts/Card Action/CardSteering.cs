using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

namespace Card
{
    [RequireComponent(typeof(Rigidbody))]
    public class CardSteering : MonoBehaviour
    {
        public float steerStrength = 1.5f;
        public float maxSteerAngle = 60f;
        public float maxVelocity = 10f;

        private InputDevice handDevice;
        private XRHandSubsystem handSubsystem;
        private XRHand hand;
        private XRHandJoint indexTip, indexIntermediate;
        private Rigidbody rb;
        private Quaternion handRotation;
        private Vector3 indexDir, steerDir;

        void Update()
        {
            if (handDevice == null || !handDevice.isValid) return;
            // set finger tip joint
            indexTip = hand.GetJoint(XRHandJointID.IndexTip);
            indexIntermediate = hand.GetJoint(XRHandJointID.IndexIntermediate);
            handDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out handRotation);
            if (!indexTip.TryGetPose(out Pose indexTipPose) || !indexIntermediate.TryGetPose(out Pose indexIntermediatePose)) return;
            indexDir = (indexTipPose.position - indexIntermediatePose.position).normalized;
            SteerCard(indexDir, handRotation);
        }

        public void SetHand(bool isRightHand)
        {
            // Get the InputDevice for the specified hand
            handDevice = InputDevices.GetDeviceAtXRNode(isRightHand ? XRNode.RightHand : XRNode.LeftHand);
            // check if hand device is valid
            if (!handDevice.isValid) Debug.LogWarning((isRightHand ? "Right" : "Left") + " hand device is not valid!");
            // get hand subsystem
            if (handSubsystem == null) handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
            if (handSubsystem == null) return;
            // get hand
            hand = isRightHand ? handSubsystem.rightHand : handSubsystem.leftHand;
        }

        public void SteerCard(Vector3 indexDir, Quaternion handRotation)
        {
            // ensure rb is set
            if (rb == null) rb = GetComponent<Rigidbody>();
            if (rb == null) return;
            // steer card
            rb.useGravity = false;
            steerDir = Vector3.RotateTowards(rb.transform.forward, indexDir, Mathf.Deg2Rad * maxSteerAngle, 0f);
            rb.AddForce(steerDir.normalized * steerStrength, ForceMode.Acceleration);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(steerDir, handRotation * Vector3.forward), Time.deltaTime * steerStrength));
            if (rb.velocity.magnitude <= maxVelocity) return; 
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
}
