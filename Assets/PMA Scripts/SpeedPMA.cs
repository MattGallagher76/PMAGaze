using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpeedPMA : MonoBehaviour
{
    [Header("The game manager script to affect.")]
    public GameManager game;
    [Header("How long in seconds the attack should last.")]
    public int PMATime;
    [Range(0f, 2f)][Header("The percentage the lower and upper bounds of the speed should be changed by.")]
    public float rate;
    [Header("Test the attack.")]
    public bool test;
    private float endTime;
    private bool activeTest;
    private float[] initial;

    // Start is called before the first frame update
    void Start()
    {
        endTime = 0;
        activeTest = false;
        initial = new float[2];
        Array.Copy(game.switchSpeedRange, initial, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(activeTest && Time.time >= endTime)
        {
            activeTest = false;
            Array.Copy(initial, game.switchSpeedRange, 2);
        }
        if(test)
        {
            test = false;
            endTime = Time.time + PMATime;
            activeTest = true;
            game.switchSpeedRange[0] *= rate;
            game.switchSpeedRange[1] *= rate;
        }
    }

    // Call this function to use the PMA
    public void Attack()
    {
        test = true;
    }
}
