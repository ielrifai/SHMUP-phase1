using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Movement : Enemy
{
    
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
    }

    // Overrides the Move method in the base class
    public override void Move()
    {
        base.Move();
    }
}
