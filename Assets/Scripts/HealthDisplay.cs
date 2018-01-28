using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay: MonoBehaviour {

    public Slider healthSlider;
    public Image healthBarImage;
    public float maxHealth = 250f;

    public Color highHealthColor = Color.green;
    public Color mediumHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;

    public float redYellowCutoff;
    public float yellowGreenCutoff;

    public void Start()
    {
        redYellowCutoff = maxHealth * 0.3f;
        yellowGreenCutoff = maxHealth * 0.7f;

        if (healthSlider != null)
            healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void Update()
    {
        healthSlider.maxValue = maxHealth;
       
    }

    public void SetHealth(float newHealth)
    {
        healthSlider.value = newHealth;
        setSliderColor(newHealth);
    }

    private void setSliderColor(float newHealth)
    {
        if (newHealth <= redYellowCutoff)
        {
            healthBarImage.color = lowHealthColor;
        }
        else
        {
            healthBarImage.color = highHealthColor;
        }
    }

}
