using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_4 : Enemy
{
    [System.Serializable]
    public class Part
    {
        public string name;
        public float health;
        public int partPoints;
        public string[] protectedBy;

        [HideInInspector]
        public GameObject go;
        [HideInInspector]
        public Material mat;
    }

    [Header("Set in Inspector: Enemy_4")]
    public Part[] parts;

    private Vector3 _p0, _p1;
    private float timeStart;
    private float duration = 4;

    void Start()
    {
        y = -1;
        _p0 = _p1 = pos;

        InitMovement();

        Transform t;
        foreach( Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    // Overrides the Move method in the mase class
    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * _p0 + u * _p1;
    }
    Part FindPart(string n)
    {
        foreach (Part prt in parts)
        {
            if (prt.name == n)
            {
                return (prt);
            }
        }
        return (null);
    }
    Part FindPart(GameObject go)
    {
        foreach (Part prt in parts)
        {
            if (prt.go == go)
            {
                return (prt);
            }
        }
        return null;
    }
    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }

    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }

    bool Destroyed(Part prt)
    {
        if (prt == null)
        {
            return (true);
        }
        return (prt.health <= 0);
    }

    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.green;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }

    void InitMovement()
    {
        _p0 = _p1;

        float widMinRad = _bndCheck.camWidth - _bndCheck.radius;
        float hgtMinRad = _bndCheck.camHeight - _bndCheck.radius;
        _p1.x = Random.Range(-widMinRad, widMinRad);
        _p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        timeStart = Time.time;
    }
    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                // If this Eneny is off screen, don't damage it.
                if (!_bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }
                // hurt this enemy
                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if (prtHit == null)
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                if (prtHit.protectedBy != null)
                {
                    foreach (string s in prtHit.protectedBy) {
                        if (!Destroyed(s))
                        {
                            Destroy(other);
                            return;
                        }
                    }
                }


                prtHit.health -= Hero.projectileDamageStatic;

                ShowLocalizedDamage(prtHit.mat);

                if (prtHit.health <= 0)
                {
                    prtHit.go.SetActive(false);
                    prtHit.mat.color = Color.green;
                    Score.AddScore(prtHit.partPoints);
                    SoundManagerScript.PlaySound("point");
                }

                bool allDestroyed = true;
                foreach (Part prt in parts)
                {
                    if (!Destroyed(prt))
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if (allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }


    }
}

