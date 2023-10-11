using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float throwForce = 10f;
    public float rotationSpeed = 180f; 
    public float rotationAngle = 45f; 
    public Vector3 rotationAxis = Vector3.forward;
    public bool isStopped = false;
    public GameObject throwCan;
    private bool rotateClockwise = true;
    public GameObject arrowTip;

    void OnEnable()
    {
        throwCan.transform.position = arrowTip.transform.position;
        throwCan.SetActive(true);
    }

    void Awake()
    {
        if (transform.parent.parent.name == "Player2")
        {
            transform.localPosition = -transform.localPosition;
            transform.rotation = Quaternion.Euler(0, 0f, 180f);
        }

    }

    void Update()
    {
        if (!isStopped)
        {
            transform.RotateAround(arrowTip.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isStopped = true;
            Throw(transform.right);
        }
    }

    void Throw(Vector2 direction)
    {
        throwCan.GetComponent<Rigidbody2D>().AddForce(direction * throwForce, ForceMode2D.Impulse);
    }
}

