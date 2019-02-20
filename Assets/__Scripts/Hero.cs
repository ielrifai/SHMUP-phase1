﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S; // Singleton

    [Header("Set in Inspector")]
    // Variables to control ship movement
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    void Awake() {
        if(S==null)
            S = this; // set singleton
        else
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
    }

    // Update is called once per frame
    void Update()
    {
        // Retrieve user inputs
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        // Move the ship based on inputs
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        // Rotate the ship
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

    }
}