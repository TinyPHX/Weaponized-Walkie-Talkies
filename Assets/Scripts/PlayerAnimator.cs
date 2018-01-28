using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public AnimationCurve speedMultiplierCurve;
    
    private const string FLOAT_SPEEDMULTIPLIER = "SpeedMultiplier";
    private const string BOOL_MOVINGBACKWARDS = "MovingBackwards";
    
    private float speedMultiplier;
    private bool movingBackwards;

    public float SpeedMultiplier
    {
        get { return speedMultiplier; }
        set
        {
            speedMultiplier = value * speedMultiplierCurve.Evaluate(value);
            animator.SetFloat(FLOAT_SPEEDMULTIPLIER, speedMultiplier);
        }
    }

    public bool MovingBackwards
    {
        get { return movingBackwards; }
        set
        {
            movingBackwards = value;
            animator.SetBool(BOOL_MOVINGBACKWARDS, movingBackwards);
        }
    }
}
