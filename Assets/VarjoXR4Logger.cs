using UnityEngine; //Shoutout Nikki and Gio
using System.IO;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI;
using Varjo.XR;
using static Varjo.XR.VarjoEyeTracking;
using System.Collections.Generic;


public class EyeTrackingManager : MonoBehaviour
{
    private VarjoEyeTracking.GazeData gazeData;

    [SerializeField]
    private Camera headPosition;

    [Header("ENTER PARTICIPANT ID (Sxxx)")]
    [SerializeField]
    private string participantID;

    private string filePath;

    //VARJO GAZE VARIABLES
    long frameNumber, captureTime, deltaTime;
    long prevCaptureTime = -1;
    GazeStatus status;
    GazeRay gaze;
    float focusDistance, focusStability;
    GazeEyeStatus leftStatus, rightStatus;
    GazeRay left, right;


    private Slider slider;
    private Button button;

    TrialManager tm;

    private List<VarjoEyeTracking.GazeData> gazeDataList = new List<VarjoEyeTracking.GazeData>();

    void Start()
    {
        if(IsGazeAllowed() && IsGazeAvailable())
        {
            //SetGazeOutputFrequency(GazeOutputFrequency.Frequency200Hz);
            SetFilePath("GazePilot");
        }
        tm = FindObjectOfType<TrialManager>();
    }

    private void FixedUpdate()
    {
        if (IsGazeAllowed() && IsGazeAvailable())
        {
            int sampleCount = VarjoEyeTracking.GetGazeList(out gazeDataList);

            foreach (var sample in gazeDataList)
            {
                //Debug.Log($"Time: {sample.captureTime}, Status: {sample.status}, Gaze: {sample.gaze.forward}");
                WriteGaze(sample);
                //Debug.Log(sampleCount);
            }
        }
    }

    void Update()
    {

        if (button == null)
        {
            button = FindAnyObjectByType<Button>();
        }
        if (slider == null)
        {
            slider = FindAnyObjectByType<Slider>();
        }
    }

    private void InitializeGazeCSV()
    {
        TextWriter textWriter = new StreamWriter(filePath, false);
        textWriter.WriteLine("unityTime,frameNumber" + ","
                + "captureTime,deltaTime" + ","
                + "status" + ","
                + "gaze.forward.x,gaze.forward.y,gaze.forward.z,"
                + "gaze.origin.x,gaze.origin.y,gaze.origin.z,"
                + "focusDistance,focusStability,"
                + "leftStatus,rightStatus,"
                + "left.forward.x,left.forward.y,left.forward.z,"
                + "left.origin.x,left.origin.y,left.origin.z,"
                + "right.forward.x,right.forward.y,right.forward.z,"
                + "right.origin.x,right.origin.y,right.origin.z,TrialFinished");
        textWriter.Close();
    }

    public void WriteGaze(VarjoEyeTracking.GazeData gazeData)
    {
        return;
        frameNumber = gazeData.frameNumber;
        captureTime = gazeData.captureTime;
        if (prevCaptureTime == -1) {
            deltaTime = 0;
            prevCaptureTime = captureTime;
        } else {
            deltaTime = captureTime - prevCaptureTime;
            prevCaptureTime = captureTime;
        }
        status = gazeData.status;
        gaze = gazeData.gaze;
        focusDistance = gazeData.focusDistance;
        focusStability = gazeData.focusStability;
        leftStatus = gazeData.leftStatus;
        rightStatus = gazeData.rightStatus;
        left = gazeData.left;
        right = gazeData.right;

        if (tm.IsInvoking("confirmSelection"))
        {
            Debug.Log("E marked on this frame in GazeLogging script!");
            WriteToCSVWithMarker(gazeData, 'E');
        }
        else if (tm.IsInvoking("startTrial"))
        {
            Debug.Log("S marked on this frame in GazeLogging script!");
            WriteToCSVWithMarker(gazeData, 'S');
        }
        else
        {
            WriteToCSV(gazeData);
        }
        

    }

    private void WriteToCSV(VarjoEyeTracking.GazeData gazeData)
    {
        using (var textWriter = new StreamWriter(filePath, true))
        {
            textWriter.WriteLine(DateTime.UtcNow + "," + frameNumber + ","
                + captureTime + "," + deltaTime + ","
                + status + ","
                + gaze.forward.x + "," + gaze.forward.y + "," + gaze.forward.z + ","
                + gaze.origin.x + "," + gaze.origin.y + "," + gaze.origin.z + ","
                + focusDistance + "," + focusStability + ","
                + leftStatus + "," + rightStatus + ","
                + left.forward.x + "," + left.forward.y + "," + left.forward.z + ","
                + left.origin.x + "," + left.origin.y + "," + left.origin.z + ","
                + right.forward.x + "," + right.forward.y + "," + right.forward.z + ","
                + right.origin.x + "," + right.origin.y + "," + right.origin.z + "," + " ");
        }
    }

    public void SetFilePath(string scenario)
    {
        Debug.Log("Participant ID: " + participantID);

        int count = 0;
        string directoryName = Application.dataPath + "/ExportData/" + participantID;
        string baseDirectory = directoryName;

        if (Directory.Exists(directoryName))
        {
            string baseFile = baseDirectory + "/" + scenario + "_" + "gaze_data.csv";

            if (File.Exists(baseFile))
            {
                while (Directory.Exists(directoryName))
                {
                    count++;
                    directoryName = baseDirectory + "_" + count.ToString();
                }
            }
        }

        Directory.CreateDirectory(directoryName);

        filePath = directoryName + "/" + scenario + "_" + "gaze_data.csv";

        Debug.Log("File path: " + filePath);

        InitializeGazeCSV();
    }

    private int DetermineParticipantID(string scenario)
    {
        string[] participantDirectories = Directory.GetDirectories(Application.dataPath + "/ExportData/");
        int lastParticipantNumber = 0;

        if (participantDirectories.Length != 0)
        {
            string lastParticipantDirectory = participantDirectories[participantDirectories.Length - 1];
            string[] pathPieces = lastParticipantDirectory.Split('/');
            lastParticipantNumber = Int32.Parse(pathPieces[pathPieces.Length - 1]);

            string[] participantFiles = Directory.GetFiles(lastParticipantDirectory);

            return participantFiles.Any(substring => substring.Contains(scenario)) ? lastParticipantNumber + 1 : lastParticipantNumber;
        }
        else
        {
            return lastParticipantNumber;
        }
    }

    private void WriteToCSVWithMarker(VarjoEyeTracking.GazeData gazeData, char marker)
    {
        using (var textWriter = new StreamWriter(filePath, true))
        {
            textWriter.WriteLine(Time.time + frameNumber + ","
                            +captureTime + ","
                            + status + ","
                            + gaze.forward.x + "," + gaze.forward.y + "," + gaze.forward.z + ","
                            + gaze.origin.x + "," + gaze.origin.y + "," + gaze.origin.z + ","
                            + focusDistance + "," + focusStability + ","
                            + leftStatus + "," + rightStatus + ","
                            + left.forward.x + "," + left.forward.y + "," + left.forward.z + ","
                            + left.origin.x + "," + left.origin.y + "," + left.origin.z + ","
                            + right.forward.x + "," + right.forward.y + "," + right.forward.z + ","
                            + right.origin.x + "," + right.origin.y + "," + right.origin.z + "," + marker);
        }
    }
}
