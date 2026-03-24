using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ContrastPMA : MonoBehaviour
{
    [Header("The game manager script to affect.")]
    public GameManager game;
    [Header("How long in seconds the attack should last.")]
    public int PMATime;
    [Header("Prefab to spawn.")]
    public GameObject contrast;
    [Header("Test the attack.")]
    public bool test;
    private float endTime;
    private bool activeTest;
    private Cup[] cups;
    private Transform target;
    private GameObject clone;
    // Start is called before the first frame update
    void Start()
    {
        endTime = 0;
        activeTest = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(activeTest && Time.time >= endTime)
        {
            activeTest = false;
            Destroy(clone);
        }
        if(!activeTest && test)
        {
            test = false;
            cups = game.getCups();
            System.Random random = new System.Random();
            endTime = Time.time + PMATime;
            activeTest = true;
            target = cups[random.Next(cups.Length)].transform;
            clone = Instantiate(contrast, target.position, target.rotation);
        }
    }

    // Call this function to use the PMA
    public void Attack()
    {
        test = true;
    }
}
