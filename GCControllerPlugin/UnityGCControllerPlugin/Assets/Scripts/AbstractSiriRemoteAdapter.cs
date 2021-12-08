using System;
using System.ComponentModel;
using System.Linq;
using HovelHouse.GameController;
using Rewired;
using UnityEngine;

public abstract class AbstractSiriRemoteAdapter : AbstractRewiredAdapter
{
    public event Action<GCControllerButtonInput, bool> OnButtonValueChanged;
    public event Action<GCControllerAxisInput, float> OnAxisValueChanged;

    public bool MenuButtonWasPressedThisUpdate;
    public bool MenuButtonIsPressed;
    
    protected readonly RewiredToGCMicroGamepadElementMap ElementConverterMap;
    private readonly GCMicroGamepad _microGamepad;

    public AbstractSiriRemoteAdapter(
        GCController controller, 
        int profileId, 
        RewiredToGCMicroGamepadElementMap elementConverterMap)
    : base(controller, profileId)
    {
        _microGamepad = controller.MicroGamepad;
        if (_microGamepad == null)
        {
            throw new InvalidOperationException("supplied controller does not support the microgamepad profile");
        }
        
        ElementConverterMap = elementConverterMap;
        
        // Special case handling for the menu button
        // Which is reporting incorrect values in the OnValueChanged handler
        // when you set allowExitToHome to false in unity
        _microGamepad.ButtonMenu.PressedChangedHandler = OnMenuButtonPressed;
        _microGamepad.ValueChangedHandler = OnValueChanged;

        ReInput.InputSourceUpdateEvent += OnInputSourceUpdated;
    }

    protected abstract void OnInputSourceUpdated();

    /**
    * Reads the button value for the given element type, and passes it to the rewired virtual controller (if it exists in the mapping)
    * If the rewired virtual controller does not use this element, we skip the update
    */
    protected void UpdateButton(GCMicroGamepadElementType elementType)
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
        
        // Special Case Handling for the menu button is handled by OnMenuButtonPressed
        // The menu buttons pressed value is overwritten by unity when allowExitToHome == false
        if (btnElement != null && btnElement != _microGamepad.ButtonMenu)
        {
            OnButtonValueChanged?.Invoke(btnElement, btnElement.Pressed);
        }

        var padElement = arg2 as GCControllerDirectionPad;

        if (padElement == _microGamepad.Dpad)
        {
            HandleAxisValueChanged(padElement.XAxis);
            HandleAxisValueChanged(padElement.YAxis);
        }
        // Null check here is important in case _microGamepad.DPadRing is also null
        else if (padElement != null && padElement == _microGamepad.DpadRing)
        {
            HandleAxisValueChanged(padElement.XAxis);
            HandleAxisValueChanged(padElement.YAxis);
        }
    }

    public override GCControllerElement GetGCElementForRewiredElementName(string rewiredElementName)
    {
        Debug.Log($"GetGCElementForRewiredElementName:{rewiredElementName}");

        var record = ElementConverterMap.Records.FirstOrDefault(r => r.RewiredElementName == rewiredElementName);
        if (record != null) return GetGCElement(record.microGamepadElementType);

        Debug.LogWarning($"no entry for rewired element named:'{rewiredElementName}' found in element map");
        return null;
    }
    
    private void HandleAxisValueChanged(GCControllerAxisInput axisInput)
    {
        var elementType = GetElementType(axisInput);
        
        var record = ElementConverterMap.Records
            .FirstOrDefault( r => r.microGamepadElementType == elementType);

        // Nothing in the element map for this input, so skip processing it
        if (record == null)
        {
            //Debug.Log($"no rewired entry in element map entry for apple controller element: {elementType}. Skipping...");
            return;
        }

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

        if (_microGamepad.DpadRing != null)
        {
            if (element == _microGamepad.DpadRing.Left)
                return GCMicroGamepadElementType.DPadRingLeft;
            if (element == _microGamepad.DpadRing.Right)
                return GCMicroGamepadElementType.DPadRingRight;
            if (element == _microGamepad.DpadRing.Up)
                return GCMicroGamepadElementType.DPadRingUp;
            if (element == _microGamepad.DpadRing.Down)
                return GCMicroGamepadElementType.DPadRingDown;
            if (element == _microGamepad.DpadRing.XAxis)
                return GCMicroGamepadElementType.DPadRingX;
            if (element == _microGamepad.DpadRing.YAxis)
                return GCMicroGamepadElementType.DPadRingY;
        }

        throw new ArgumentException($"unhandled element: {element}");
    }
    
    protected GCControllerElement GetGCElement(GCMicroGamepadElementType elementType)
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
            case GCMicroGamepadElementType.DPadRingDown:
                return _microGamepad.DpadRing?.Down;
            case GCMicroGamepadElementType.DPadRingUp:
                return _microGamepad.DpadRing?.Up;
            case GCMicroGamepadElementType.DPadRingLeft:
                return _microGamepad.DpadRing?.Left;
            case GCMicroGamepadElementType.DPadRingRight:
                return _microGamepad.DpadRing?.Right;
            case GCMicroGamepadElementType.DPadRingX:
                return _microGamepad.DpadRing?.XAxis;
            case GCMicroGamepadElementType.DPadRingY:
                return _microGamepad.DpadRing?.YAxis;
            default:
                throw new InvalidEnumArgumentException(nameof(elementType));
        }
    }
}
