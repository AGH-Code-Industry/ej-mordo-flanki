using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCan : MonoBehaviour
{

    public bool pickUpAllowed = false;
    private TargetCanPlace targetCanPlace;

    void Awake()
    {
        targetCanPlace = GameObject.Find("TargetCanPlace").GetComponent<TargetCanPlace>();   
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
}
