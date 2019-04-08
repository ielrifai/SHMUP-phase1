using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT; 

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultpadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public Text scoreText;
    public Text highScoreText;
    public int pUpDropChance = 15;

    private BoundsCheck _bndCheck;
    public static int powerUpDropChanceStatic = 15; // used to store the chance of a powerup dropping

    public void ShipDestroyed(Enemy e)
    {
        int rand = Random.Range(1, 100);

        // Drop a powerup if the conditions are met
        if(rand <= pUpDropChance)
        {
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(rand);
            pu.transform.position = e.transform.position;
        }
    }
    

    //Called when the game starts
    void Awake()
    {
        S = this;

        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }

        UpdateHighScore();// displays high score
    }

    // Spawns enemies
    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        float enemyPadding = enemyDefaultpadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;
        float xMin = -_bndCheck.camWidth + enemyPadding;
        float xMax =  _bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = _bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay); // Invoke the Restart() method in delay seconds
    }

    public void Restart()
    {
        SceneManager.LoadScene("Menu");

        // Update the high score
        if (Score.score > Score.GetHighScore())
            Score.ChangeHighScore(Score.score);

        Score.ResetScore(); // set the score to zero
    }

    
    //Get a WeaponDefinition from WEAP_DICT
    static public WeaponDefinition GetWeaponDefinition(WeaponType weaponType)
    {
        if (WEAP_DICT.ContainsKey(weaponType))
            return (WEAP_DICT[weaponType]);
        return (new WeaponDefinition());  
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + Score.score.ToString(); // displays the score 
    }

    public void UpdateHighScore()
    {
         highScoreText.text = "High Score: " + Score.GetHighScore().ToString();// displays the high score 
    }

    public void Update()
    {
        UpdateScore();// displays current score
        powerUpDropChanceStatic = pUpDropChance;
    }
}
