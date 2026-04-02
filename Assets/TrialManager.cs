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
    public GameManager gm;
    public int trialState = 0;

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
        Debug.Log(gm == null);
        Debug.Log(gm.getCups() == null);
        Debug.Log(gm.getCups().Length);
        trialState = 3;

        string selectedCups = "";
        foreach (Cup c in gm.getCups())
        {
            if (c.hasBeenSelected)
                selectedCups += c.id + "-";
        }

        string correctCups = "";
        foreach (Cup c in gm.getCups())
        {
            if (c.doesHaveBall)
                correctCups += c.id + "-";
        }

        string line = Time.time + "," + userID + "," + selectedCups + "," + (selectedCups.Equals(correctCups)) + "," + "TODO" + "TODO";

        gm.destroyCups(selectedCups.Equals(correctCups));
        trialState = 0;
    }

    void Update()
    {
        if (trialState == 0)
        {
            if (DEBUG_TriggerPull || triggerLeft.action.ReadValue<float>() > 0.1f || triggerRight.action.ReadValue<float>() > 0.1f)
            {
                startTrial();
                DEBUG_TriggerPull = false;
            }
        }

        else if (trialState == 1)
        {
            if (UnityEngine.Random.Range(0f, 1f) < PMAFrequency)
            {
                switch (currentPMA)
                {
                    case 1: contrastPMA.Attack(); break;
                    case 2: bufferPMA.Attack(); break;
                    case 3: speedPMA.Attack(); break;
                    case 4: lightPMA.Attack(); break;
                    case 5: soundPMA.Attack(); break;
                }
            }
        }

        // trialState == 2 handled entirely by Cup collisions
    }
}