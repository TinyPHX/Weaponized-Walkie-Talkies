using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public PlayerAnimator playerAnimator;
    public WalkieController activelyHeldWalkie;
    public Transform walkieAnchor;

    public float move_speed = 10f; // units per second
    public float rotate_speed = 10f; // degrees per second
    [HideInInspector]
    public InputDevice inputDevice;
    public Rigidbody rigidBody;
    public float rotationDeadzone = 0.001f;

    public bool altAntennaControl = false;
    public float antennaSpeed = 10;
    public AnimationCurve antennaSpeedCurve;

    public InputDevice.GenericInputs jumpAxis = InputDevice.GenericInputs.ACTION_1; // a

    //Input
    private Vector2 inputMove = Vector2.zero;
    private Vector2 inputLook = Vector2.zero;
    private float inputJump;
    private float inputToggleWalkieOn;
    private float inputPreviousToggleWalkieOn;
    private float inputAntennaOut;
    private float inputAntennaIn;

    private bool jumpLock = false;
    private float lastJumpTime = 0f;
    [SerializeField]
    private bool grounded = false;
    public float jumpLockLength = 1f;
    public float jumpForce = 7f;
    public string jumpMaskString = "Everything";
    private float distanceToGround = .05f;
    private LayerMask jumpMask;
    public Vector3 jumpDirection = Vector3.up;

    [SerializeField]
    private float maxHealth = 250f;
    [SerializeField]
    private float health = 250f;

    public WalkieController.Team playerTeam = WalkieController.Team.RED;

    // Use this for initialization
    void Start()
    {
        jumpMask = LayerMask.NameToLayer(jumpMaskString);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWalkiePosition();

        if (inputDevice != null)
        {
            UpdateInput();

            UpdateMovement();
            UpdateWalkieAntenna();
        }

        UpdateJump();
    }

    public void UpdateWalkiePosition()
    {
        if (activelyHeldWalkie != null)
        {
            activelyHeldWalkie.transform.position = walkieAnchor.transform.position;
            activelyHeldWalkie.transform.rotation = walkieAnchor.transform.rotation;

            Rigidbody walkieRigidBody = activelyHeldWalkie.GetComponent<Rigidbody>();
            if (walkieRigidBody != null)
            {
                walkieRigidBody.velocity = Vector3.zero;
            }
        }
    }

    private void UpdateInput()
    {
        inputPreviousToggleWalkieOn = inputToggleWalkieOn;

        inputMove = new Vector2(inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_1_X), -inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_1_Y));
        inputLook = new Vector2(inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_2_X), inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_2_Y));
        inputJump = inputDevice.GetAxis(InputDevice.GenericInputs.ACTION_1);
        inputToggleWalkieOn = inputDevice.GetAxis(InputDevice.GenericInputs.ACTION_4);
        inputAntennaIn = inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_ALT_1);
        inputAntennaOut = inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_ALT_2);
    }

    private void UpdateMovement()
    {
        rigidBody.AddForce(new Vector3(inputMove.x, 0f, inputMove.y) * Time.deltaTime * move_speed);

        Vector3 goalRotationVector = Vector3.zero;

        if (inputLook.magnitude > .1f)
        {
            goalRotationVector = new Vector3(inputLook.x, 0f, -inputLook.y);
        }
        else if (inputMove.magnitude > .1f)
        {
            goalRotationVector = new Vector3(inputMove.x, 0f, inputMove.y);
        }

        Quaternion goalRotation = new Quaternion();
        if (goalRotationVector != Vector3.zero)
        {
            goalRotation = Quaternion.LookRotation(goalRotationVector);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, goalRotation, Time.deltaTime * move_speed);
        }

        playerAnimator.RunBlend = inputMove.magnitude;
        playerAnimator.SpeedMultiplier = inputMove.magnitude;
    }

    private void UpdateWalkieAntenna()
    {
        if (activelyHeldWalkie != null)
        {
            if (altAntennaControl)
            {
                float newAntennaLenngth = activelyHeldWalkie.AntennaLength +
                    ((antennaSpeedCurve.Evaluate(inputAntennaOut) - antennaSpeedCurve.Evaluate(inputAntennaIn)) * Time.deltaTime * antennaSpeed);
                activelyHeldWalkie.AntennaLength = Mathf.Clamp(newAntennaLenngth, 0, 1);
            }
            else
            {
                activelyHeldWalkie.AntennaLength = inputLook.magnitude;
            }

            if (inputToggleWalkieOn > 0 && inputPreviousToggleWalkieOn == 0)
            {
                activelyHeldWalkie.on = !activelyHeldWalkie.on;
            }
        }
    }

    public float getHealth()
    {
        return health;
    }

    public void setHealth(float health)
    {
        this.health = health;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public void setMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public void changeHealth(float amount) // negative or positive
    {
        if (this.health + amount > maxHealth)
        {
            this.health = maxHealth;
        }
        else if (this.health + amount < 0)
        {
            this.health = 0;
        }
        else
        {
            this.health += amount;
        }
    }

    public bool Grounded
    {
        get
        {
            return grounded;
        }

        set
        {
            grounded = value;
        }
    }

    private void UpdateJump()
    {

        float jump_axis = 0;
        if (inputDevice != null)
            jump_axis = inputDevice.GetAxis(jumpAxis);

        bool newGrounded = CheckIfGrounded();

        if (jumpLock && Time.time - lastJumpTime > jumpLockLength && jump_axis == 0)
        {
            jumpLock = false;
        }

        if (!jumpLock)
        {
            if (newGrounded && !Grounded)
            {
                jump_axis = 0;
            }

            Grounded = newGrounded;
        }

        if (jump_axis > 0)
        {
            Jump();
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    private void Jump()
    {
        if (Grounded && !jumpLock)
        {
            Vector3 jumpVelocity = jumpDirection * (jumpForce);

            rigidBody.velocity += jumpVelocity;
            //animationController.TriggerJump = true;
            Grounded = false;
            jumpLock = true;
            lastJumpTime = Time.time;

            //jumpEffect.transform.position = transform.position;
            //jumpEffect.Play();
        }
    }

    /// <summary>
    /// Determins wheather the player collider is on the ground.
    /// </summary>
    private bool CheckIfGrounded()
    {
        bool groundCheck = false;

        Collider collider = this.GetComponent<Collider>();

        RaycastHit raycastHit = new RaycastHit();
        groundCheck = Physics.Raycast(
            new Ray(collider.bounds.center, Vector3.down),
            out raycastHit,
            collider.bounds.size.y / 2 + distanceToGround,
            jumpMask);

        return groundCheck;
    }
}
