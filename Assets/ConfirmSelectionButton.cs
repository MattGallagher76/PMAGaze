using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmSelectionButton : MonoBehaviour
{
    TrialManager tm;

    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TrialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("controller") && tm.trialState == 2)
        {
            tm.confirmSelection();
        }
    }
}
