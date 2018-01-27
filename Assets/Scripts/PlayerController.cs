using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float move_speed = 10f; // units per second
    public float rotate_speed = 10f; // degrees per second
    public string horizontal_position_axis = "WASD Horizontal";
    public string vertical_position_axis = "WASD Vertical";
    public string horizontal_rotation_axis = "Arrow Keys Horizontal";
    public string vertical_rotation_axis = "Arrow Keys Vertical";


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position.Set(Input.GetKey * Time.deltaTime * move_speed, 0f, (transform.position.z + Input.GetAxis(vertical_position_axis)) * Time.deltaTime * move_speed); // set position with wasd

	}
}
