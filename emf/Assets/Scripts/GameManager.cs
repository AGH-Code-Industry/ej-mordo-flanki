using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Characters")] 
    public List<GameObject> characters = new List<GameObject>();

    private bool isOver = false;
    private GameObject Player1;
    private GameObject Player2;
    private GameObject[] charactersPlayer1;
    private GameObject[] charactersPlayer2;
    private float moveSpeed;

    public float turnTime = 10f;
    public bool stopTimer = false;
    public bool holdTimer = false;

    private Rigidbody2D TargetCanRB;
    private TargetCan TargetCanSC;
    public bool hittedSomethingElse = false;

    private PlayerMovement currPlayer1SC;
    private PlayerMovement currPlayer2SC;

    private BeerManager BeerManager1SC;
    private BeerManager BeerManager2SC;


    // Start is called before the first frame update
    void Awake()
    {

        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
        charactersPlayer1 = new GameObject[4];
        charactersPlayer2 = new GameObject[4]; //W wersji rozszerzonej ma być opcja wyboru ilości postaci (chyba)
        
        //SelectedCharactersSC = GameObject.Find("SelectedCharacters").GetComponent<SelectedCharacters>();

        for (int i = 0; i < GameInfo.charactersAmount; i++)
        {
            Instantiate(characters[GameInfo.selectedPlayer1[i]], transform.position, transform.rotation, Player1.transform);
            Instantiate(characters[GameInfo.selectedPlayer2[i]], transform.position, transform.rotation, Player2.transform);
        }


        int j = 0;
        float posY = -2f;
        foreach (Transform child in Player1.transform)
        {
            charactersPlayer1[j] = child.gameObject;
            charactersPlayer1[j].transform.position = new Vector3(-6, posY, 0);
            j++;
            posY += 1.5f;

        }

        j = 0;
        posY = -2f;
        foreach (Transform child in Player2.transform)
        {
            charactersPlayer2[j] = child.gameObject;
            charactersPlayer2[j].transform.position = new Vector3(6, posY, 0);
            j++;
            posY += 1.5f;

        }

        TargetCanSC = GameObject.Find("TargetCan").GetComponent<TargetCan>();
        TargetCanRB = GameObject.Find("TargetCan").GetComponent<Rigidbody2D>();
        

        BeerManager1SC = GameObject.Find("BeerManager1").GetComponent<BeerManager>();
        BeerManager2SC = GameObject.Find("BeerManager2").GetComponent<BeerManager>();


    }

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    // Update is called once per frame
    void Update()
    {

        if (TargetCanSC.hitted)
        {
            stopTimer = true;
            TargetCanSC.hitted = false;
        }

        if (hittedSomethingElse)
        {
            stopTimer = true;
            holdTimer = true;
            hittedSomethingElse = false;
        }

        if (BeerManager1SC.isEmpty)
        {
            isOver = true;
            StopAllCoroutines();
            GameInfo.player1Won = true;
            SceneManager.LoadScene("PlayerWonScene");
        }
        if (BeerManager2SC.isEmpty)
        {
            isOver = true;
            StopAllCoroutines();
            GameInfo.player2Won = true;
            SceneManager.LoadScene("PlayerWonScene");
        }
        
    }

    
    IEnumerator ThrowTurnTimer(float trunTime)
    {
        float counter = 0;

        while (counter < turnTime)
        {
            if (!stopTimer)
            {
                counter += Time.deltaTime;
                yield return null;
            }
            else if (stopTimer && holdTimer)
            {
                stopTimer = false;
                holdTimer = false;
                yield return new WaitForSeconds(1f);
                yield break;
            }
            else
            {
                stopTimer = false;
                yield break;
            }
        }
    }
    
    IEnumerator RunTurnTimer(float trunTime)
    {
        float counter = 0;

        while (counter < turnTime * 10)
        {
            if (!stopTimer)
            {
                counter += Time.deltaTime;
                yield return null;
            }
            else
            {
                stopTimer = false;
                yield break;
            }
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
                yield return StartCoroutine(ThrowTurnTimer(turnTime));
                charactersPlayer1[i].transform.GetChild(1).gameObject.SetActive(false);
                
                //---------------------------------------------------------------------- stawianie puchy player2 jezeli player1 trafił

                if (charactersPlayer1[i].transform.GetChild(0).GetComponent<ThrowCan>().hitTargetCan)
                {
                    BeerManager1SC.canDrink = true;
                    
                    TargetCanSC.hitted = false;
                    
                    TargetCanRB.bodyType = RigidbodyType2D.Kinematic;
                    
                    currPlayer2SC = charactersPlayer2[j].GetComponent<PlayerMovement>();
;
                    moveSpeed = currPlayer2SC.moveSpeed;
                    currPlayer2SC.rb.bodyType = RigidbodyType2D.Dynamic;
                    currPlayer2SC.currMoveSpeed = moveSpeed;
                    currPlayer2SC.canMove = true;
                    
                    yield return StartCoroutine(RunTurnTimer(turnTime));

                    BeerManager1SC.canDrink = false;
                    
                    
                    currPlayer2SC.currMoveSpeed = 0;
                    charactersPlayer2[j].transform.position = currPlayer2SC.startPosition;
                    currPlayer2SC.canMove = false;
                    currPlayer2SC.rb.bodyType = RigidbodyType2D.Kinematic;

                    TargetCanRB.bodyType = RigidbodyType2D.Dynamic;
                    
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
                yield return StartCoroutine(ThrowTurnTimer(turnTime));
                charactersPlayer2[j].transform.GetChild(1).gameObject.SetActive(false);
                
                //---------------------------------------------------------------------- stawianie puchy player1 jezeli player2 trafil

                if (charactersPlayer2[j].transform.GetChild(0).GetComponent<ThrowCan>().hitTargetCan)
                {
                    BeerManager2SC.canDrink = true;
                    
                    TargetCanSC.hitted = false;
                    
                    TargetCanRB.bodyType = RigidbodyType2D.Kinematic;
                    
                    currPlayer1SC = charactersPlayer1[i].GetComponent<PlayerMovement>();

                    moveSpeed = currPlayer1SC.moveSpeed;
                    currPlayer1SC.rb.bodyType = RigidbodyType2D.Dynamic;
                    currPlayer1SC.currMoveSpeed = moveSpeed;
                    currPlayer1SC.canMove = true;
                    
                    yield return StartCoroutine(RunTurnTimer(turnTime));

                    BeerManager2SC.canDrink = false;
                    
                    
                    currPlayer1SC.currMoveSpeed = 0;
                    charactersPlayer1[i].transform.position = currPlayer1SC.startPosition;
                    currPlayer1SC.canMove = false;
                    currPlayer1SC.rb.bodyType = RigidbodyType2D.Kinematic;

                    TargetCanRB.bodyType = RigidbodyType2D.Dynamic;

                    charactersPlayer2[j].transform.GetChild(0).gameObject.GetComponent<ThrowCan>().hitTargetCan = false;
                }
                else
                {
                    charactersPlayer2[j].transform.GetChild(0).gameObject.GetComponent<ThrowCan>().hitSomethingElse = false;
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

        }
    }
    

}
