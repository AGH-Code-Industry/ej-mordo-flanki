using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    [Header("Characters Panels")]
    public List<GameObject> charactersPanels = new List<GameObject>();

    [Header("Selection Characters")] 
    public List<GameObject> selectionCharacters = new List<GameObject>();
    
    [Header("Kocham Piwo")] 
    public GameObject StartGamePanel;
    public GameObject MainPanel;
    public int _charactersToSelect = GameInfo.charactersAmount;
    private string player1Nick = "Player1";
    private string player2Nick = "Player2";
    public TMP_Text nickText;
    
    private Vector3 standardSize = new Vector3(0.05f, 0.05f, 0.05f);
    private Vector3 scaledSize = new Vector3(0.08f, 0.08f, 0.08f);

    private Color standardColor = Color.white;
    private Color chosenColor = Color.gray;
    

    public bool isPlayer1 = true;
    public bool isPlayer2 = false;

    private int counter = 0;
    public int charactersToSelect
    {
        
        get { return _charactersToSelect;  }
        set
        {
            _charactersToSelect = value;
            if (_charactersToSelect == 0)
            {
                counter++;
                if (isPlayer1)
                {
                    isPlayer1 = false;
                    isPlayer2 = true;
                    _charactersToSelect = 4;
                    BringColorsBack();
                    DeactivateAllBut(-1);
                    MainPanel.SetActive(true);
                    nickText.text = player2Nick + " turn!";
                }

                if (counter == 2)
                {
                    DeactivateAllBut(-1);
                    StartGamePanel.SetActive(true);
                    BringColorsBack();
                }
            }
        }
    }

    private void Awake()
    {
        nickText.text = player1Nick + " turn!";
    }

    public void AddToList(int indexToAdd)
    {
        selectionCharacters[indexToAdd].GetComponent<SpriteRenderer>().color = chosenColor;
        
        if (isPlayer1)
        {
            if (charactersToSelect > 0 && !GameInfo.selectedPlayer1.Contains(indexToAdd))
            {
                GameInfo.selectedPlayer1.Add(indexToAdd);
                charactersToSelect--;
            }
        }
        else
        {
            if (charactersToSelect > 0 && !GameInfo.selectedPlayer2.Contains(indexToAdd))
            {
                GameInfo.selectedPlayer2.Add(indexToAdd);
                charactersToSelect--;
            }
        }
        
    }
    
        
    private void DeactivateAllBut(int index)
    {
        MainPanel.SetActive(false);

        foreach (GameObject panel in charactersPanels)
        {
            panel.SetActive(false);
        }

        if (index >= 0)
        {
            charactersPanels[index].SetActive(true);
        }

        foreach (GameObject character in selectionCharacters)
        {
            character.transform.localScale = standardSize;
        }

        if (index >= 0)
        {
            selectionCharacters[index].transform.localScale = scaledSize;
        }
    }

    private void BringColorsBack()
    {
        foreach (GameObject character in selectionCharacters)
        {
            character.GetComponent<SpriteRenderer>().color = standardColor;
        }
    }
    
    public void ChooseBlue()
    {
        AddToList(0);
    }
    
    public void ChooseGreen()
    {
        AddToList(1);
    }
    public void ChoosePurple()
    {
        AddToList(2);
    }
    public void ChooseRed()
    {
        AddToList(3);
    }
    public void ChooseRedHair()
    {
        AddToList(4);
    }
    public void ChooseWhite()
    {
        AddToList(5);
    }
    public void ChooseYellow()
    {
        AddToList(6);
    }
    public void ChooseBlack()
    {
        AddToList(7);
    }
    

    public void StartGameButon()
    {
        SceneManager.LoadScene("MovementScene");
    }
    
    public void BlueButton()
    {
        DeactivateAllBut(0);
    }
    public void GreenButton()
    {
        DeactivateAllBut(1);
    }
    public void PurpleButton()
    {
        DeactivateAllBut(2);
    }
    public void RedButton()
    {
        DeactivateAllBut(3);
    }
    public void RedHairButton()
    {
        DeactivateAllBut(4);
    }
    public void WhiteButton()
    {
        DeactivateAllBut(5);
    }
    public void YellowButton()
    {
        DeactivateAllBut(6);
    }
    public void BlackButton()
    {
        DeactivateAllBut(7);
    }
}
