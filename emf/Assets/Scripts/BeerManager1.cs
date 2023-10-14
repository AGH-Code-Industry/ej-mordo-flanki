using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeerManager1 : MonoBehaviour
{
    public Image beerBar;

    public float beerAmount = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Period))
        {
            DrinkBeer(5);
        }
    }

    public void DrinkBeer(float drink)
    {
        beerAmount -= drink;
        beerBar.fillAmount = beerAmount / 100f;
        
    }
    
}
