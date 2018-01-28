using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkieAnimator : MonoBehaviour {
    
    public Animator animator;
    
    private float antennaLength;
    
    private const string FLOAT_ANTENNALENGTH = "AntennaLength";
    
    public float AntennaLength
    {
        get { return antennaLength; }
        set
        {
            antennaLength = value;
            animator.SetFloat(FLOAT_ANTENNALENGTH, antennaLength);
        }
    }
}
