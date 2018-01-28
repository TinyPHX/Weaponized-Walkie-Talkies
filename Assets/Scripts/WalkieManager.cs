﻿using System.Collections;
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
    public bool friendlyFire = false;

    public bool updateBatterySliders = true;
    public List<BatteryDisplay> batteryDisplays = new List<BatteryDisplay>();

    public float[] beamCutoffs = { 2, 6 }; // less than i0 = 3 beams, less than i1 and greater than i0 = 2 beams, greater than i1 = 1 beam
    public float[] beamIntervalDamages = { 12, 6, 3 }; // damages for each of the above three intervals
    public int[] intervalNumBeams = { 3, 2, 1 };

    // Use this for initialization
    void Start () {
        foreach (Signal s in signalEffectsRed)
        {
            s.GetComponent<LineRenderer>().enabled = false;
        }

        foreach (Signal s in signalEffectsBlue)
        {
            s.GetComponent<LineRenderer>().enabled = false;
        }

        //walkieControllerList = new List<WalkieController>(FindObjectsOfType<WalkieController>());
    }
	
	// Update is called once per frame
	void Update () {
        if (updateBatterySliders)
            UpdateBatterySliders();

        foreach (WalkieController wc1 in walkieControllerList)
        {
            foreach (WalkieController wc2 in walkieControllerList)
            {
                if (wc1 != wc2 && wc1.playerTeam == wc2.playerTeam)
                {
                    Transform at1 = wc1.antennaTip;
                    Transform at2 = wc2.antennaTip;
                    float antennaDistance = Vector3.Distance(at1.position, at2.position);

                    if (wc1.power > 0 && wc2.power > 0 && wc1.on == true && wc2.on == true) {

                        if (antennaDistance <= 30) // this can change
                        {
                            RaycastHit[] hits;
                            hits = Physics.RaycastAll(at1.transform.position, at2.transform.position - at1.transform.position, Vector3.Distance(at1.transform.position, at2.transform.position)); // layermask can be added if needed

                            // for each hit in collision
                            foreach (RaycastHit hit in hits)
                            {
                                PlayerController pc = hit.collider.GetComponent<PlayerController>();
                                if (pc != null)
                                {
                                    if (pc.playerTeam != wc1.playerTeam || friendlyFire)
                                    {
                                        float damage = 0;

                                        if (wc1.playerTeam == WalkieController.Team.RED) // 
                                        {
                                            foreach (Signal s in signalEffectsRed)
                                            {
                                                s.GetComponent<LineRenderer>();
                                            }
                                        }

                                        if (wc1.playerTeam == WalkieController.Team.BLUE) // 
                                        {
                                            foreach (Signal s in signalEffectsBlue)
                                            {
                                                s.GetComponent<LineRenderer>();
                                            }
                                        }

                                        if (antennaDistance < beamCutoffs[0])
                                        {
                                            damage = beamIntervalDamages[0];
                                        }
                                        if(antennaDistance >= beamCutoffs[0] && antennaDistance <= beamCutoffs[1])
                                        {
                                            damage = beamIntervalDamages[1];
                                        }
                                        if(antennaDistance >= beamCutoffs[1])
                                        {
                                            damage = beamIntervalDamages[2];
                                        }

                                        pc.changeHealth(-(damage * Time.deltaTime));
                                    }
                                }
                            }

                            if (wc1.playerTeam == WalkieController.Team.RED)
                            {
                                startPointRed.position = at1.position;
                                endPointRed.position = at2.position;

                                foreach (Signal s in signalEffectsRed)
                                {
                                    s.GetComponent<LineRenderer>().enabled = false;
                                }

                                int numBeams = 0;

                                if (antennaDistance < beamCutoffs[0])
                                {
                                    numBeams = intervalNumBeams[0];
                                }
                                if (antennaDistance >= beamCutoffs[0] && antennaDistance <= beamCutoffs[1])
                                {
                                    numBeams = intervalNumBeams[1];
                                }
                                if (antennaDistance >= beamCutoffs[1])
                                {
                                    numBeams = intervalNumBeams[2];
                                }

                                for(int i=0;i<numBeams;i++)
                                {
                                    if (signalEffectsRed.Count > i)
                                        signalEffectsRed[i].GetComponent<LineRenderer>().enabled = true;
                                }
                            }

                            if (wc1.playerTeam == WalkieController.Team.BLUE)
                            {
                                startPointBlue.position = at1.position;
                                endPointBlue.position = at2.position;

                                foreach (Signal s in signalEffectsBlue)
                                {
                                    s.GetComponent<LineRenderer>().enabled = false;
                                }

                                int numBeams = 0;

                                if (antennaDistance < beamCutoffs[0])
                                {
                                    numBeams = intervalNumBeams[0];
                                }
                                if (antennaDistance >= beamCutoffs[0] && antennaDistance <= beamCutoffs[1])
                                {
                                    numBeams = intervalNumBeams[1];
                                }
                                if (antennaDistance >= beamCutoffs[1])
                                {
                                    numBeams = intervalNumBeams[2];
                                }

                                for (int i = 0; i < numBeams; i++)
                                {
                                    if (signalEffectsBlue.Count > i)
                                        signalEffectsBlue[i].GetComponent<LineRenderer>().enabled = true;
                                }
                            }

                        }
                    }
                    else
                    {
                        if (wc1.playerTeam == WalkieController.Team.RED)
                        {
                            foreach (Signal s in signalEffectsRed)
                            {
                                s.GetComponent<LineRenderer>().enabled = false;
                            }
                        }

                        if (wc1.playerTeam == WalkieController.Team.BLUE)
                        {
                            foreach (Signal s in signalEffectsBlue)
                            {
                                s.GetComponent<LineRenderer>().enabled = false;
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
