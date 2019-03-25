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
    none, // The default / no weapon
    blaster // A simple blaster
}


[System.Serializable] 
public class WeaponDefinition
{ 
    public WeaponType type = WeaponType.none;
    public string letter; // Letter to show on the power-up
    public Color color = Color.white; // Color of Collar & power-up
    public GameObject projectilePrefab; // Prefab for projectiles
    public Color projectileColor = Color.white;
    public float damageOnHit = 0; // Amount of damage caused
    public float continuousDamage = 0; // Damage per second (Laser)
    public float delayBetweenShots = 0;
    public float velocity = 20; // Speed of projectiles
}


public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime; // Time last shot was fired

    private Renderer collarRend;

    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();
        
        // Call SetType() for the default _type of WeaponType.none
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
                _type = WeaponType.none;
                SetType(_type);
            }

            else
            {
                _type = WeaponType.blaster;
                SetType(_type);

            }
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
        if (type == WeaponType.none)
        { 
            this.transform.localScale = new Vector3(0, 0, 0); // makes the weapon invisible
            return;
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1); // makes the weapon visible
        }

        def = Main.GetWeaponDefinition(_type); 
        collarRend.material.color = def.color;
        lastShotTime = 0; // You can fire immediately after _type is set. 
    }

    public void Fire()
    {
        // If this.gameObject is inactive, return
        if (!gameObject.activeInHierarchy) return;

        // If it hasn't been enough time between shots, return
        if (Time.time - lastShotTime < def.delayBetweenShots)
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
            case WeaponType.none:
                p = MakeProjectile(); 
                p.rigid.velocity = vel;
                break;

            case WeaponType.blaster:
                p = MakeProjectile(); // middle projectile
                p.rigid.velocity = vel;
                p = MakeProjectile(); // right projectile
                p.transform.rotation = Quaternion.AngleAxis(30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile(); // left projectile
                p.transform.rotation = Quaternion.AngleAxis(-30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
        }
    }

    public Projectile MakeProjectile()
    { 
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);

        if (transform.parent.gameObject.tag == "Hero")
        { 
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true); // o
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time; // p
        return (p);
    }
}
