using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private bool isOver = false;
    private GameObject Player1;
    private GameObject Player2;
    private GameObject[] charactersPlayer1;
    private GameObject[] charactersPlayer2;
    private float moveSpeed;
    
    // Start is called before the first frame update
    void Awake()
    {
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
        charactersPlayer1 = new GameObject[4];
        charactersPlayer2 = new GameObject[4]; //W wersji rozszerzonej ma być opcja wyboru ilości postaci (chyba)
        

        int i = 0;
        foreach (Transform child in Player1.transform)
        {
            charactersPlayer1[i] = child.gameObject;
            charactersPlayer1[i].transform.position = new Vector3(-6, i, 0);
            i++;

        }

        i = 0;
        foreach (Transform child in Player2.transform)
        {
            charactersPlayer2[i] = child.gameObject;
            charactersPlayer2[i].transform.position = new Vector3(4, i, 0);
            i++;

        }

    }

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GameLoop()
    {
        int i = 0;
        int j = 0;
        int turnCounter = 0;
        while (!isOver)
        {

            if (turnCounter % 2 == 0) // player1
            {
                Vector2 startPosition = charactersPlayer1[i].transform.position;
                moveSpeed = charactersPlayer1[i].GetComponent<PlayerMovement>().moveSpeed;
                charactersPlayer1[i].GetComponent<PlayerMovement>().currMoveSpeed = moveSpeed;
                yield return new WaitForSeconds(5f);
                charactersPlayer1[i].GetComponent<PlayerMovement>().currMoveSpeed = 0;
                charactersPlayer1[i].transform.position = startPosition;
                i++;
                if (i > 4)
                {
                    i = 0;
                }

            }  
            else //player2
            {
                Vector2 startPosition = charactersPlayer2[j].transform.position;
                moveSpeed = charactersPlayer2[j].GetComponent<PlayerMovement>().moveSpeed;
                charactersPlayer2[j].GetComponent<PlayerMovement>().currMoveSpeed = moveSpeed;
                yield return new WaitForSeconds(5f);
                charactersPlayer2[j].GetComponent<PlayerMovement>().currMoveSpeed = 0;
                charactersPlayer2[j].transform.position = startPosition;
                j++;
                if (j > 4)
                {
                    j = 0;
                }

            }


            turnCounter++;
            if (turnCounter == 8) // zeby petla sie konczyla
            {
                isOver = true;
            }
        }
    }

}
