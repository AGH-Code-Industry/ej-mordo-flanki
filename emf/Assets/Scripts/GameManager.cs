using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private bool isOver = false;
    private GameObject Player1;
    private GameObject Player2;
    private GameObject[] charactersPlayer1;
    private GameObject[] charactersPlayer2;
    private float moveSpeed;
    public float turnTime = 10f;
    public bool stopTimer = false;
    private PlayerMovement currPlayer1PM;
    private PlayerMovement currPlayer2PM;


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

    IEnumerator TurnTimer(float trunTime)
    {
        float counter = 0;

        while (counter < turnTime && !StopTimer())
        {
            counter += Time.deltaTime;
            yield return null;
        }
    }

    bool StopTimer()
    {
        if (stopTimer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    
    IEnumerator GameLoop()
    {
        int i = 0;
        int j = 0;
        int turnCounter = 0;
        while (!isOver)
        {

            if (turnCounter % 2 == 0) // player1 - rzuca | player2 - biegnie
            {
                //---------------------------------------------------------------------- rzut w puche player1
            
                charactersPlayer1[i].transform.GetChild(1).gameObject.SetActive(true);
                yield return StartCoroutine(TurnTimer(turnTime));
                charactersPlayer1[i].transform.GetChild(1).gameObject.SetActive(false);
                
                //---------------------------------------------------------------------- stawianie puchy player2 jezeli player1 trafił

                if (charactersPlayer1[i].transform.GetChild(0).gameObject.GetComponent<ThrowCan>().hitTargetCan == true)
                {

                    Vector2 startPosition = charactersPlayer2[j].transform.position;
                    currPlayer2PM = charactersPlayer2[j].GetComponent<PlayerMovement>();
                    moveSpeed = currPlayer2PM.moveSpeed;
                    currPlayer2PM.rb.bodyType = RigidbodyType2D.Dynamic;
                    currPlayer2PM.currMoveSpeed = moveSpeed;
                    yield return StartCoroutine(TurnTimer(turnTime));
                    currPlayer2PM.currMoveSpeed = 0;
                    charactersPlayer2[j].transform.position = startPosition;
                    currPlayer2PM.rb.bodyType = RigidbodyType2D.Kinematic;
                    charactersPlayer1[i].transform.GetChild(0).gameObject.GetComponent<ThrowCan>().hitTargetCan = false;

                }
                else
                {
                    charactersPlayer1[i].transform.GetChild(0).gameObject.GetComponent<ThrowCan>().hitSomethingElse = false;
                }

            }
            else // player2 - rzuca | player1 - biegnie
            {
                //---------------------------------------------------------------------- rzut w puche player2

                charactersPlayer2[j].transform.GetChild(1).gameObject.SetActive(true);
                yield return StartCoroutine(TurnTimer(turnTime));
                charactersPlayer2[j].transform.GetChild(1).gameObject.GetComponent<ArrowController>().isStopped = false;
                charactersPlayer2[j].transform.GetChild(1).gameObject.SetActive(false);
                
                //---------------------------------------------------------------------- stawianie puchy player1 jezeli player2 trafil

                if (charactersPlayer2[j].transform.GetChild(0).gameObject.GetComponent<ThrowCan>().hitTargetCan == true)
                {

                    Vector2 startPosition = charactersPlayer1[i].transform.position;
                    currPlayer1PM = charactersPlayer1[i].GetComponent<PlayerMovement>();
                    moveSpeed = currPlayer1PM.moveSpeed;
                    currPlayer1PM.rb.bodyType = RigidbodyType2D.Dynamic;
                    currPlayer1PM.currMoveSpeed = moveSpeed;
                    yield return StartCoroutine(TurnTimer(turnTime));
                    currPlayer1PM.currMoveSpeed = 0;
                    charactersPlayer1[i].transform.position = startPosition;
                    currPlayer1PM.rb.bodyType = RigidbodyType2D.Kinematic;
                }
                else
                {
                    charactersPlayer1[i].transform.GetChild(0).gameObject.GetComponent<ThrowCan>().hitSomethingElse = false;
                }

                j++;
                i++;
                if (j > 3 && i > 3)
                {
                    j = 0;
                    i = 0;
                }
                

            }


            turnCounter++;
            if (turnCounter == 10) // zeby petla sie konczyla
            {
                isOver = true;
            }
        }
    }
    

}
