using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundPMA : MonoBehaviour
{
    [Header("Sound to play for the attack.")]
    public AudioSource audS;
    [Header("Test the attack.")]
    public bool test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(test)
        {
            test = false;
            audS.volume = Random.Range(1f, 3f);
            audS.Play();//, new Vector3(0, 0, 0), 1f);
        }
    }

    // Call this function to use the PMA
    public void Attack()
    {
        test = true;
    }
}
