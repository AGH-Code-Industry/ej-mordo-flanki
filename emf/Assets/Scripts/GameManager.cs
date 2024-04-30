using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

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

    private bool player1IsThrowing = false;
    
    
    private float boostSpawnedDuration = 10f;
    
    // speed boost
    private float speedBoostDuration = 40f;
    private float speedBoostProbability = 0.001f;
    private bool speedBoostSpawned = false;
    [SerializeField] private GameObject speedBoost;
    public GameObject speedBoostInstance;
    
    // accuracy boost 
    private float accuracyBoostDuration = 40f;
    private float accuracyBoostProbability = 0.005f;
    private bool accuracyBoostSpawned = false;
    [SerializeField] private GameObject accuracyBoost;
    public GameObject accuracyBoostInstance;
    
    private Vector3 spawnFieldLeftBot = new Vector3(-4, -3, 0);
    private Vector3 spawnFieldRightTop = new Vector3(4, 3, 0);


    // Start is called before the first frame update
    void Awake()
    {

        Instantiate(GameInfo.map, new Vector3(0, 0, 0), Quaternion.identity);

        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
        charactersPlayer1 = new GameObject[4];
        charactersPlayer2 = new GameObject[4]; //W wersji rozszerzonej ma być opcja wyboru ilości postaci (chyba)
        
        //SelectedCharactersSC = GameObject.Find("SelectedCharacters").GetComponent<SelectedCharacters>();

        for (int i = 0; i < GameInfo.teamSize; i++)
        {
            Instantiate(characters[GameInfo.selectedPlayer1[i]], transform.position, transform.rotation, Player1.transform);
            Instantiate(characters[GameInfo.selectedPlayer2[i]], transform.position, transform.rotation, Player2.transform);
        }


        int j = 0;
        float posY = -13f;
        foreach (Transform child in Player1.transform)
        {
            charactersPlayer1[j] = child.gameObject;
            charactersPlayer1[j].transform.position = new Vector3(-23, posY, 0);
            j++;
            posY += 9f;

        }

        j = 0;
        posY = -13f;
        foreach (Transform child in Player2.transform)
        {
            charactersPlayer2[j] = child.gameObject;
            charactersPlayer2[j].transform.position = new Vector3(23, posY, 0);
            j++;
            posY += 9f;

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

        if (UnityEngine.Random.value <= speedBoostProbability && !speedBoostSpawned)
        {
            float randomX = UnityEngine.Random.Range(spawnFieldLeftBot.x, spawnFieldRightTop.x);
            float randomY = UnityEngine.Random.Range(spawnFieldLeftBot.y, spawnFieldRightTop.y);
            float randomZ = 0;
            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);
            speedBoostInstance = Instantiate(speedBoost, randomPosition, Quaternion.identity);
            speedBoostSpawned = true;
            Invoke("destroySpeedBoost", boostSpawnedDuration);
            
        }

        if (UnityEngine.Random.value <= accuracyBoostProbability && !accuracyBoostSpawned)
        {
            
            float randomX = UnityEngine.Random.Range(spawnFieldLeftBot.x, spawnFieldRightTop.x);
            float randomY = UnityEngine.Random.Range(spawnFieldLeftBot.y, spawnFieldRightTop.y);
            float randomZ = 0;
            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);
            accuracyBoostInstance = Instantiate(accuracyBoost, randomPosition, Quaternion.identity);
            accuracyBoostSpawned = true;
            Invoke("destroyAccuracyBoost", boostSpawnedDuration );
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
                player1IsThrowing = true;
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
                    currPlayer2SC.graphics.transform.rotation = Quaternion.Euler(0, 0, 0);
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
                player1IsThrowing = false;
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
                    currPlayer1SC.graphics.transform.rotation = Quaternion.Euler(0, 0, 0);
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
    // SPEED BOOST
    public void applySpeedBoost()
    {
        destroySpeedBoost();
        if (player1IsThrowing)
        {
            for(int i = 0; i < charactersPlayer1.Length; i++)
            {
                PlayerMovement movement = charactersPlayer1[i].GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    
                    movement.moveSpeed *= 1.5f;
                }
            }
            Invoke("deactivateSpeedBoostForPlayer1", speedBoostDuration);
        }
        else
        {
            for(int i = 0; i < charactersPlayer2.Length; i++)
            {
                PlayerMovement movement = charactersPlayer2[i].GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    
                    movement.moveSpeed *= 1.5f;
                }
            }
            Invoke("deactivateSpeedBoostForPlayer2", speedBoostDuration);
        }
    }

    private void deactivateSpeedBoost(bool forPlayer1)
    {
        if (forPlayer1)
        {
            for(int i = 0; i < charactersPlayer1.Length; i++)
            {
                PlayerMovement movement = charactersPlayer1[i].GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    movement.moveSpeed /= 1.5f;
                }
                
            }
        }
        else
        {
            for(int i = 0; i < charactersPlayer2.Length; i++)
            {
                PlayerMovement movement = charactersPlayer2[i].GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    
                    movement.moveSpeed /= 1.5f;
                }
            }
        }
    }

    private void deactivateSpeedBoostForPlayer1()
    {
        deactivateSpeedBoost(true);
    }
    private void deactivateSpeedBoostForPlayer2()
    {
        deactivateSpeedBoost(false);
    }

    private void destroySpeedBoost()
    {
        speedBoostSpawned = false;
        Destroy(speedBoostInstance);
    }
    
    
    // ACCURACY BOOST 
    public void applyAccuracyBoost()
    {
        destroyAccuracyBoost();
        if (player1IsThrowing)
        {
            for (int i = 0; i < charactersPlayer1.Length; i++)
            {
                PlayerMovement movement = charactersPlayer1[i].GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    Debug.Log("zwiekszam celność dla gracza 1, obecna celność: " + movement.accuracy);
                    movement.accuracy  *= 2f;
                    Debug.Log("nowa celność: " + movement.accuracy);
                }
            }
            Invoke("deactivateAccuracyBoostForPlayer1", accuracyBoostDuration);
        }
        else
        {
            for (int i = 0; i < charactersPlayer2.Length; i++)
            {
                PlayerMovement movement = charactersPlayer2[i].GetComponent<PlayerMovement>();
                if (movement != null)
                {   
                    Debug.Log("zwiekszam celność dla gracza 2, obecna celność: " + movement.accuracy);
                    movement.accuracy *= 2f;
                    Debug.Log("nowa celność: " + movement.accuracy);
                }
            }
            Invoke("deactivateAccuracyBoostForPlayer2", accuracyBoostDuration);
        }

    }

    private void deactivateAccuracyBoostForPlayer1()
    {
        deactiveAccuracyBoost(true);
    }

    private void deactivateAccuracyBoostForPlayer2()
    {
        deactiveAccuracyBoost(false);
    }

    private void deactiveAccuracyBoost(bool forPlayer1)
    {
        if (forPlayer1)
        {
            for (int i = 0; i < charactersPlayer1.Length; i++)
            {
                PlayerMovement movement = charactersPlayer1[i].GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    Debug.Log("cofam boost dla gracza 1");
                    movement.accuracy  /= 2f;
                }
            }
        }
        else
        {
            for (int i = 0; i < charactersPlayer2.Length; i++)
            {
                PlayerMovement movement = charactersPlayer2[i].GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    Debug.Log("cofam boost dla gracza 2");
                    movement.accuracy /= 2f;
                }
            }
            
        }
    }

    private void destroyAccuracyBoost()
    {
        accuracyBoostSpawned = false;
        Destroy(accuracyBoostInstance);
    }

}
