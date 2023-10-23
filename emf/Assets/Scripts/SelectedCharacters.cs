using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SelectedCharacters : MonoBehaviour
{
    public List<int> selectedPlayer1 = new List<int>();
    public List<int> selectedPlayer2 = new List<int>();
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
