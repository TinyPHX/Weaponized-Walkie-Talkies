using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public List<PlayerController> playerControllers = new List<PlayerController> { };
    public bool allowKeyboardInput = true;
    private List<InputDevice> allInputDevices = new List<InputDevice> { };
    private List<InputDevice.ID> unassignedInputDevices = new List<InputDevice.ID> { };
    private List<InputDevice.ID> possibleInputIds = new List<InputDevice.ID> { };

    public bool updateHealthSliders = true;
    public List<HealthDisplay> healthDisplays = new List<HealthDisplay>();

    public void Start()
    {
        SetupInputDeviceAssignments();
        if(updateHealthSliders)
            UpdateHealthDisplays();
    }

    public void Update()
    {
        UpdateInputDeviceAssignments();
        RefreshInputDevices();
        if(updateHealthSliders)
            UpdateHealthDisplays();
    }

    private void UpdateHealthDisplays()
    {
        for (int i = 0; i < healthDisplays.Count; i++)
        {
            if (playerControllers.Count > i) {
                healthDisplays[i].SetHealth(playerControllers[i].getHealth());
                healthDisplays[i].maxHealth = playerControllers[i].getMaxHealth();
            }
        }
    }

    private void SetupInputDeviceAssignments()
    {
        possibleInputIds = InputDevice.CONTROLLERS;
        if (allowKeyboardInput)
        {
            possibleInputIds.Add(InputDevice.ID.K1);
        }
        foreach (InputDevice.ID possibleInputId in possibleInputIds)
        {
            InputDevice inputDevice = new InputDevice(possibleInputId);
            unassignedInputDevices.Add(possibleInputId);
            allInputDevices.Add(inputDevice);
        }
    }

    private void UpdateInputDeviceAssignments()
    {
        InputDevice.ID idToRemove = InputDevice.ID.NONE;
        foreach (PlayerController pc in playerControllers) {
            if(pc.inputDevice == null)
            {
                foreach (InputDevice inputDevice in allInputDevices)
                {
                    foreach (InputDevice.ID i in unassignedInputDevices) {
                        if (inputDevice != null && inputDevice.Id == i && inputDevice.GetAxis(InputDevice.GenericInputs.ACTION_1) > 0)
                        {
                            pc.inputDevice = inputDevice;
                            //unassignedInputDevices.Remove(pc.inputDevice.Id);
                            idToRemove = i;
                            continue;
                        }
                    }
                    if (idToRemove != InputDevice.ID.NONE)
                        unassignedInputDevices.Remove(idToRemove);
                    idToRemove = InputDevice.ID.NONE;
                }
            }

        }

        
    }

    private void RefreshInputDevices()
    {
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
