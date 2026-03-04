using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundPMA : MonoBehaviour
{
    [Header("Sound to play for the attack.")]
    public AudioClip clip;
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
            AudioSource.PlayClipAtPoint(clip, new Vector3(0, 0, 0), 1f);
        }
    }
}
