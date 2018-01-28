using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BatteryDisplay : MonoBehaviour {

    public Slider batterySlider;
    public Image healthBarImage;

    public float maxValue = 100f;
    private Color fillColor = Color.yellow;

    // Use this for initialization
    void Start () {
        if (batterySlider != null)
            batterySlider.maxValue = maxValue;
	}

	
	// Update is called once per frame
	void Update () {
        if(batterySlider != null)
            batterySlider.maxValue = maxValue;
        healthBarImage.color = fillColor;
    }

    public void SetValue(float newValue)
    {
        batterySlider.value = newValue;
    }
}
