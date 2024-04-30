using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float currMoveSpeed = 0;
    public bool canMove = false;
    private float drinkSpeed;
    private float throwForce;
    public float moveSpeed;
    public float accuracy;
    private float head;

    public BeerManager BeerManagerSC;

    public Rigidbody2D rb;
    private Vector2 moveDirection;

    public Vector3 startPosition;

    private GameObject targetCan;
    private GameObject targetCanPlace;
    private SpriteRenderer TCPRenderer;
    private Color TCPColor;

    public GameObject PlayerHome;

    private bool isPlayer1 = false;
    private bool isPlayer2 = false;

    public GameManager GameManagerSC;
    public CharacterStats CharacterStats;
    public ArrowController ArrowController;

    private float swayAmount = 0;
    private float targetSwayAmount = 0;
    private float swayChangeInterval = 1.0f;
    private float lastSwayChangeTime = 0f;

    private float drunkennessTimeCounter = 0.0f;
    private float drunkennessEffectFrequency = 4.0f;
    private float drunkBeer = 0.0f;
    private float timer = 0f;
    private int offset = 10;

    [SerializeField]
    public GameObject graphics;

    private void Awake()
    {
        GameManagerSC = GameObject.Find("GameManager").GetComponent<GameManager>();
        drinkSpeed = CharacterStats.drinkSpeed / 600;
        ArrowController.throwForce = CharacterStats.throwForce;
        moveSpeed = CharacterStats.moveSpeed / 4;
        accuracy = CharacterStats.accuracy;
        ArrowController.rotationSpeed = 120 - CharacterStats.accuracy;
        head = CharacterStats.head;
    }

    void Start()
    {
        if (transform.parent.name == "Player1")
        {
            isPlayer1 = true;
            BeerManagerSC = GameObject.Find("BeerManager1").GetComponent<BeerManager>();
            BeerManagerSC.beerSpeed += drinkSpeed;
        }
        else
        {
            isPlayer2 = true;
            BeerManagerSC = GameObject.Find("BeerManager2").GetComponent<BeerManager>();
            BeerManagerSC.beerSpeed += drinkSpeed;
            graphics.GetComponent<SpriteRenderer>().flipX = true;
        }

        targetCan = GameObject.Find("TargetCan");
        targetCanPlace = GameObject.Find("TargetCanPlace");
        TCPRenderer = targetCanPlace.GetComponent<SpriteRenderer>();
        TCPColor = TCPRenderer.material.color;
        startPosition = transform.position;
    }

    void Update()
    {
        if (isPlayer1)
        {
            InputsPlayer1();
        }
        else if (isPlayer2)
        {
            InputsPlayer2();
        }

        if (isPlayer1)
        {
            if (targetCan.GetComponent<TargetCan>().pickUpAllowed && Input.GetKeyDown(KeyCode.C) && canMove)
            {
                Pickup();
            }
            if (targetCanPlace.GetComponent<TargetCanPlace>().canPlace && Input.GetKeyDown(KeyCode.C) && canMove)
            {
                Place();
            }
        }
        else
        {
            if (targetCan.GetComponent<TargetCan>().pickUpAllowed && Input.GetKeyDown(KeyCode.Slash) && canMove)
            {
                Pickup();
            }
            if (targetCanPlace.GetComponent<TargetCanPlace>().canPlace && Input.GetKeyDown(KeyCode.Slash) && canMove)
            {
                Place();
            }
        }

        drunkBeer = BeerManagerSC.getDrunkBeer() / drinkSpeed;
    }

    private void FixedUpdate()
    {
        ApplyDrunkenEffect();
        Move();
        timer += 0.005f;
    }

    // void ApplyDrunkenEffect()
    // {
    //     float normalizedHead = (125.0f - head) / 100.0f;
    //     float normalizedDrunkBeer = drunkBeer / 100.0f;

    //     float drunkenness = normalizedDrunkBeer * normalizedHead;

    //     if (moveDirection.magnitude > 0)
    //     {
    //         float chanceToApplyEffect = drunkenness;
            
    //         if (UnityEngine.Random.Range(0f, 1f) < chanceToApplyEffect)
    //         {
    //             drunkennessTimeCounter += Time.fixedDeltaTime * drunkennessEffectFrequency;
    //             float swayX = Mathf.Sin(drunkennessTimeCounter) * Offset() * Mathf.Sin(drunkennessTimeCounter + 10) * drunkenness / 2;
    //             float swayY = Mathf.Cos(drunkennessTimeCounter) * Offset() * Mathf.Sin(drunkennessTimeCounter + 10) * drunkenness / 2;

    //             moveDirection.x += swayX;
    //             moveDirection.y += swayY;
    //             moveDirection = moveDirection.normalized;
    //         }
    //     }
    //     else
    //     {
    //         drunkennessTimeCounter = 0.0f;
    //     }
    // }

    // int Offset()
    // {
    //     if(timer >= 2f){
    //         Debug.Log("chujchuj");
    //         timer = 0f;
    //         return offset*(-1);

    //     }
    //     return offset;
    // }

    void ApplyDrunkenEffect()
    {
        float normalizedHead = (100.0f - head) / 100.0f;
        float normalizedDrunkBeer = drunkBeer / 100.0f;
    
        float drunkenness = normalizedDrunkBeer * normalizedHead;
    
        if (moveDirection.magnitude > 0)
        {        
    
            if (Time.time - lastSwayChangeTime >= swayChangeInterval)
            {
                float offset = drunkenness / (4 + currMoveSpeed / 5);
                Debug.Log(offset);
    
                lastSwayChangeTime = Time.time;
                swayAmount = UnityEngine.Random.Range(- offset, offset);
                targetSwayAmount = UnityEngine.Random.Range(- offset, offset);
            }
    
            swayAmount = Mathf.Lerp(swayAmount, targetSwayAmount, (Time.time - lastSwayChangeTime) / swayChangeInterval) + Mathf.Sin(swayAmount)/4;
            moveDirection.x += swayAmount;
            moveDirection.y += swayAmount;
    
            moveDirection = moveDirection.normalized;
    
        }
        else
        {
            drunkennessTimeCounter = 0.0f;
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * currMoveSpeed, moveDirection.y * currMoveSpeed);
    
        if (moveDirection != Vector2.zero && canMove)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation;

            if (isPlayer2)
            {
                targetRotation = Quaternion.Euler(0, 0, angle - 180);
            }
            else
            {
                targetRotation = Quaternion.Euler(0, 0, angle);
            }
            graphics.transform.rotation = Quaternion.Lerp(graphics.transform.rotation, targetRotation, Time.deltaTime * 10);
        }
    }


    void Pickup()
    {
        targetCan.SetActive(false);
        TCPColor.a = 0.2f;
        TCPRenderer.color = TCPColor;
        targetCan.GetComponent<TargetCan>().pickUpAllowed = false;
    }

    void Place()
    {
        targetCan.SetActive(true);
        TCPColor.a = 0f;
        TCPRenderer.color = TCPColor;
        targetCan.transform.position = targetCanPlace.GetComponent<TargetCanPlace>().placePosition;
        targetCan.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        PlayerHome.SetActive(true);
        PlayerHome.transform.position = startPosition;
    }

    void InputsPlayer1()
    {
        float moveX = Input.GetAxisRaw("Horizontal1");
        float moveY = Input.GetAxisRaw("Vertical1");
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void InputsPlayer2()
    {
        float moveX = Input.GetAxisRaw("Horizontal2");
        float moveY = Input.GetAxisRaw("Vertical2");
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerHome")
        {
            GameManagerSC.stopTimer = true;
            PlayerHome.SetActive(false);
        }
    }
}
