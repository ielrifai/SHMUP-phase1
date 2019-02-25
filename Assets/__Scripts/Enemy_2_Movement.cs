using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2_Movement : Enemy
{
   
    float counter = 0;
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
        counter += 6;
        x = 7 * (Mathf.Sin((counter * Mathf.PI) / 180));
        base.Move();
    }
}
