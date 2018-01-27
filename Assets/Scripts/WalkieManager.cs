using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkieManager : MonoBehaviour {

    public List<WalkieController> walkieControllerList = new List<WalkieController>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (WalkieController wc1 in walkieControllerList)
        {
            foreach (WalkieController wc2 in walkieControllerList)
            {
                if (wc1 != wc2 && wc1.playerTeam == wc2.playerTeam && wc1.on == true && wc2.on == true && wc1.power > 0 && wc2.power > 0)
                {
                    Transform at1 = wc1.antennaTip;
                    Transform at2 = wc2.antennaTip;
                    float antennaDistance = Vector3.Distance(at1.position, at2.position);
                    float minimumEffectiveRange = Mathf.Min(wc1.effectiveRange, wc2.effectiveRange);

                    if (antennaDistance <= minimumEffectiveRange) // if walkie talkies are within the minimum of their two effective ranges
                    {
                        RaycastHit[] hits;
                        hits = Physics.RaycastAll(at1.transform.position, at2.transform.position - at1.transform.position, Vector3.Distance(at1.transform.position, at2.transform.position)); // layermask can be added if needed

                        // for each hit in collision
                        foreach (RaycastHit hit in hits)
                        {
                            PlayerController pc = hit.collider.GetComponent<PlayerController>();
                            if (pc != null && pc.playerTeam != wc1.playerTeam)
                            {
                                float distanceMultiplier = 0; 
                                if (minimumEffectiveRange > 0 && antennaDistance <= minimumEffectiveRange)
                                    distanceMultiplier = ((-antennaDistance / minimumEffectiveRange) + 1);
                                //Debug.Log("antennaDistance: " + antennaDistance + ", minimumEffectiveRange: " + minimumEffectiveRange + ", distanceMultiplier: " + distanceMultiplier);
                                pc.health -= (((wc1.damage + wc2.damage) / 2) * Time.deltaTime * distanceMultiplier); // damage is averaged between two walkies, damage inflicted is per second, and damage is based on how close the two tips are
                            }
                        }

                    }
                }
            }
        }
	}
}
