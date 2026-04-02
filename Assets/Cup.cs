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

    public bool hasBeenSelected = false;

    private float lastToggleTime = 0f;
    private float toggleCooldown = 0.25f;

    public void initCup(int x, int y, int id, bool hasBall)
    {
        this.id = id;
        xPos = x;
        yPos = y;
        doesHaveBall = hasBall;
        primaryMat = GetComponentInChildren<Renderer>().material;
    }

    public void showDebug(bool show)
    {
        if ((show && doesHaveBall) || hasBeenSelected)
            GetComponentInChildren<Renderer>().material = DEBUGShowHasBallMat;
        else
            GetComponentInChildren<Renderer>().material = primaryMat;
    }

    public void setBaselineMaterial(Material mat)
    {
        GetComponentInChildren<Renderer>().material = mat;
        primaryMat = mat;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only allow interaction during selection phase
        TrialManager tm = FindObjectOfType<TrialManager>();
        if (tm == null || tm.trialState != 2)
            return;

        // prevent rapid flickering toggles
        if (Time.time - lastToggleTime < toggleCooldown)
            return;

        // detect controller by rigidbody (your setup)
        if (other.attachedRigidbody != null)
        {
            hasBeenSelected = !hasBeenSelected;
            showDebug(hasBeenSelected);
            lastToggleTime = Time.time;
        }
    }
}