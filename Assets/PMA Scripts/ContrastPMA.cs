using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrastPMA : MonoBehaviour
{
    [Header("The game manager script to affect.")]
    public GameManager game;
    [Header("How long in seconds the attack should last.")]
    public int PMATime;
    [Header("Prefab to spawn.")]
    public GameObject contrast;
    [Range(1f, 2f)][Header("The variation in size contrast objects should have. 1 means same size.")]
    public float rate;
    [Range(0, 75)][Header("The variation in color contrast objects should have. 0 means same color.")]
    public int color;
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
        rate -= 0.5f;
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
            Vector3 tempScale = new Vector3(clone.transform.localScale.x * (random.Next(50, (int)(rate * 100)) / 100f), clone.transform.localScale.y * (random.Next(50, (int)(rate * 100)) / 100f), clone.transform.localScale.z);
            clone.transform.localScale = tempScale;
            Color32 col = clone.GetComponent<Renderer>().material.color;
            clone.GetComponent<Renderer>().material.color = new Color32((byte)random.Next(col.r - color, col.r + color), (byte)random.Next(col.g - color, col.g + color), (byte)random.Next(col.b - color, col.b + color), col.a);
        }
    }

    // Call this function to use the PMA
    public void Attack()
    {
        test = true;
    }
}
