using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseMapManager : MonoBehaviour
{
    [SerializeField] private GameObject kapitolMap;
    [SerializeField] private GameObject msMap;
    public void chooseKapitol()
    {
        GameInfo.map = kapitolMap;
        SceneManager.LoadScene("SelectionScene");
    }

    public void chooseMS()
    {
        GameInfo.msMap = true;
        GameInfo.map = msMap;
        SceneManager.LoadScene("SelectionScene");
    }

}
