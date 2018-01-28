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
    private Vector2 inputMove = Vector2.zero;
    private Vector2 inputLook = Vector2.zero;

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
        inputMove = new Vector2(inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_1_X), -inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_1_Y));
        inputLook = new Vector2(inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_2_X), inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_2_Y));
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
        activlyHeldWalkie.AntennaLength = inputLook.magnitude;
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
