using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWonManager : MonoBehaviour
{
    public TMP_Text playerWonText;
    public TMP_Text playAgain;
    public List<GameObject> Buttons = new List<GameObject>();
    
    void Start()
    {
        Invoke("DisplayUI", 1f);
    }

    private void DisplayUI()
    {
        if (GameInfo.player1Won)
        {
            playerWonText.text = GameInfo.player1Nick + " Won!";
        }
        else
        {
            playerWonText.text = GameInfo.player2Nick + " Won!";
        }
        
        playAgain.GameObject().SetActive(true);
        foreach (GameObject Button in Buttons)
        {
            Button.SetActive(true);
        }
        
        GameInfo.player1Won = false;
        GameInfo.player2Won = false;
    }

    public void YesButton()
    {
        SceneManager.LoadScene("MovementScene");
    }

    public void SelectTeamButton()
    {
        SceneManager.LoadScene("SelectionScene");
        GameInfo.selectedPlayer1 = new List<int>();
        GameInfo.selectedPlayer2 = new List<int>();
    }

    public void NoButton()
    {
        Application.Quit();
    }

}
