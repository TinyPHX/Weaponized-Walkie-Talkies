using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public PlayerAnimator playerAnimator;
    public WalkieController activlyHeldWalkie;
    public Transform walkieAnchor;

    public float move_speed = 10f; // units per second
    public float rotate_speed = 10f; // degrees per second
    [HideInInspector] public InputDevice inputDevice;
    public Rigidbody rigidBody;
    public float rotationDeadzone = 0.001f;

    public bool altAntennaControl = false;
    public float antennaSpeed = 10;
    public AnimationCurve antennaSpeedCurve;

    //Input
    private Vector2 inputMove = Vector2.zero;
    private Vector2 inputLook = Vector2.zero;
    private float inputJump;
    private float inputToggleWalkieOn;
    private float inputPreviousToggleWalkieOn;
    private float inputAntennaOut;
    private float inputAntennaIn;


    [SerializeField] private float maxHealth = 250f;
    private float health = 250f;

    public WalkieController.Team playerTeam = WalkieController.Team.RED;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        UpdateWalkiePosition();

        if (inputDevice != null)
        {
            UpdateInput();

            UpdateMovement();
            UpdateWalkieAntenna();
        }
    }

    public void UpdateWalkiePosition()
    {
        if (activlyHeldWalkie != null)
        {
            activlyHeldWalkie.transform.position = walkieAnchor.transform.position;
            activlyHeldWalkie.transform.rotation = walkieAnchor.transform.rotation;
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

        Debug.Log("inputMove.magnitude: " + inputMove.magnitude);

        playerAnimator.RunBlend = inputMove.magnitude;
        playerAnimator.SpeedMultiplier = inputMove.magnitude;
    }

    private void UpdateWalkieAntenna()
    {
        if (activlyHeldWalkie != null)
        {
            if (altAntennaControl)
            {
                float newAntennaLenngth = activlyHeldWalkie.AntennaLength + 
                    ((antennaSpeedCurve.Evaluate(inputAntennaOut) - antennaSpeedCurve.Evaluate(inputAntennaIn)) * Time.deltaTime * antennaSpeed);
                activlyHeldWalkie.AntennaLength = Mathf.Clamp(newAntennaLenngth, 0, 1);
            }
            else
            {
                activlyHeldWalkie.AntennaLength = inputLook.magnitude;
            }

            if (inputToggleWalkieOn > 0 && inputPreviousToggleWalkieOn == 0)
            {
                activlyHeldWalkie.on = !activlyHeldWalkie.on;
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
}
