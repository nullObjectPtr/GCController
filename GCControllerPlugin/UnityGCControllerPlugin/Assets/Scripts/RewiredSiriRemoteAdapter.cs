using System;
using System.ComponentModel;
using System.Linq;
using HovelHouse.GameController;
using Rewired;
using UnityEngine;

public class RewiredSiriRemoteAdapter : AbstractRewiredAdapter
{
    public event Action<GCControllerButtonInput, bool> OnButtonValueChanged;
    public event Action<GCControllerAxisInput, float> OnAxisValueChanged;
    
    private readonly GCMicroGamepad _microGamepad;
    
    private readonly RewiredToGCMicroGamepadElementMap ElementConverterMap;

    private bool MenuButtonWasPressedThisUpdate;
    private bool MenuButtonIsPressed;

    public RewiredSiriRemoteAdapter(
        GCController controller, 
        int profileId, 
        RewiredToGCMicroGamepadElementMap elementConverterMap)
    : base(controller, profileId)
    {
        _microGamepad = controller.MicroGamepad;
        
        ElementConverterMap = elementConverterMap;
        
        // Special case handling for the menu button
        // Which is reporting incorrect values in the OnValueChanged handler
        // when you set allowExitToHome to false in unity
        _microGamepad.ButtonMenu.PressedChangedHandler = OnMenuButtonPressed;
        _microGamepad.ValueChangedHandler = OnValueChanged;

        ReInput.InputSourceUpdateEvent += OnInputSourceUpdated;
    }

    private void OnInputSourceUpdated()
    {
        UpdateButton(GCMicroGamepadElementType.ButtonA);
        UpdateButton(GCMicroGamepadElementType.ButtonX);
        UpdateButton(GCMicroGamepadElementType.DPadLeft);
        UpdateButton(GCMicroGamepadElementType.DPadRight);
        UpdateButton(GCMicroGamepadElementType.DPadUp);
        UpdateButton(GCMicroGamepadElementType.DPadDown);
        
        // Special handling for the menuButton
        var record =
            ElementConverterMap.Records.FirstOrDefault(r => r.microGamepadElementType == GCMicroGamepadElementType.ButtonMenu);

        if (record == null) { return; }
        VirtualController.SetButtonValue(record.RewiredElementName, MenuButtonWasPressedThisUpdate || MenuButtonIsPressed);
        MenuButtonWasPressedThisUpdate = false;
    }

    private void UpdateButton(GCMicroGamepadElementType elementType)
    {
        var record =
            ElementConverterMap.Records.FirstOrDefault(r => r.microGamepadElementType == elementType);
        
        // If the rewired element isn't in the map, then the rewired virtual controller does not process this
        // element and we can skip it
        if (record == null) { return; }

        var element = (GCControllerButtonInput) GetGCElement(elementType);
        VirtualController.SetButtonValue(record.RewiredElementName, element.Pressed);
    }

    private void OnMenuButtonPressed(GCControllerButtonInput arg1, float buttonValue, bool buttonPressed)
    {
        var buttonInput = _microGamepad.ButtonMenu;

        Debug.Log($"On Menu Button Pressed: {buttonPressed}");
        
        // We may get pressed/not-pressed events from iOS in a single update, so we have to check if the button
        // was down at any-point in a single input update
        MenuButtonWasPressedThisUpdate |= buttonPressed;
        MenuButtonIsPressed = buttonPressed;
        
        OnButtonValueChanged?.Invoke(buttonInput, buttonPressed);
    }
    
    private void OnValueChanged(GCMicroGamepad arg1, GCControllerElement arg2)
    {
        var elem = arg2;

        var btnElement = arg2 as GCControllerButtonInput;
        var padElement = arg2 as GCControllerDirectionPad;

        // Special Case Handling for the menu button is handled by OnMenuButtonPressed
        // The menu buttons pressed value is overwritten by unity when allowExitToHome == false
        if (btnElement != null && btnElement != _microGamepad.ButtonMenu)
        {
            OnButtonValueChanged?.Invoke(btnElement, btnElement.Pressed);
        }

        if (padElement == null)
        {
            return; 
        }

        if (padElement != _microGamepad.Dpad) return;
        
        HandleAxisValueChanged(padElement.XAxis);
        HandleAxisValueChanged(padElement.YAxis);
    }

