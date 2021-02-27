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

    public RewiredSiriRemoteAdapter(
        GCController controller, 
        int profileId, 
        RewiredToGCMicroGamepadElementMap elementConverterMap)
    : base(controller, profileId)
    {
        _microGamepad = controller.MicroGamepad;
        
        ElementConverterMap = elementConverterMap;
        
        _microGamepad.ValueChangedHandler = OnValueChanged;
    }

    private void OnValueChanged(GCMicroGamepad arg1, GCControllerElement arg2)
    {
        var elem = arg2;
        var parentElem = arg2.Collection;

        Debug.Log($"element symbols: {elem.SfSymbolsName}:{elem.UnmappedSfSymbolsName}");
        if (parentElem != null)
            Debug.Log($"parent element symbols {parentElem.SfSymbolsName}:{parentElem.UnmappedSfSymbolsName}");

        var btnElement = arg2 as GCControllerButtonInput;
        var padElement = arg2 as GCControllerDirectionPad;

        if (btnElement != null)
        {
            HandleButtonValueChanged(btnElement);
        }

        if (padElement == null)
        {
            return; 
        }

        if (padElement != _microGamepad.Dpad) return;
        
        HandleAxisValueChanged(padElement.XAxis);
        HandleAxisValueChanged(padElement.YAxis);

        // Modify this if you need to treat the touchpad like an digital DPad
        // HandleButtonValueChanged(_microGamepad.Dpad.Left);
        // HandleButtonValueChanged(_microGamepad.Dpad.Right);
        // HandleButtonValueChanged(_microGamepad.Dpad.Up);
        // HandleButtonValueChanged(_microGamepad.Dpad.Down);
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

    private void HandleButtonValueChanged(GCControllerButtonInput buttonInput)
    {
        var elementType = GetElementType(buttonInput);
        
        var record = ElementConverterMap.Records
            .FirstOrDefault( r => r.microGamepadElementType == elementType);
        
        if(record == null)
            throw new Exception($"could not find an element map entry for controller element: {buttonInput.LocalizedName}");

        // hrm - apple buttons are also PRESSURE sensitive
        // var value = buttonInput.Value; 
        // is a float between 0 and 1 
        // Interesting...
        var pressed = buttonInput.Pressed;
        
        Debug.Log($"SetButtonValue {elementType}/{record.RewiredElementName} pressed:{pressed}");
        VirtualController.SetButtonValue(record.RewiredElementName, pressed);
        OnButtonValueChanged?.Invoke(buttonInput, pressed);
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
