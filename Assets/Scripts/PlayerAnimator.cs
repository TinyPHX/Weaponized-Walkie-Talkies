using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public AnimationCurve speedMultiplierCurve;
    
    private const string FLOAT_SPEEDMULTIPLIER = "SpeedMultiplier";
    private const string FLOAT_RUNBLEND = "RunBlend";
    private const string BOOL_MOVINGBACKWARDS = "MovingBackwards";
    
    private float speedMultiplier;
    private float runBlend;
    private bool movingBackwards;

    public float RunBlend
    {
        get { return runBlend; }
        set
        {
            runBlend = value * speedMultiplierCurve.Evaluate(value);
            animator.SetFloat(FLOAT_RUNBLEND, RunBlend);

            if (runBlend != 0)
            {
                Debug.Log("runBlend: " + runBlend);
            }
        }
    }

    public float SpeedMultiplier
    {
        get { return speedMultiplier; }
        set
        {
            speedMultiplier = speedMultiplierCurve.Evaluate(value) * 5;
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
