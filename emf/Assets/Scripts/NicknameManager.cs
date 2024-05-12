using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class NicknameManager : MonoBehaviour
{
    public TMP_InputField player1Input;
    public TMP_InputField player2Input;
    public TMP_Text alert2;
    public TMP_Text alert1;
    public GameObject player1Panel;
    public GameObject player2Panel;
    private bool player1turn = true;

    private void Start()
    {
        player1Input.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (player1turn)
            {
                if (!string.IsNullOrEmpty(player1Input.text))
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
                    alert1.text = "Podaj nickname mordeczko";
                }
            }
            else
            {
                if (GameInfo.getPlayer1Nick() == player2Input.text)
                {
                    player2Input.text = "";
                    alert2.text = "Mordo, Gracz 1 juz wybral ten nick: " + GameInfo.getPlayer1Nick();
                    
                }
                else 
                {
                    if(!string.IsNullOrEmpty(player2Input.text))
                    {
                        setNickPlayer2();
                        SceneManager.LoadScene("ChooseMapScene");
                    }
                    else
                    {
                        alert2.text = "Podaj nickname mordeczko";
                    }
                }
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
