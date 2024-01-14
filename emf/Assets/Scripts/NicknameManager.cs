using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class NicknameManager : MonoBehaviour
{
    public TMP_InputField player1Input;
    public TMP_InputField player2Input;
    public GameObject player1Panel;
    public GameObject player2Panel;
    private bool player1turn = true;

    private void Start()
    {
        player1Input.text = "";
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            if (player1turn)
            {
                setNickPlayer1();
                player1turn = false;
                player1Panel.SetActive(false);
                player2Panel.SetActive(true);
                player2Input.text = "";
                Thread.Sleep(500);
            }
            else
            {
                setNickPlayer2();
                SceneManager.LoadScene("ChooseMapScene");
            }
        }
    }

    private void setNickPlayer1()
    {
        GameInfo.setPlayer1Nick(player1Input.text);
    }
    
    private void setNickPlayer2()
    {
        GameInfo.setPlayer2Nick(player2Input.text);
    }
}
