using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ThrowCan : MonoBehaviour
{

    public bool hitTargetCan = false;
    public bool hitSomethingElse = false;


    public GameManager GameManager;

    void Awake()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "TargetCan")
        {
            hitTargetCan = true;
            gameObject.SetActive(false);
        }
        else
        {
            GameManager.hittedSomethingElse = true;
            hitSomethingElse = true;
            gameObject.SetActive(false);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("SpeedBoost"))
        {
            GameManager.applySpeedBoost();
        }
    }
}
