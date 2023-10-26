using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo
{
    public static bool player1Won = false;
    public static bool player2Won = true;

    public static string player1Nick = "Player1";
    public static string player2Nick = "Player2";

    public static int charactersAmount = 4;
    public static List<int> selectedPlayer1 = new List<int>();
    public static List<int> selectedPlayer2 = new List<int>();
    
}


