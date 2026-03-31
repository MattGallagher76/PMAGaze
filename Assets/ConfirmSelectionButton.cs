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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Contains("controller"))
        {
            tm.confirmSelection();
        }
    }
}
