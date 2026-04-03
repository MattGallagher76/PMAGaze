using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startTrialButton : MonoBehaviour
{
    TrialManager tm;
    public bool DEBUG_Press;
    void Start()
    {
        tm = FindObjectOfType<TrialManager>();
    }

    private void Update()
    {
        if(DEBUG_Press)
        {
            tm.startTrial();
            DEBUG_Press = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.ToLower().Contains("controller"))
        {
            tm.startTrial();
        }
    }

}
