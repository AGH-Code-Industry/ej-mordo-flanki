using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerWonManager : MonoBehaviour
{
    public TMP_Text playerWonText;
    void Start()
    {
        Invoke("setText", 1f);
    }

    private void setText()
    {
        if (GameInfo.player1Won)
        {
            playerWonText.text = "Player1" + " Won!";
        }
        else
        {
            playerWonText.text = "Player2" + " Won!";
        }
    }
    
}
