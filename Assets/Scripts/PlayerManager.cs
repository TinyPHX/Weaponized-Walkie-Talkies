using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerManager
{
    List<PlayerController> playerControllers = new List<PlayerController> { };
    List<InputDevice> allInputDevices = new List<InputDevice> { };
    List<InputDevice.ID> unassignedInputDevices = new List<InputDevice.ID> { };

    public void Start()
    {
        int controllerCount = InputDevice.CONTROLLERS.Count;
        for (int i = 0; i < controllerCount; i++)
        {
            InputDevice.ID inputDeviceID = InputDevice.CONTROLLERS[i];
            InputDevice inputDevice = new InputDevice(inputDeviceID);

            allInputDevices.Add(inputDevice);
        }
    }

    public void Update()
    {

    }
    
    private void UpdateInputDeviceAssignments(PlayerController playerController)
    {
        //for each playerController 
            //if playerController.inputDevice == null
                //loop each allInputDevices
                    //check if "A" pressed
                        //playerController.inputDevice = curentInputDevice
    }

    private void RefreshInputDevices()
    {
        //loop through playerControllers
            //check if playerController.inputDevice.IsController() == false
                //playerController.InputDevice = null;
    }

}
