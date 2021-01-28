using System;
using HovelHouse.GameController;
using Rewired;
using UnityEngine;

public class RewiredSiriRemoteAdapter : AbstractRewiredAdapter
{
    public event Action<GCControllerButtonInput, bool> OnButtonValueChanged;
    public event Action<GCControllerAxisInput, float> OnAxisValueChanged;
    
    private readonly GCMicroGamepad _microGamepad;

    public RewiredSiriRemoteAdapter(GCController controller, int profileId)
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
            HandleButtonValueChanged(GetElementId(btnElement), btnElement);
        }

        if (padElement != null && padElement == _microGamepad.Dpad)
        {
            HandleAxisValueChanged(0, padElement.XAxis);
            HandleAxisValueChanged(1, padElement.YAxis);
        }
    }

    public override GCControllerElement GetGCElementForRewiredElementId(
        ControllerElementType elementType, int elementId)
    {
        if (elementType == ControllerElementType.Button)
        {
            if (elementId == 1)
                return _microGamepad.ButtonA;
            if (elementId == 2)
                return _microGamepad.ButtonX;
            if (elementId == 3)
                return _microGamepad.ButtonMenu;
        }
        else if (elementType == ControllerElementType.Axis)
        {
            if (elementId == 0)
                return _microGamepad.Dpad.XAxis;
            if (elementId == 4)
                return _microGamepad.Dpad.YAxis;
        }

        throw new ArgumentOutOfRangeException(nameof(elementId));
    }

    private int GetElementId(GCControllerElement element)
    {
        if (element == _microGamepad.ButtonA)
        {
            return 0;
        }

        if (element == _microGamepad.ButtonX)
        {
            return 1;
        }

        if (element == _microGamepad.ButtonMenu)
        {
            return 2;
        }

        return -1;
    }

    private void HandleButtonValueChanged(int rewiredElementIndex, GCControllerButtonInput buttonInput)
    {
        var pressed = buttonInput.Pressed;
        VirtualController.SetButtonValue(rewiredElementIndex, pressed);
        OnButtonValueChanged?.Invoke(buttonInput, pressed);
    }

    private void HandleAxisValueChanged(int rewiredElementIndex, GCControllerAxisInput axisInput)
    {
        var value = axisInput.Value;
        VirtualController.SetAxisValue(rewiredElementIndex, value);
        OnAxisValueChanged?.Invoke(axisInput, value);
    }
}
