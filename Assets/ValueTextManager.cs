using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValueTextManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI valueText;
    private Slider slider;

    // Start is called before the first frame update
    void OnEnable() 
    {
        slider = FindFirstObjectByType<Slider>();
    }

    // Update is called once per frame
    public void ChangeValue()
    {
        valueText.text = "" + slider.value;
        
    }
}
