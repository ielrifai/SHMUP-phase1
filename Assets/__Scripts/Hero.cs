using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S; // Singleton

    [Header("Set in Inspector")]
    // Variables to control ship movement
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 4;


    private GameObject _lastTriggerGo = null;

    // Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate(); 
    // Create a WeaponFireDelegate field named fireDelegate.
    public WeaponFireDelegate fireDelegate;

    void Awake() {
        if(S==null)
            S = this; // set singleton
        else
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
    }

    // Update is called once per frame
    void Update()
    {
        // Retrieve user inputs
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        // Move the ship based on inputs
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        // Rotate the ship
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);


        // Shoot projectile
        if (Input.GetKeyDown(KeyCode.Space) && fireDelegate != null)
            fireDelegate();
    }


    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        // Make sure the same enemy doesn't cause multiple triggers
        if (go == _lastTriggerGo)
            return;

        _lastTriggerGo = go;

        // Decrease shield level and destroy enemy upon collision
        if(go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
            Score.AddScore(1); // One point is awarded for destroying an enemy by crashing into it
        }
        else
            print("Triggered by non-enemy: " + go.name);
    }

    // shieldLevel property
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }

        set
        {
            _shieldLevel = Mathf.Min(value, 4);

            // If the shield reaches below 0
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
