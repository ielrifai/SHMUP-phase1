using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Movement : Enemy
{
    private BoundsCheck _boundCheck;
    private bool _hitSideBound = false;
    private float _lastCollidedTime;


    void Awake()
    {
        _boundCheck = GetComponent<BoundsCheck>();

    }

    void Start()
    {
        y = -1;
        Random rnd = new Random();
        x = Random.Range(0, 2);
        if (System.Math.Abs(x) < 1)
        {
            x = -1;
        }
        this.SetHealth(7);
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        // Allows the ship to reverse directions multiple times
        if (Time.time - _lastCollidedTime > 1)
            _hitSideBound = false;
    }

    // Overrides the Move method in the base class
    public override void Move()
    {
        base.Move();

        // Reverse the ship's direction if it hits a side boundary
        if ((_boundCheck.offLeft || _boundCheck.offRight) && _hitSideBound == false)
        {
            int rand = Random.Range(1, 3); // used to randomize if the enemy is destroyed or changes direction

            // destroy the enemy
            if (rand == 1)
                Destroy(gameObject);

            // reverse the enemy's direction
            else
            {
                x *= -1;
                _hitSideBound = true;
                _lastCollidedTime = Time.time;
            }
        }
    }
}
