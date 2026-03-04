using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPMA : MonoBehaviour
{
    [Header("Light to use for the attack.")]
    public Light PMALight;

    [Range(0f, 1f)] [Header("How dark the light should become. Lower = darker.")]

    public float PMAIntensity;
    [Header("How long in seconds the attack should last.")]
    public int PMATime;
    [Header("Click to test the attack.")]

    public bool PMATest;
    private float initialIntensity;
    private float endTime;
    private bool activeTest;

    // Start is called before the first frame update
    void Start()
    {
        initialIntensity = PMALight.intensity;
        endTime = 0;
        activeTest = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(activeTest && Time.time >= endTime)
        {
            activeTest = false;
            PMALight.intensity = initialIntensity;
        }
        if(PMATest)
        {
            PMATest = false;
            endTime = Time.time + PMATime;
            activeTest = true;
            PMALight.intensity = PMAIntensity;
        }
    }
}
