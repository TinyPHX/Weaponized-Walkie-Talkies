using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float move_speed = 10f; // units per second
    public float rotate_speed = 10f; // degrees per second
    private InputDevice inputDevice;
    public Rigidbody rigidBody;
    public float rotationDeadzone = 0.001f;
    private Vector2 inputRotation = Vector2.zero;

	// Use this for initialization
	void Start () {
        inputDevice = new InputDevice(InputDevice.ID.C1);
	}
	
	// Update is called once per frame
	void Update () {
        rigidBody.AddForce(new Vector3(inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_1_X), 0f, -inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_1_Y)) * Time.deltaTime * move_speed);
        inputRotation = new Vector2(inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_2_X), inputDevice.GetAxis(InputDevice.GenericInputs.AXIS_2_Y));

        Quaternion goalRotation = Quaternion.LookRotation(new Vector3(inputRotation.x, 0f, -inputRotation.y));
        if (Mathf.Abs(inputRotation.x) >= rotationDeadzone || Mathf.Abs(inputRotation.y) >= rotationDeadzone) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, goalRotation, Time.deltaTime * move_speed);
        }
    }
}
