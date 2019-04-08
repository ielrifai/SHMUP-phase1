using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public static int score=0;// stores current score
    public static int highScoreLevel1=0;// stores high score for level 1
    public static int highScoreLevel2 = 0;// stores high score for level 2

    // Updates the score variable 
    public static void AddScore(int point)
    {
        score += point;
    }

    // Updates the high score variable
    public static void ChangeHighScore(int high)
    {
        if (Application.loadedLevelName == "Level1")
            highScoreLevel1 = high;

        if (Application.loadedLevelName == "Level2")
            highScoreLevel2 = high;
    }

    // Retrieve the high score for the current scene
    public static int GetHighScore()
    {
        if (Application.loadedLevelName == "Level1")
            return highScoreLevel1;

        else if (Application.loadedLevelName == "Level2")
            return highScoreLevel2;

        else
            return -1; // used for debugging
    }

    // Resets the score to zero
    public static void ResetScore()
    {
        score = 0;
    }
}
