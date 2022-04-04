using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public IntegerReference boostAmount;
    public Slider boostSlider;
    public TMPro.TextMeshProUGUI boostAmountText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.boostSlider.value = this.boostAmount.Value;
        this.boostAmountText.text = boostAmount.Value + "%";
    }
}
