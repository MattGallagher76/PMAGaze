using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TrialManager : MonoBehaviour
{
    public XRRayInteractor rayInteractorRight;
    public XRRayInteractor rayInteractorLeft;
    public InputActionReference triggerLeft;
    public InputActionReference triggerRight;

    public int PMACount;

    public int currentPMA;
    //0 - baseline
    //1 - Constrast Change
    //2 - buffering
    //3 - speed up
    //4 - alter lighting
    //5 - new sounds
    //5 - Remove/delay sounds

    public int trialCount;
    public int currentTrialCount;
    public int baselineCount;
    public int userID;

    string dataLogFilePath;
    GameManager gm;
    int trialState = 0;
    
    void Start()
    {
        triggerRight.action.Enable();
        triggerLeft.action.Enable();
        gm = FindObjectOfType<GameManager>();
        Random.InitState(userID * 1367);
        dataLogFilePath = Path.Combine(Application.dataPath, "user" + userID + "trialStateLogging.csv");
        if (!File.Exists(dataLogFilePath))
        {
            File.WriteAllText(dataLogFilePath, "Time,UserID,Selected Cups,Correct Cups,Successful Trial,PMAState,PMAIntensity");
        }
    }

    public void startTrial()
    {
        gm.startTrialSequence();
        currentTrialCount++;

        //Selects PMA State
        if (currentTrialCount % (trialCount / baselineCount) == 0)
            currentPMA = 0;
        else
            currentPMA = UnityEngine.Random.Range(1, PMACount + 1);
    }

    public void updateTrialState(int i)
    {
        trialState = i;
    }

    public void confirmSelection()
    {
        trialState = 2;
        string selectedCups = "";
        foreach(Cup c in gm.getCups())
        {
            if (c.hasBeenSelected)
                selectedCups += c.id + "-";
        }
        string correctCups = "";
        foreach(Cup c in gm.getCups())
        {
            if (c.doesHaveBall)
                correctCups += c.id + "-";
        }
        string line = Time.time + "," + userID + "," + selectedCups + "," + (selectedCups.Equals(correctCups)) + "," + "TODO" + "TODO";
    }

    // Update is called once per frame
    void Update()
    {
        if(trialState == 1)
        {
            //Swaps have finished, open for selection

            //Left
            if(triggerLeft.action.ReadValue<float>() > 0.1f)
            {
                if(rayInteractorLeft.TryGetCurrent3DRaycastHit(out RaycastHit hit))
                {
                    if(hit.collider.gameObject.GetComponent<Cup>() != null)
                    {
                        hit.collider.gameObject.GetComponent<Cup>().hasBeenSelected = true;
                    }
                }
            }
            else if(triggerRight.action.ReadValue<float>() > 0.1f)
            {
                if (rayInteractorRight.TryGetCurrent3DRaycastHit(out RaycastHit hit))
                {
                    if (hit.collider.gameObject.GetComponent<Cup>() != null)
                    {
                        hit.collider.gameObject.GetComponent<Cup>().hasBeenSelected = true;
                    }
                }
            }
        }
    }
}
