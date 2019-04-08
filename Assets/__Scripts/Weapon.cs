using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is an enum of the various possible weapon types.
/// It also includes a "shield" type to allow a shield power-up.
/// Items marked [NI] below are Not Implemented in the IGDPD book.
/// </summary>
public enum WeaponType
{
    simple, // The default weapon
    blaster, // Shoots three bullets
    nuke,  // destroys all enemies on screen
    homing // targets the closest enemy
}


[System.Serializable] 
public class WeaponDefinition
{ 
    public WeaponType type = WeaponType.simple;
    public Color color = Color.white; // Color of Collar & power-up
    public GameObject projectilePrefab; // Prefab for projectiles
    public Color projectileColor = Color.white;
    public float multiplier = 1; // doesn't do anything right now
    public float delayBetweenShots = 0.3f;
    public float velocity = 50; // Speed of projectiles
}



public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;
    public float angle;

    public static float damageMultiplierStatic = 1;

    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.simple;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime; // Time last shot was fired
    public float lastHomingTime = 0; // Time the homing missile was last used
    public float lastNukeTime = 0; // Time the nuke was last used

    private Renderer collarRend;

    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        // Call SetType() for the default _type of WeaponType.simple
        SetType(_type);

        // Dynamically create an anchor for all Projectiles
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        // Find the fireDelegate of the root GameObject
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        { // d
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    // Switches the weapon types when 'E' is pressed
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_type == WeaponType.blaster)
            {
                SetType(WeaponType.homing);
            }

            else if (_type == WeaponType.homing)
            {
                SetType(WeaponType.nuke);
            }

            else if (_type == WeaponType.nuke)
            {
                SetType(WeaponType.simple);
            }

            else
            {
                SetType(WeaponType.blaster);
            }

            damageMultiplierStatic = def.multiplier;
        }
    }

    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;
        if (type == WeaponType.simple)
        {
            this.transform.localScale = new Vector3(0, 0, 0); // makes the weapon invisible
        }
        else if (type == WeaponType.blaster)
        {
            this.transform.localScale = new Vector3(1, 1, 1); // makes the blaster weapon visible 
        }

        else if (type == WeaponType.homing)
        {
            this.transform.localScale = new Vector3(1, 2, 1); // re-proportions the blaster weapon into the homing missile weapon
        }

        else
        {
            this.transform.localScale = new Vector3(2, 2, 2); // re-proportions the blaster weapon into the nuke weapon
        }

        def = Main.GetWeaponDefinition(wt);
        collarRend.material.color = def.color;
        lastShotTime = 0; // You can fire immediately after _type is set.
    }

    public void Fire()
    {
        // If this.gameObject is inactive, return
        if (!gameObject.activeInHierarchy) return;

        // If it hasn't been enough time between shots, return
        if (Time.time - lastShotTime < def.delayBetweenShots && (type == WeaponType.simple || type == WeaponType.blaster))
        { 
            return;
        }

        // If it hasn't been enough time between missie use, return
        if (Time.time - lastHomingTime < def.delayBetweenShots && type == WeaponType.homing)
        {
            return;
        }

        // If it hasn't been enough time between missie use, return
        if (Time.time - lastNukeTime < def.delayBetweenShots && type == WeaponType.nuke)
        {
            return;
        }

        Projectile p;
        Vector3 vel = Vector3.up * def.velocity; 

        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch (type)
        { 
            case WeaponType.blaster:
                p = MakeProjectile(); // middle projectile
                p.rigid.velocity = vel;
                p = MakeProjectile(); // right projectile
                p.transform.rotation = Quaternion.AngleAxis(30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile(); // left projectile
                p.transform.rotation = Quaternion.AngleAxis(-30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                SoundManagerScript.PlaySound("shoot");
                break;

            case WeaponType.nuke:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                SoundManagerScript.PlaySound("shoot");
                lastNukeTime = Time.time;
                break;

            case WeaponType.homing:
                p = MakeProjectile();
                setAngle();
                p.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                SoundManagerScript.PlaySound("shoot");
                lastHomingTime = Time.time;
                break;

            // for the simple weapon type
            default:      
                p = MakeProjectile();
                p.rigid.velocity = vel;
                SoundManagerScript.PlaySound("shoot");
                break;
        }
    }

    public Projectile MakeProjectile()
    { 
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true); 
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time; 
        return (p);
    }

    // Method to set the angle used by the homing missile
    public void setAngle()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // collect the enemies on screen
        float[] distances = new float[100];
        int count = 0;
        float closest = 10000;
        Vector3 closestEnemy = Hero.S.transform.position;

        // Determine and store the distance between the hero and the closest enemy
        foreach (GameObject go in enemies)
        {
            distances[count] = Vector3.Distance(Hero.S.transform.position, go.transform.position);

            if (closest > distances[count])
            {
                closest = distances[count];
                closestEnemy = go.transform.position;
            }
            count++;
        }
        // Calculate the angle that the homing missile travels at
        Vector3 targetDirection = closestEnemy - Hero.S.transform.position;
        angle = Vector3.Angle(targetDirection, Vector3.up);
        
        // Flip the angle if the closest enemy is on the left
        if(targetDirection.x < 0)
            angle *= -1;
    }
}
