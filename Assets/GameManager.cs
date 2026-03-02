using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int horizontalCount;
    public int verticalCount;

    public float horizontalOffset;
    public float verticalOffset;

    public float[] switchSpeedRange;

    public GameObject swapObjectsOrigin;

    public GameObject cupPrefab;
    public GameObject ballPrefab;

    public Cup[] cupRegistry;

    public bool DEBUGMakeRandomSwap = false;

    Vector3 downEulerOffset = Vector3.zero;

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

                float xPos = - horizontalCount / 2f + x;
                float yPos = -verticalCount / 2f + y;

                gb.transform.localPosition = new Vector3(xPos * horizontalOffset, yPos * verticalOffset, 0);
                gb.GetComponent<Cup>().initCup(x, y, y * horizontalCount + x, false);
                cupRegistry[y * horizontalCount + x] = gb.GetComponent<Cup>();
            }
        }

        cupRegistry[UnityEngine.Random.Range(0, cupRegistry.Length)].doesHaveBall = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(DEBUGMakeRandomSwap)
        {
            DEBUGMakeRandomSwap = false;
            StartCoroutine(SwapCupsHalfCircle(UnityEngine.Random.Range(0, cupRegistry.Length),
                UnityEngine.Random.Range(0, cupRegistry.Length),
                UnityEngine.Random.Range(switchSpeedRange[0], switchSpeedRange[1])));
        }
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
    }

    private static Vector3 RotateAroundAxis(Vector3 v, Vector3 axisUnit, float angleDeg)
    {
        return Quaternion.AngleAxis(angleDeg, axisUnit) * v;
    }
}
