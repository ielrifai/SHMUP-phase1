using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int progressionScore = 0; // used to determine how many points are required to progress to Level 2

    public void Level1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Level2()
    {
        // The next level can only be reached if the required score is obtained on Level 1
        if(Score.highScoreLevel1 >= progressionScore)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}