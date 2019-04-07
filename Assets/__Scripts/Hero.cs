using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S; // Singleton

    [Header("Set in Inspector")]
    // Variables to control ship movement
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    
    public float damageIncrement = 0.25f;
    public float speedIncrement = 5f;

    public static float projectileDamageStatic = 1; // used to store the damage done by the hero projectile

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 4;
    public float speed = 30;
    public float projectileDamage = 1;
    

    private GameObject _lastTriggerGo = null;
    private float _projectileDamageHolder; // used to keep track of the projectile damage
    private float _startingProjectileDamage; // used to reset the projectile damage 
    private float _startingSpeed; // used to reset the projectile damage

    // Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate(); 
    // Create a WeaponFireDelegate field named fireDelegate.
    public WeaponFireDelegate fireDelegate;

    void Awake() {
        if(S==null)
            S = this; // set singleton
        else
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");

        _projectileDamageHolder = projectileDamage; // initialize the projectile damage holder variable
        _startingProjectileDamage = projectileDamage;
        _startingSpeed = speed;
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
        {
            fireDelegate();
        }

        projectileDamageStatic = projectileDamage; // update the static projectile damage variable so it equals the local version
    }


    // Used to make sure the damage is updated after the weapon is switched
    void LateUpdate()
    {
        // Update the damage value of the projectile
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateProjectileDamage();
        }
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
            SoundManagerScript.PlaySound("damaged");
            Destroy(go);
            Score.AddScore(1); // One point is rewarded for destroying an enemy by crashing into it
        }

        // Absorb the power up upon collision
        else if(go.tag == "PowerUp")
        {
            AbsorbPowerUp(go);
        }
        else
            print("Triggered by non-enemy: " + go.name);
    }

    // Method to absorb the power up
    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.letter.text) {
            case "S":
                speed += speedIncrement; // increase the hero's speed
                SoundManagerScript.PlaySound("powerUp");
                break;
            case "D":
                _projectileDamageHolder += damageIncrement; // increase the hero's projectile damage
                UpdateProjectileDamage();
                SoundManagerScript.PlaySound("powerUp");
                break;
            default:
                shieldLevel--; // decrease the hero's shield level
                ResetSpeed(); // reset the hero's speed to its defaut value
                ResetProjectileDamage(); // reset the hero's projectile damage to its default
                UpdateProjectileDamage();
                SoundManagerScript.PlaySound("powerUpBad");
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }

    
    // Method to reset the hero's speed to its default value
    public void ResetSpeed()
    {
        speed = _startingSpeed;
    }

    // Method to reset the holder for the hero projectile's damage to its default value
    public void ResetProjectileDamage()
    {
        _projectileDamageHolder = _startingProjectileDamage;
    }

    // Method to update the hero's projectile damage whenever the weapon changes of a power-up is collected
    public void UpdateProjectileDamage()
    {
        projectileDamage = Weapon.damageMultiplierStatic * _projectileDamageHolder;
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
                ResetSpeed(); // reset the hero's speed to its defaut value
                ResetProjectileDamage(); // reset the holder for the hero's projectile damage to its default
                UpdateProjectileDamage(); // reset the hero's projectile damage to its default
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
