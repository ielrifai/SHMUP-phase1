using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int score=0;// stores current score
    public static int highScore=0;// stores highscore

    // Updates the score variable 
    public static void AddScore(int points)
    {
        score += points;
      
    }

    // Updates the high score variable
    public static void AddHighScore(int high)
    {
        highScore= high;
    }

    // Resets the score to zero
    public static void ResetScore()
    {
        score = 0;
    }
}
