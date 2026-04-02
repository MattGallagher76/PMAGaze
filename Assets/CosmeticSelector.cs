using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CosmeticSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int cost;
    public GameObject lockedIndicator;
    public TextMeshProUGUI costText;
    public bool isLocked;

    private CosmeticManager cosMang;

    private void Start()
    {
        cosMang = FindObjectOfType<CosmeticManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverStart();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverEnd();
    }

    public void OnHoverStart()
    {
        if (isLocked && cosMang.score < cost)
        {
            lockedIndicator.SetActive(true);
        }
    }

    public void OnHoverEnd()
    {
        lockedIndicator.SetActive(false);
    }

    public void UnlockItem(int index)
    {
        if (isLocked)
        {
            if (cosMang.score >= cost)
            {
                cosMang.unlockCupCosmetic(index);
                costText.text = "Unlocked";
                isLocked = false;
            }
            else
            {
                // Play bad noise
            }
        }
        else
        {
            cosMang.unlockCupCosmetic(index);
        }
    }
}