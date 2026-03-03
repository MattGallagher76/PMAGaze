using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public int horizontalCount;
    public int verticalCount;

    public float horizontalOffset;
    public float verticalOffset;

    public float[] switchSpeedRange;

    public GameObject swapObjectsOrigin;

    public GameObject cupPrefab;
    //public GameObject ballPrefab;

    Cup[] cupRegistry;

    public bool DEBUGMakeRandomSwap = false;

    Vector3 downEulerOffset = Vector3.zero;

    //A randomize list to order the swaps randomly
    List<int> swapList = new List<int>();

    public float trialDuration;
    public float[] timeBetweenSwapStartRange;

    public bool DEBUGShowMat;
    Cup ballCup;

    // Start is called before the first frame update
    void Start()
    {
        buildCups();
    }

    public void buildCups()
    {
        cupRegistry = new Cup[horizontalCount * verticalCount];
        for(int x = 0; x < horizontalCount; x ++)
        {
            for(int y = 0; y < verticalCount; y ++)
            {
                GameObject gb = Instantiate(cupPrefab);
                gb.transform.parent = swapObjectsOrigin.transform;

                float xPos = x - (horizontalCount - 1) / 2f;
                float yPos = y - (verticalCount - 1) / 2f;

                gb.transform.localPosition = new Vector3(xPos * horizontalOffset, yPos * verticalOffset, 0);
                gb.GetComponent<Cup>().initCup(x, y, y * horizontalCount + x, false);
                cupRegistry[y * horizontalCount + x] = gb.GetComponent<Cup>();
                swapList.Add(y * horizontalCount + x);
            }
        }

        ShuffleList(swapList);
        int ballIndex = UnityEngine.Random.Range(0, cupRegistry.Length);

        cupRegistry[ballIndex].doesHaveBall = true;
        ballCup = cupRegistry[ballIndex];
    }

    void ShuffleList(List<int> swapList)
    {
        for (int i = swapList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = swapList[i];
            swapList[i] = swapList[j];
            swapList[j] = temp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DEBUGMakeRandomSwap)
        {
            DEBUGMakeRandomSwap = false;
            //StartCoroutine(SwapCupsHalfCircle(UnityEngine.Random.Range(0, cupRegistry.Length),
            //    UnityEngine.Random.Range(0, cupRegistry.Length),
            //    UnityEngine.Random.Range(switchSpeedRange[0], switchSpeedRange[1])));
            startTrialSequence();
        }

        ballCup.showDebug(DEBUGShowMat);
    }

    public IEnumerator SwapCupsHalfCircle(int indexA, int indexB, float duration)
    {
        Transform a = cupRegistry[indexA].transform;
        Transform b = cupRegistry[indexB].transform;

        Vector3 a0 = a.position;
        Vector3 b0 = b.position;

        Vector3 center = (a0 + b0) * 0.5f;

        Vector3 v = b0 - a0;
        Vector3 worldZ = Vector3.forward;

        Vector3 n = Vector3.Cross(v, worldZ);

        if (n.sqrMagnitude < 1e-8f)
            n = Vector3.Cross(v, Vector3.up);

        if (n.sqrMagnitude < 1e-8f)
            n = Vector3.right;

        n.Normalize();

        Vector3 offsetA = a0 - center;
        Vector3 offsetB = b0 - center; // = -offsetA

        Quaternion down = Quaternion.Euler(downEulerOffset);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, duration);
            float u = Mathf.Clamp01(t);

            float angDeg = 180f * u;

            Vector3 aPos = center + RotateAroundAxis(offsetA, n, angDeg);
            Vector3 bPos = center + RotateAroundAxis(offsetB, n, angDeg);

            a.position = aPos;
            b.position = bPos;

            a.rotation = down;
            b.rotation = down;

            yield return null;
        }

        a.position = b0;
        b.position = a0;

        a.rotation = down;
        b.rotation = down;

        (cupRegistry[indexA], cupRegistry[indexB]) = (cupRegistry[indexB], cupRegistry[indexA]);

        swapList.Insert(Random.Range(0, swapList.Count + 1), indexA);
        swapList.Insert(Random.Range(0, swapList.Count + 1), indexB);
    }

    private static Vector3 RotateAroundAxis(Vector3 v, Vector3 axisUnit, float angleDeg)
    {
        return Quaternion.AngleAxis(angleDeg, axisUnit) * v;
    }

    public void startTrialSequence()
    {
        StartCoroutine(singleTrialSequence());
    }

    int PopRandomAvailable()
    {
        int k = Random.Range(0, swapList.Count);
        int index = swapList[k];
        swapList.RemoveAt(k);
        return index;
    }

    IEnumerator singleTrialSequence()
    {
        yield return new WaitForSeconds(2f);
        DEBUGShowMat = true;
        yield return new WaitForSeconds(2f);
        DEBUGShowMat = false;
        yield return new WaitForSeconds(2f);
        float t = 0f;
        while (t < trialDuration)
        {
            if (swapList.Count < 2)
            {
                yield return null;
                continue;
            }

            int indexA = PopRandomAvailable();
            int indexB = PopRandomAvailable();

            StartCoroutine(SwapCupsHalfCircle(
                indexA,
                indexB,
                Random.Range(switchSpeedRange[0], switchSpeedRange[1])
            ));
            float wait = Random.Range(timeBetweenSwapStartRange[0], timeBetweenSwapStartRange[1]);
            yield return new WaitForSeconds(wait);

            t += wait;
        }
        yield return new WaitForSeconds(2f);
        DEBUGShowMat = true;
        yield return new WaitForSeconds(2f);
        DEBUGShowMat = false;
        yield return new WaitForSeconds(2f);
    }

    public Cup[] getCups()
    {
        return cupRegistry;
    }
}
