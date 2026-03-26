using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class BufferPMA : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("The game manager object to affect.")]
    public GameObject game;
    [Header("How long in seconds each frame should be paused for.")]
    public float frameTime;
    [Range(1, 5)][Header("The amount of buffered frames that should be shown.")]
    public int frames;
    [Header("Test the attack.")]
    public bool test;
    private float endTime;
    private int frameCount;
    private bool activeTest;
    private Transform clone;
    private MeshRenderer[] rend;
    void Start()
    {
        endTime = 0;
        frameCount = 0;
        activeTest = false;
        rend = game.GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(activeTest && Time.time >= endTime)
        {
            activeTest = false;
            Destroy(clone.gameObject);
            foreach (MeshRenderer mr in rend)
            {
                mr.enabled = true;
            }
            frameCount--;
            if (frameCount > 0)
            {
                test = true;
            }
        }
        if(!activeTest && test)
        {
            if (frameCount <= 0)
            {
                frameCount = frames;
            }
            test = false;
            endTime = Time.time + frameTime;
            activeTest = true;
            clone = Instantiate(game.transform.GetChild(0));
            foreach (MeshRenderer mr in rend)
            {
                mr.enabled = false;
            }
        }
    }

    // Call this function to use the PMA
    public void Attack()
    {
        test = true;
    }
}
