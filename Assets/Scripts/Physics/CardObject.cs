using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{

    public bool IsPlaced { get; private set; } = false;

    private Rigidbody rb;
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoverCard()
    {
        rb.isKinematic = true;
        boxCollider.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Hand"))
        {
            rb.isKinematic = false;
            boxCollider.isTrigger = false;

            Vector3 forwardDir = transform.rotation * Vector3.forward;
            float throwForce = 500f;
            rb.AddForce(forwardDir * throwForce);
        }
    }
}
