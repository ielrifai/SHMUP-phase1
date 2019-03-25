using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0_Move : Enemy
{

    void Start()
    {
        y = -1;
        this.SetHealth(5);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    // Overrides the Move method from the base class
    public override void Move()
    {
        base.Move();
    }

}
