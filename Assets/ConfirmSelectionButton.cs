using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmSelectionButton : MonoBehaviour
{
    TrialManager tm;

    public bool DEBUG_Press;

    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TrialManager>();
    }

    private void Update()
    {
        if(DEBUG_Press)
        {
            tm.confirmSelection();
            DEBUG_Press = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("controller"))
        {
            tm.confirmSelection();
        }
    }
}
