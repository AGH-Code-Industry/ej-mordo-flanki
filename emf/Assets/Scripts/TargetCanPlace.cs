using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCanPlace : MonoBehaviour
{
    public bool canPlace = false;
    public Vector3 placePosition;

    private void Awake()
    {
        placePosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Character")
        {
            canPlace = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Character")
        {
            canPlace = false;
        }
    }
}
