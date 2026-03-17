using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CosmeticSelector : MonoBehaviour
{
    public int cost;
    public GameObject lockedIndicator;
    public TextMeshProUGUI costText;
    CosmeticManager cosMang;

    public bool isLocked;

    private void Start()
    {
        cosMang = FindObjectOfType<CosmeticManager>();
    }

    public void onHoverStart()
    {
        if(cosMang.score < cost)
        {
            lockedIndicator.SetActive(true);
        }
    }

    public void onHoverEnd()
    {
        lockedIndicator.SetActive(false);
    }

    public void unlockItem(int index)
    {
        if(isLocked)
        {
            if (cosMang.score >= cost)
            {
                cosMang.unlockCupCosmetic(index);
                costText.text = "Unlocked";
                isLocked = false;
            }
            else
            {
                //Play bad noise
            }
        }
        else
        {
            cosMang.unlockCupCosmetic(index);
        }
    }
}
