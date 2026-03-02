using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    public int id;

    public int xPos;
    public int yPos;

    public bool doesHaveBall = false;

    public Material DEBUGShowHasBallMat;
    Material primaryMat;

    public void initCup(int x, int y, int id, bool hasBall)
    {
        this.id = id;
        xPos = x;
        yPos = y;
        doesHaveBall = hasBall;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
