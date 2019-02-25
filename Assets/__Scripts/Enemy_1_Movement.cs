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
    }

    void Update()
    {
        Move();
    }
    public override void Move()
    {
        base.Move();
    }
}
