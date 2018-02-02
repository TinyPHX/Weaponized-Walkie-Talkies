using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkieController : MonoBehaviour {

    [Header(" --- EFFECTS --- ")]
    public Effect walkieOn;
    public Effect walkieOff;
    public Effect batteryDead;
    public Effect batteryFull;
    
    [Header(" --- OTHER STUFF --- ")]
    public WalkieAnimator walkieAnimator;
    
    public Transform antennaTip;
    public bool on = false;
    public enum Team { RED, BLUE };
    public Team playerTeam = Team.RED;

    public float maxPower = 240f;
    public float power = 240f;
    public float powerLoss = 5f; // power lost per second
    public float powerGain = 5f; // power lost per second

    private MeshRenderer[] meshRenderers;
    public Material onMaterial;
    public Material offMaterial;

    private float antennaLength;
    private bool previousOn = false;
    private float previousPower;

    // Use this for initialization
    void Start () {
		meshRenderers = GetComponentsInChildren<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (power <= 0 && on)
        {
            on = false;
        }

		if(on)
        {
            if (!previousOn)
            {
                foreach (MeshRenderer mesh in meshRenderers)
                {
                    mesh.material = onMaterial;
                }

                if (walkieOn != null)
                {
                    walkieOn.PrefabPlay();
                }
            }

            if (power > 0)
            {
                power -= powerLoss * Time.deltaTime;
            }
        }
        else
        {
            if (previousOn)
            {
                foreach (MeshRenderer mesh in meshRenderers)
                {
                    mesh.material = offMaterial;
                }

                if (walkieOff != null)
                {
                    walkieOff.PrefabPlay();
                }
            }

            if (power < maxPower)
            {
                power += powerGain * Time.deltaTime;
            }
        }

        if (power < 0)
        {
            power = 0;
        }

        if (power >= maxPower)
        {
            power = maxPower;
        }

        if (previousPower != power)
        {
            if (power <= 0)
            {
                if (batteryDead != null)
                {
                    batteryDead.PrefabPlay();
                }
            }

            if (power == maxPower)
            {
                if (batteryFull != null)
                {
                    batteryFull.PrefabPlay();
                }
            }
        }

        previousOn = on;
        previousPower = power;
	}
    
    public float AntennaLength
    {
        get
        {
            return antennaLength;
        }

        set
        {
            antennaLength = value;
            walkieAnimator.AntennaLength = value;
        }
    }
}
