using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float rotationSpeed; 
    private float rotationAngle = 90f;
    public Vector3 rotationAxis = Vector3.forward;
    private int rotationDirection = 1;
    private float currRotation;
    private float startRotation;

    public bool isStopped = false;
    public GameObject arrowTip;
    
    public float throwForce;
    public GameObject throwCan;

    private bool isPlayer1 = false;
    private bool isPlayer2 = false;

    void OnEnable()
    {
        throwCan.transform.position = arrowTip.transform.position;
        throwCan.SetActive(true);

    }

    void OnDisable()
    {
        isStopped = false;
        throwCan.SetActive(false);
    }

    void Awake()
    {
        if (transform.parent.parent.name == "Player2")
        {
            transform.localPosition = -transform.localPosition;
            transform.rotation = Quaternion.Euler(0, 0f, 180f);
            startRotation = 180f;
            isPlayer2 = true;

        }
        else
        {
            startRotation = 0f;
            isPlayer1 = true;
        }

    }

    private void Update()
    {
        if (isPlayer1)
        {
            if (Input.GetKeyDown(KeyCode.V) && !isStopped)
            {
                isStopped = true;
                Throw(transform.right);
            }
        }
        
        else
        {
            if (Input.GetKeyDown(KeyCode.Slash) && !isStopped)
            {
                isStopped = true;
                Throw(transform.right);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isStopped)
        {
            transform.RotateAround(arrowTip.transform.position, rotationAxis, rotationSpeed * rotationDirection * Time.deltaTime);

            currRotation = transform.localRotation.eulerAngles.z;
            
            if (currRotation > 180f)
            {
                currRotation -= 360f;
            }
            
            if (Mathf.Abs(currRotation) >= startRotation + (rotationAngle/2) || Mathf.Abs(currRotation) <= startRotation - (rotationAngle/2))
            {
                rotationDirection *= -1;
            }
            
        }
    }

    void Throw(Vector2 direction)
    {
        throwCan.GetComponent<Rigidbody2D>().AddForce(direction * throwForce, ForceMode2D.Impulse);
    }
}

