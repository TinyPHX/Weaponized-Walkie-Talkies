using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float move_speed = 10f; // units per second
    public float rotate_speed = 10f; // degrees per second
    [HideInInspector] public InputDevice inputDevice;
    public Rigidbody rigidBody;
    public float rotationDeadzone = 0.001f;
    private Vector2 inputRotation = Vector2.zero;

    private float maxHealth = 250f;
    private float health = 250f;

    public WalkieController.Team playerTeam = WalkieController.Team.RED;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (inputDevice != null)
        {
            rigidBody.AddForce(new Vector3(inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_1_X), 0f, -inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_1_Y)) * Time.deltaTime * move_speed);
            inputRotation = new Vector2(inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_2_X), inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_2_Y));

            Vector3 goalRotationVector = new Vector3(inputRotation.x, 0f, -inputRotation.y);
            Quaternion goalRotation = new Quaternion();
            if (goalRotationVector != Vector3.zero)
                goalRotation = Quaternion.LookRotation(goalRotationVector);
            if (Mathf.Abs(inputRotation.x) >= rotationDeadzone || Mathf.Abs(inputRotation.y) >= rotationDeadzone)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, goalRotation, Time.deltaTime * move_speed);
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

    public void changeHealth(float amount) // negative or positive
    {
        if (this.health + amount > maxHealth)
        {
            this.health = maxHealth;
        } else if (this.health + amount < 0)
        {
            this.health = 0;
        } else
        {
            this.health += amount;
        }
    }
}
