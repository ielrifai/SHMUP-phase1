using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0_Move : Enemy
{

    void Start()
    {
        y = -1;

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
