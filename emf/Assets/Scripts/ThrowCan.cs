using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ThrowCan : MonoBehaviour
{

    public bool hitTargetCan = false;
    public bool hitSomethingElse = false;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "TargetCan")
        {
            hitTargetCan = true;
            Invoke("DestroyObject", 1f);
        }
        else
        {
            hitSomethingElse = true;
            Invoke("DestroyObject", 1f);
        }
    }

    void DestroyObject()
    {
        gameObject.SetActive(false);
    }
    
}
