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
        
        Debug.Log("attaching value changed handler");
        
        _microGamepad.ValueChangedHandler = OnValueChanged;
    }

    private void OnValueChanged(GCMicroGamepad arg1, GCControllerElement arg2)
    {
        Debug.Log("OnValueChanged");
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

        if (padElement != null && padElement == _microGamepad.Dpad)
        {
            HandleAxisValueChanged(padElement.XAxis);
            HandleAxisValueChanged(padElement.YAxis);;
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
        if (element == _microGamepad.Dpad.Left)
            return GCMicroGamepadElementType.DPadX;
        if (element == _microGamepad.Dpad.Right)
            return GCMicroGamepadElementType.DPadY;
        
        throw new ArgumentException($"unhandled element: {element}");
    }
}
