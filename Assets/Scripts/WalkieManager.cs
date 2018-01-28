using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkieManager : MonoBehaviour {

    public List<WalkieController> walkieControllerList;
    public List<Signal> signalEffectsRed = new List<Signal>();
    public List<Signal> signalEffectsBlue = new List<Signal>();
    public Transform startPointRed;
    public Transform startPointBlue;
    public Transform endPointRed;
    public Transform endPointBlue;

    public bool updateBatterySliders = true;
    public List<BatteryDisplay> batteryDisplays = new List<BatteryDisplay>();

    // Use this for initialization
    void Start () {
        foreach (Signal s in signalEffectsRed)
        {
            s.GetComponent<LineRenderer>().enabled = true;
        }

        foreach (Signal s in signalEffectsBlue)
        {
            s.GetComponent<LineRenderer>().enabled = true;
        }

        walkieControllerList = new List<WalkieController>(FindObjectsOfType<WalkieController>());
    }
	
	// Update is called once per frame
	void Update () {
        if (updateBatterySliders)
            UpdateBatterySliders();

        foreach (WalkieController wc1 in walkieControllerList)
        {
            foreach (WalkieController wc2 in walkieControllerList)
            {
                if (wc1 != wc2 && wc1.playerTeam == wc2.playerTeam && wc1.on == true && wc2.on == true && wc1.power > 0 && wc2.power > 0)
                {
                    Transform at1 = wc1.antennaTip;
                    Transform at2 = wc2.antennaTip;
                    float antennaDistance = Vector3.Distance(at1.position, at2.position);

                    if (antennaDistance <= 30) // this can change
                    {
                        RaycastHit[] hits;
                        hits = Physics.RaycastAll(at1.transform.position, at2.transform.position - at1.transform.position, Vector3.Distance(at1.transform.position, at2.transform.position)); // layermask can be added if needed

                        // for each hit in collision
                        foreach (RaycastHit hit in hits)
                        {
                            PlayerController pc = hit.collider.GetComponent<PlayerController>();
                            if (pc != null && pc.playerTeam != wc1.playerTeam)
                            {
                                float distanceMultiplier = 1;
                                pc.changeHealth(-(4 * Time.deltaTime * distanceMultiplier));
                            }
                        }

                        if (wc1.playerTeam == WalkieController.Team.RED)
                        {
                            startPointRed.position = at1.position;
                            endPointRed.position = at2.position;
                            foreach (Signal s in signalEffectsRed)
                            {
                                s.GetComponent<LineRenderer>().enabled = true;
                            }
                        }

                        if (wc1.playerTeam == WalkieController.Team.BLUE)
                        {
                            startPointBlue.position = at1.position;
                            endPointBlue.position = at2.position;
                            foreach (Signal s in signalEffectsBlue)
                            {
                                s.GetComponent<LineRenderer>().enabled = true;
                            }
                        }

                    }

                }
            }
        }
    }

    private void UpdateBatterySliders()
    {
        for (int i = 0; i < batteryDisplays.Count; i++)
        {
            if (walkieControllerList.Count > i)
            {
                batteryDisplays[i].SetValue(walkieControllerList[i].power);
                batteryDisplays[i].maxValue = walkieControllerList[i].maxPower;
            }
        }
    }
}
