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
                if(wc1 != wc2 && wc1.playerTeam == wc2.playerTeam && wc1.on == wc2.on == true)
                {
                    Transform at1 = wc1.antennaTip;
                    Transform at2 = wc2.antennaTip;
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(at1.transform.position, at2.transform.position - at1.transform.position, Vector3.Distance(at1.transform.position, at2.transform.position)); // layermask can be added if needed

                    // for each hit in collision
                    foreach(RaycastHit hit in hits)
                    {
                        Debug.Log(hit.collider.gameObject.name);
                    }
                }
            }
        }
	}
}
