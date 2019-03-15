using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    public float x = 0;
    public float y = 0;



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

        if (_bndCheck != null && _bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }
    
    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.x += x * speed * Time.deltaTime;
        tempPos.y += y* speed * Time.deltaTime;
        pos = tempPos;
    }
    
}
