using System;
using HovelHouse.GameController;
using Rewired;
using UnityEngine;

public abstract class AbstractRewiredAdapter : IRewiredAppleControllerAdapter
{
    protected readonly CustomController _virtualController;
    public CustomController VirtualController => _virtualController;

    protected readonly GCController _appleController;
    public GCController AppleController => _appleController;

    protected AbstractRewiredAdapter(GCController appleController, int profileId)
    {
        if(appleController == null)
            throw new ArgumentNullException(nameof(appleController));
        
        Debug.Log("Creating new rewired virtual controller");
        
        _virtualController = ReInput.controllers.CreateCustomController(profileId);
        _appleController = appleController;
    }

    public abstract GCControllerElement GetGCElementForRewiredElementName(string rewiredElementName);
    public abstract string GetElementName(GCControllerElement element);
}
