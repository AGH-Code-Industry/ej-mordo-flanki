using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BeerManager : MonoBehaviour
{
    public Image beerBar;

    public float beerAmount = 100f;
    public float beerSpeed = 0;
    
    public bool isPlayer1 = false;
    public bool isEmpty = false;
    public bool canDrink = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == "BeerManager1")
        {
            isPlayer1 = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer1 && canDrink)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                DrinkBeer(beerSpeed);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Period) && canDrink)
        {
            DrinkBeer(beerSpeed);
        }

        if (beerAmount <= 0)
        {
            isEmpty = true;
        }
    }

    public void DrinkBeer(float drink)
    {
        beerAmount -= drink;
        beerBar.fillAmount = beerAmount / 100f;
        
    }
    
}
