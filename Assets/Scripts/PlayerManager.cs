using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerManager : MonoBehaviour
{
    public List<PlayerController> playerControllers = new List<PlayerController> { };
    public List<InputDevice> allInputDevices = new List<InputDevice> { };
    public List<InputDevice.ID> unassignedInputDevices = new List<InputDevice.ID> { };

    public void Start()
    {
        int controllerCount = InputDevice.CONTROLLERS.Count;
        for (int i = 0; i < controllerCount; i++)
        {
            InputDevice.ID inputDeviceID = InputDevice.CONTROLLERS[i];
            InputDevice inputDevice = new InputDevice(inputDeviceID);
            unassignedInputDevices.Add(inputDeviceID);

            allInputDevices.Add(inputDevice);
        }
    }

    public void Update()
    {
        UpdateInputDeviceAssignments();
        RefreshInputDevices();
    }
    
    private void UpdateInputDeviceAssignments()
    {
        //for each playerController 
            //if playerController.inputDevice == null
                //loop each allInputDevices
                    //check if "A" pressed
                        //playerController.inputDevice = curentInputDevice
        foreach (PlayerController pc in playerControllers) {
            if(pc.inputDevice == null)
            {
                foreach (InputDevice.ID i in unassignedInputDevices)
                {
                    foreach (InputDevice inputDevice in allInputDevices) {
                        if (inputDevice != null && inputDevice.Id == i && inputDevice.GetAxis(InputDevice.GenericInputs.ACTION_1) > 0)
                        {
                            pc.inputDevice = inputDevice;
                            unassignedInputDevices.Remove(pc.inputDevice.Id);
                        }
                    }
                }
            }

        }
    }

    private void RefreshInputDevices()
    {
        //loop through playerControllers
        //check if playerController.inputDevice.IsController() == false
        //playerController.InputDevice = null;
        foreach (PlayerController pc in playerControllers)
        {
            if (pc.inputDevice != null && pc.inputDevice.IsController() == false)
            {
                unassignedInputDevices.Add(pc.inputDevice.Id);
                pc.inputDevice = null;
            }

        }
    }

}