    public override GCControllerElement GetGCElementForRewiredElementName(string rewiredElementName)
    {
        Debug.Log($"GetGCElementForRewiredElementName:{rewiredElementName}");

        var record = ElementConverterMap.Records.FirstOrDefault(r => r.RewiredElementName == rewiredElementName);
        if (record != null) return GetGCElement(record.microGamepadElementType);

        Debug.LogWarning($"no entry for rewired element named:'{rewiredElementName}' found in element map");
        return null;
    }

    private GCControllerElement GetGCElement(GCMicroGamepadElementType elementType)
    {
        switch (elementType)
        {
            case GCMicroGamepadElementType.ButtonA:
                return _microGamepad.ButtonA;
            case GCMicroGamepadElementType.ButtonX:
                return _microGamepad.ButtonX;
            case GCMicroGamepadElementType.ButtonMenu:
                return _microGamepad.ButtonMenu;
            case GCMicroGamepadElementType.DPad:
                return _microGamepad.Dpad;
            case GCMicroGamepadElementType.DPadX:
                return _microGamepad.Dpad.XAxis;
            case GCMicroGamepadElementType.DPadY:
                return _microGamepad.Dpad.YAxis;
            case GCMicroGamepadElementType.DPadLeft:
                return _microGamepad.Dpad.Left;
            case GCMicroGamepadElementType.DPadRight:
                return _microGamepad.Dpad.Right;
            case GCMicroGamepadElementType.DPadUp:
                return _microGamepad.Dpad.Up;
            case GCMicroGamepadElementType.DPadDown:
                return _microGamepad.Dpad.Down;
            default:
                throw new InvalidEnumArgumentException(nameof(elementType));
        }
    }

    private void HandleAxisValueChanged(GCControllerAxisInput axisInput)
    {
        var elementType = GetElementType(axisInput);
        
        var record = ElementConverterMap.Records
            .FirstOrDefault( r => r.microGamepadElementType == elementType);
        
        if(record == null)
            throw new Exception($"could not find an element map entry for controller element: {axisInput.LocalizedName}");
        
        // hrm - apple buttons are also PRESSURE sensitive
        // var value = buttonInput.Value; 
        // is a float between 0 and 1 
        // Interesting...
        
        var value = axisInput.Value;
        VirtualController.SetAxisValue(record.RewiredElementName, value);
        OnAxisValueChanged?.Invoke(axisInput, value);
    }

    public override string GetElementName(GCControllerElement element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));

        var type = GetElementType(element);
        return type.ToString();
    }

    private GCMicroGamepadElementType GetElementType(GCControllerElement element)
    {
        if (element == _microGamepad.ButtonA)
            return GCMicroGamepadElementType.ButtonA;
        if (element == _microGamepad.ButtonX)
            return GCMicroGamepadElementType.ButtonX;
        if (element == _microGamepad.ButtonMenu)
            return GCMicroGamepadElementType.ButtonMenu;
        if (element == _microGamepad.Dpad)
            return GCMicroGamepadElementType.DPad;
        if (element == _microGamepad.Dpad.XAxis)
            return GCMicroGamepadElementType.DPadX;
        if (element == _microGamepad.Dpad.YAxis)
            return GCMicroGamepadElementType.DPadY;
        if (element == _microGamepad.Dpad.Left)
            return GCMicroGamepadElementType.DPadLeft;
        if (element == _microGamepad.Dpad.Right)
            return GCMicroGamepadElementType.DPadRight;
        if (element == _microGamepad.Dpad.Up)
            return GCMicroGamepadElementType.DPadUp;
        if (element == _microGamepad.Dpad.Down)
            return GCMicroGamepadElementType.DPadDown;
        
        throw new ArgumentException($"unhandled element: {element}");
    }
}
