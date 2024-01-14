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
    [Header("Panels Rect")]
    public List<GameObject> panelsRectTransform = new List<GameObject>();

    private List<int> selectedCharacters = new List<int>();
    private int selectedAmount = GameInfo.teamSize*2;

    [Header("Buttons")] 
    public List<Button> buttons;
    public List<Button> chooseButtons;
    
    [Header("Kocham Piwo")]
    public Button StartGame;
    private int currButton = 0;
    public GameObject StartGamePanel;
    private string player1Nick = GameInfo.getPlayer1Nick();
    private string player2Nick = GameInfo.getPlayer2Nick();
    public TMP_Text nickText;

    public GameObject chosenPlayer1Panel;
    public GameObject chosenPlayer2Panel;

    private Vector3 standardSize = new Vector3(0.05f, 0.05f, 0.05f);
    private Vector3 scaledSize = new Vector3(0.08f, 0.08f, 0.08f);
    
    private Color chosenColor = Color.gray;
    

    public bool isPlayer1 = true;
    public bool isPlayer2 = false;

    private int counter = 0;

    private void Awake()
    {
        nickText.text = player1Nick + ", twoja kolej";
    }

    private void Update()
    {
        if (isPlayer1)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !StartGamePanel.activeSelf)
            {
                buttons[0].onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                SelectPreviousButton();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                SelectNextButton();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                ChooseButton();
            }
        }

        if (isPlayer2)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !StartGamePanel.activeSelf)
            {
                buttons[0].onClick.Invoke();
            }

            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SelectPreviousButton();
            }

            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SelectNextButton();
            }
            
            if (Input.GetKeyDown(KeyCode.Period))
            {
                ChooseButton();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && StartGamePanel.activeSelf)
        {
            StartGame.onClick.Invoke();
        }
    }

    void SelectNextButton()
    {
        currButton++;
        if (currButton > 7)
        {
            currButton = 0;
        }
        buttons[currButton].onClick.Invoke();
    }

    void SelectPreviousButton()
    {
        currButton--;
        if (currButton < 0)
        {
            currButton = 7;
        }
        buttons[currButton].onClick.Invoke();
    }

    void ChooseButton()
    {
        chooseButtons[currButton].onClick.Invoke();
    }
    
    public void AddToList(int indexToAdd)
    {
        selectionCharacters[indexToAdd].GetComponent<SpriteRenderer>().color = chosenColor;
        
        if (isPlayer1)
        {
            if (!selectedCharacters.Contains(indexToAdd))
            {
                GameInfo.selectedPlayer1.Add(indexToAdd);
                selectedCharacters.Add(indexToAdd);
                selectedAmount--;

                Instantiate(panelsRectTransform[indexToAdd], chosenPlayer1Panel.transform);

                if (selectedAmount == 0)
                {
                    DeactivateAllBut(-1);
                    StartGamePanel.SetActive(true);
                }
                else
                {
                    SwitchPlayer();
                }
            }
        }
        else
        {
            if (!selectedCharacters.Contains(indexToAdd))
            {
                GameInfo.selectedPlayer2.Add(indexToAdd);
                selectedCharacters.Add(indexToAdd);
                selectedAmount--;
                
                GameObject obj = Instantiate(panelsRectTransform[indexToAdd], chosenPlayer2Panel.transform);
                obj.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
                
                if (selectedAmount == 0)
                {
                    DeactivateAllBut(-1);
                    StartGamePanel.SetActive(true);
                }
                else
                {
                    SwitchPlayer();
                }
            }
        }
        
    }
    
    private void SwitchPlayer()
    {
        isPlayer1 = !isPlayer1;
        isPlayer2 = !isPlayer2;
        nickText.text = isPlayer1 ? player1Nick + ",twoja kolej" : player2Nick + ", twoja kolej";
        DeactivateAllBut(-1);
    }

        
    private void DeactivateAllBut(int index)
    {

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
