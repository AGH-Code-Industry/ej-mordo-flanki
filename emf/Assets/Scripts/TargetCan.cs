using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCan : MonoBehaviour
{

    public bool pickUpAllowed = false;
    private TargetCanPlace targetCanPlace;
    public bool hitted = false;
    public Rigidbody2D rb;
    public float slow = 0.5f; 

    void Awake()
    {
        targetCanPlace = GameObject.Find("TargetCanPlace").GetComponent<TargetCanPlace>();   
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0)
        {
            SlowDown();
        }
    }

    private void OnEnable()
    {
        pickUpAllowed = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Character")
        {
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Character")
        {
            pickUpAllowed = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ThrowCan")
        {
            hitted = true;
        }
    }

    void SlowDown()
    {
        if (!float.IsNaN(rb.velocity.x) && !float.IsNaN(rb.velocity.y))
        {
            float slowDownFactor = 1f - (slow * Time.deltaTime);
            rb.velocity *= slowDownFactor;
        }
        
        if (!float.IsNaN(rb.angularVelocity))
        {
            float rotationSlowDownFactor = 1f - (slow * Time.deltaTime);
            rb.angularVelocity *= rotationSlowDownFactor;
        }
    }

}
