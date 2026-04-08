using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CosmeticManager : MonoBehaviour
{
    public int score = 10;

    [Header("Base Cup Materials")]
    public Material baseMaterial;
    public Material firstCosmetic;
    public Material secondCosmetic;
    public Material finalCosmetic;

    public int selectedMaterial = 0;

    GameManager gm;

    public int DEBUGUNLOCK;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(DEBUGUNLOCK != 0)
        {
            unlockCupCosmetic(DEBUGUNLOCK - 1);
            DEBUGUNLOCK = 0;
        }
    }

    public void unlockCupCosmetic(int index)
    {
        selectedMaterial = index;
        Cup[] cList = gm.getCups();
        if(cList != null)
        {
            foreach (Cup c in cList)
            {
                c.setBaselineMaterial(index == 0 ? baseMaterial : index == 1 ? firstCosmetic : index == 2 ? secondCosmetic : finalCosmetic);
            }
        }
    }

    public void setCupMat(int index, Cup c)
    {
        c.setBaselineMaterial(index == 0 ? baseMaterial : index == 1 ? firstCosmetic : index == 2 ? secondCosmetic : finalCosmetic);
    }
}
