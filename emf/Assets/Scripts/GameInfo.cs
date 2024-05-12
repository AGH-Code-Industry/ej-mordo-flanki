using System.Collections.Generic;
using UnityEngine;

public static class GameInfo
{
    public static bool player1Won = false;
    public static bool player2Won = true;
    public static bool msMap = false;
    private static string player1Nick = "Player1";
    private static string player2Nick = "Player2";

    public static readonly int teamSize = 4;
    public static List<int> selectedPlayer1 = new List<int>();
    public static List<int> selectedPlayer2 = new List<int>();
    public static GameObject map;

    public static void setPlayer1Nick(string nick)
    {
        player1Nick = nick;
    }
    
    public static void setPlayer2Nick(string nick)
    {
        player2Nick = nick;
    }

    public static string getPlayer1Nick()
    {
        return player1Nick;
    }
    
    public static string getPlayer2Nick()
    {
        return player2Nick;
    }
    
}


