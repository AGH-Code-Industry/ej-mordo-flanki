using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHome : MonoBehaviour
{

    public bool isHome = false;
    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Character")
        {
            isHome = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Character")
        {
            isHome = false;
        }
    }

}
