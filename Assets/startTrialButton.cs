using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startTrialButton : MonoBehaviour
{
    TrialManager tm;
    void Start()
    {
        tm = FindObjectOfType<TrialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.ToLower().Contains("controller") && tm.trialState == 0)
        {
            tm.startTrial();
        }
    }

}
