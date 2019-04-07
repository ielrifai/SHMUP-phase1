using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Declare the variables
    
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 0f;
    public int points = 0; // used to calculate score
    public float x = 0;
    public float y = 0;
    public bool notifiedOfDestruction = false;
    public bool collided = false; // used to prevent multiple collisions



    private BoundsCheck _bndCheck;

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();

    }

    public Vector3 pos
    {

        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }


    void Update()
    {
        Move();

        // Destroy the enemy
        if (_bndCheck != null && _bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        collided = false;
    }
    
    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.x += x * speed * Time.deltaTime;
        tempPos.y += y* speed * Time.deltaTime;
        pos = tempPos;
    }

    // Used to set the enemy's health and the points rewarded for destroying them
    public void SetHealth(float x) 
    {
        health = x;
        points = (int)x/2 - 1;
    }


    // Allows the enemy to take damage
    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;

        // if condition makes sure the same collision isn't registered multiple times (might not work properly???)
        if (collided == false)
        {
            // the blaster weapon may hit the same target multiple times, so it should be allowed to register multiple collisions (it should ALWAYS have a damage multiplier of 1)
            // the missile weapon seems to collide multiple times, but since it has a higher damage multiplier, this statement will execute and it should only register one collision
            if(Weapon.damageMultiplierStatic != 1)
                collided = true;

            // Handle a collision with the Nuke weapon
            if (otherGO.tag == "Nuke")
            {
                Destroy(otherGO);
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject go in enemies)
                {
                    // Tell main that the ship was destroyed
                    if (!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    Destroy(go);
                    SoundManagerScript.PlaySound("nuke");
                }
            }

            // Handle a collision with the regular weapon
            else if (otherGO.tag == "ProjectileHero")
            {
                Destroy(otherGO);
                health -= Hero.projectileDamageStatic;
                if (health <= 0)
                {
                    // Tell main that the ship was destroyed
                    if (!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    if (points != 10) // if condition is used to fix a bug
                        Score.AddScore(points);
                    Destroy(gameObject);
                    SoundManagerScript.PlaySound("point");
                }
            }

            else
                print("Enemy hit by a non-Projectile Hero: " + otherGO.name);
        }
    }
}
