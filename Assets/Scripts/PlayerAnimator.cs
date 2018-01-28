using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    public Animator animator;
    public AnimationCurve speedMultiplierCurve;
    //public AnimationCurve hurtFlash;
    //public float hurtTime;
    //public Renderer[] flashRenders;

    //private const string LAYER_IDLE = "Idle";
    //private const string LAYER_SWORDIK = "Sword IK";
    //private const string LAYER_MOVEMENT = "Movement";
    //private const string LAYER_ATTACK = "Attack";
    //private const string LAYER_ATTACK_A = "Attack A";
    //private const string LAYER_REACTION = "Reaction";
    //private const string LAYER_REACTION_A = "Reaction A";
    //private const string LAYER_DEATH = "Death";
    //private const string LAYER_RUN = "Run";
    //private const string LAYER_WALK = "Walk";
    //private const string LAYER_FROZEN = "Frozen";

    //Animator Parameters Constants
    private const string FLOAT_SPEEDMULTIPLIER = "SpeedMultiplier";
    //private const string FLOAT_SHOOTSPEED = "ShootSpeed";
    //private const string BOOL_SPECIAL = "Special";
    private const string BOOL_MOVINGBACKWARDS = "MovingBackwards";
    //private const string BOOL_GUNACTIVE = "GunActive";
    //private const string BOOL_SWORDACTIVE = "SwordActive";
    //private const string BOOL_SHOOTING = "Shooting";
    //private const string BOOL_JUMPING = "Jumping";
    //private const string BOOL_DEAD = "Dead";
    //private const string BOOL_FROZEN = "Frozen";
    //private const string TRIGGER_JUMP = "Jump";
    //private const string TRIGGER_DAMAGED = "Damaged";
    //private const string TRIGGER_DIE = "Die";
    //private const string TRIGGER_ENTER_SHIP = "EnterShip";
    //private const string TRIGGER_SHOTFIRED = "ShotFired";

    private const string STATE_EMPTY = "None";

    //Animator Parameters
    private float speedMultiplier;
    //private float shootSpeed;
    //private bool special;
    private bool movingBackwards;
    //private bool shooting;
    //private bool jumping;
    //private bool dead;
    //private bool gunActive;
    //private bool swordActive;
    //private bool frozen;
    
    void Update()
    {
        UpdateState();
    }

    /// <summary>
    /// Updates the animation state based off of input,
    /// getting hit, etc.
    /// </summary>
    void UpdateState()
    {
        //Use Override attack animation when in Idle and Additve in all other states.
        //bool isIdle = !animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex(LAYER_IDLE)).IsName(STATE_EMPTY);
        //bool isAttack = !animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex(LAYER_ATTACK)).IsName(STATE_EMPTY);
        //if (isAttack)
        //{
        //    float currentAttackWeight = animator.GetLayerWeight(animator.GetLayerIndex(LAYER_ATTACK));
        //    float currentAttackAdditiveWeight = animator.GetLayerWeight(animator.GetLayerIndex(LAYER_ATTACK_A));
        //    float transitionSpeed = Time.deltaTime * 8;

        //    float newAttackWeight;
        //    float newAttackAdditiveWeight;

        //    if (isIdle)
        //    {
        //        newAttackWeight = 1f;
        //        newAttackAdditiveWeight = 0f;
        //    }
        //    else
        //    {
        //        newAttackWeight = 0f;
        //        newAttackAdditiveWeight = 1f;
        //    }

        //    animator.SetLayerWeight(animator.GetLayerIndex(LAYER_ATTACK), Mathf.MoveTowards(currentAttackWeight, newAttackWeight, transitionSpeed));
        //    animator.SetLayerWeight(animator.GetLayerIndex(LAYER_ATTACK_A), Mathf.MoveTowards(currentAttackAdditiveWeight, newAttackAdditiveWeight, transitionSpeed));

        //    //animator.SetLayerWeight(animator.GetLayerIndex(LAYER_ATTACK), newAttackWeight);
        //    //animator.SetLayerWeight(animator.GetLayerIndex(LAYER_ATTACK_ADDITIVE), newAttackAdditiveWeight);
        //}
    }

    public float SpeedMultiplier
    {
        get { return speedMultiplier; }
        set
        {
            speedMultiplier = value * speedMultiplierCurve.Evaluate(value);
            animator.SetFloat(FLOAT_SPEEDMULTIPLIER, speedMultiplier);
            
            //animator.SetLayerWeight(animator.GetLayerIndex(LAYER_RUN), SpeedMultiplier);
            //animator.SetLayerWeight(animator.GetLayerIndex(LAYER_WALK), 1 - SpeedMultiplier);
        }
    }

    //public float ShootSpeed
    //{
    //    get { return shootSpeed; }
    //    set
    //    {
    //        shootSpeed = Mathf.Clamp(value, minShootSpeed, maxShootSpeed);
    //        animator.SetFloat(FLOAT_SHOOTSPEED, shootSpeed);
    //    }
    //}

    //public bool Special
    //{
    //    get { return special; }
    //    set
    //    {
    //        special = value;
    //        animator.SetBool(BOOL_SPECIAL, special);
    //    }
    //}

    public bool MovingBackwards
    {
        get { return movingBackwards; }
        set
        {
            movingBackwards = value;
            animator.SetBool(BOOL_MOVINGBACKWARDS, movingBackwards);
        }
    }

    //public bool Shooting
    //{
    //    get { return shooting; }
    //    set
    //    {
    //        shooting = value;
    //        animator.SetBool(BOOL_SHOOTING, shooting);
    //    }
    //}

    //public bool Jumping
    //{
    //    get { return jumping; }

    //    set
    //    {
    //        jumping = value;
    //        animator.SetBool(BOOL_JUMPING, jumping);
    //    }
    //}

    //public bool Dead
    //{
    //    get { return dead; }
    //    set
    //    {
    //        if (value && !dead)
    //        {
    //            TriggerDie();
    //        }

    //        dead = value;
    //        animator.SetBool(BOOL_DEAD, dead);
    //    }
    //}

    //public bool GunActive
    //{
    //    get { return gunActive; }
    //    set
    //    {
    //        gunActive = value;
    //        animator.SetBool(BOOL_GUNACTIVE, gunActive);
    //    }
    //}

    //public bool SwordActive
    //{
    //    get { return swordActive; }
    //    set
    //    {
    //        swordActive = value;
    //        animator.SetBool(BOOL_SWORDACTIVE, swordActive);
            
    //        float layerWeight = 0;

    //        if (swordActive)
    //        {
    //            layerWeight = 1;
    //        }
    //        else
    //        {
    //            layerWeight = 0;
    //        }

    //        animator.SetLayerWeight(animator.GetLayerIndex(LAYER_SWORDIK), layerWeight);
    //        animator.SetLayerWeight(animator.GetLayerIndex(LAYER_IDLE), 1 - layerWeight);
    //    }
    //}

    //public bool Frozen
    //{
    //    get { return frozen; }
    //    set
    //    {
    //        frozen = value;
    //        animator.SetBool(BOOL_FROZEN, frozen);
    //    }
    //}

    //public bool TriggerJump
    //{
    //    set
    //    {
    //        if (value)
    //        {
    //            animator.SetTrigger(TRIGGER_JUMP);
    //        }
    //    }
    //}

    //public void TriggerDamaged()
    //{
    //    animator.SetTrigger(TRIGGER_DAMAGED);

    //    //foreach (Renderer renderer in flashRenders)
    //    //{
    //    //    Shader shader = renderer.material.shader;
    //    //    Debug.Log("Label: " + shader.ToString());
    //    //    if (shader.ToString() != "Standard")
    //    //    {
    //    //        float redColor = hurtFlash.Evaluate(playerController.stopWatch.ElapsedMilliseconds / playerController.invincTime);
    //    //        renderer.material.color = new Color(redColor, 0, 0);
    //    //    }
    //    //}
    //}

    //public void TriggerDie()
    //{
    //    animator.SetTrigger(TRIGGER_DIE);
    //}

    //public void TriggerEnterShip()
    //{
    //    animator.SetTrigger(TRIGGER_ENTER_SHIP);
    //}

    //public void TriggerShotFired()
    //{
    //    animator.SetTrigger(TRIGGER_SHOTFIRED);
    //}
}
