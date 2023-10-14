using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5;
    public float currMoveSpeed = 0;
    public bool canMove = false;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    private string parentName;
    

    public Vector3 startPosition;

    private GameObject targetCan;
    private GameObject targetCanPlace;
    private SpriteRenderer TCPRenderer;
    private Color TCPColor;

    public GameObject PlayerHome;

    private bool isPlayer1 = false;
    private bool isPlayer2 = false;

    // Start is called before the first frame update
    void Start()
    {
        parentName = transform.parent.name;
        
        targetCan = GameObject.Find("TargetCan");
        targetCanPlace = GameObject.Find("TargetCanPlace");
        
        TCPRenderer = targetCanPlace.GetComponent<SpriteRenderer>();
        TCPColor = TCPRenderer.material.color;

        startPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (parentName == "Player1")
        {
            InputsPlayer1();
            isPlayer1 = true;
        }
        
        else if (parentName == "Player2")
        {
            InputsPlayer2();
            isPlayer2 = true;
        }

        if (isPlayer1)
        {

            if (targetCan.GetComponent<TargetCan>().pickUpAllowed && Input.GetKeyDown(KeyCode.Space) && canMove)
            {
                Pickup();
            }

            if (targetCanPlace.GetComponent<TargetCanPlace>().canPlace && Input.GetKeyDown(KeyCode.Space) && canMove)
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
    }

    private void FixedUpdate()
    {
        
        Move();
        
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

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * currMoveSpeed, moveDirection.y * currMoveSpeed);
    }
}
