using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public float launchForce = 500f;
    private bool isTouchedByPlayer = false;
    private bool isActive = false;

    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (isTouchedByPlayer && isActive)
        {
            LaunchCard();
        }
        
    }

    public void HoverCard()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

    }

    public void LaunchCard()
    {

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;

        Vector3 forwardDir = transform.rotation * Vector3.down;
        rb.AddForce(forwardDir * launchForce);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            isTouchedByPlayer = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            isTouchedByPlayer = false;
            isActive = true;
        }
    }
}
