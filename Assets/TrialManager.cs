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

    public ContrastPMA contrastPMA;
    public BufferPMA bufferPMA;
    public SpeedPMA speedPMA;
    public LightPMA lightPMA;
    public SoundPMA soundPMA;

    public float PMAFrequency;

    public int trialCount;
    public int currentTrialCount;
    public int baselineCount;
    public int userID;

    string dataLogFilePath;
    GameManager gm;
    int trialState = 0;

    public bool DEBUG_TriggerPull;
    public int cupCount;
    
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
        gm.ballCupCount = cupCount;
        gm.buildCups();
        gm.startTrialSequence();
        currentTrialCount++;

        //Selects PMA State
        if (currentTrialCount % (trialCount / baselineCount) == 0)
            currentPMA = 0;
        else
            currentPMA = UnityEngine.Random.Range(1, PMACount + 1);
        Debug.Log("Current PMA: " + currentPMA);
    }

    public void updateTrialState(int i)
    {
        trialState = i;
    }

    public void confirmSelection()
    {
        trialState = 3;
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
        gm.destroyCups(selectedCups.Equals(correctCups));
        trialState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(trialState == 0)
        {
            if(DEBUG_TriggerPull || triggerLeft.action.ReadValue<float>() > 0.1f || triggerRight.action.ReadValue<float>() > 0.1f)
            {
                startTrial();
                DEBUG_TriggerPull = false;
            }
        }
        if(trialState == 1)
        {
            //Conduct attacks
            if (UnityEngine.Random.Range(0f, 1f) < PMAFrequency)
            {
                switch (currentPMA)
                {
                    case 1:
                        contrastPMA.Attack();
                        break;
                    case 2:
                        bufferPMA.Attack();
                        break;
                    case 3:
                        speedPMA.Attack();
                        break;
                    case 4:
                        lightPMA.Attack();
                        break;
                    case 5:
                        soundPMA.Attack();
                        break;
                    case 0:
                        break;
                    default:
                        break;
                }
            }

        }
        if(trialState == 2)
        {
            //Swaps have finished, open for selection

            //Left
            if(triggerLeft.action.ReadValue<float>() > 0.1f)
            {
                RaycastHit hit;
                if (Physics.Raycast(rayInteractorLeft.gameObject.transform.position, rayInteractorLeft.gameObject.transform.forward, out hit, Mathf.Infinity))
                {
                    Cup c = hit.collider.gameObject.GetComponent<Cup>();
                    Debug.Log(c.id);
                    if (c != null)
                    {
                        Debug.Log(c.id);
                        c.hasBeenSelected = !c.hasBeenSelected;
                        c.showDebug(c.hasBeenSelected);
                    }
                }
            }
            else if(triggerRight.action.ReadValue<float>() > 0.1f)
            {
                RaycastHit hit;
                if (Physics.Raycast(rayInteractorRight.gameObject.transform.position, rayInteractorLeft.gameObject.transform.forward, out hit, Mathf.Infinity))
                {
                    Cup c = hit.collider.gameObject.GetComponent<Cup>();
                    Debug.Log(c.id);
                    if (c != null)
                    {
                        Debug.Log(c.id);
                        c.hasBeenSelected = !c.hasBeenSelected;
                        c.showDebug(c.hasBeenSelected);
                    }
                }
            }
        }
    }
}
