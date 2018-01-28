using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkieController : MonoBehaviour {

    public WalkieAnimator walkieAnimator;
    
    public Transform antennaTip;
    public bool on = false;
    public enum Team { RED, BLUE };
    public Team playerTeam = Team.RED;

    public float maxPower = 240f;
    public float power = 240f;
    public float powerLoss = 1f; // power lost per second
    public float damage = 30f; // damage done to playerController
    public float effectiveRange = 5f;

    private MeshRenderer[] meshRenderers;
    public Material onMaterial;
    public Material offMaterial;

    private float antennaLength;
    private bool previousOn = false;

    // Use this for initialization
    void Start () {
		meshRenderers = GetComponentsInChildren<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(on)
        {
            if (!previousOn)
            {
                foreach (MeshRenderer mesh in meshRenderers)
                {
                    mesh.material = onMaterial;
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
            }
        }

        if (power < 0)
        {
            power = 0;
        }

        previousOn = on;
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

            Debug.Log("antennaLength: " + antennaLength);
        }
    }
}
