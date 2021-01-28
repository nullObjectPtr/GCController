using System;
using HovelHouse.GameController;
using Rewired;
using UnityEngine;

public class RewiredExtendedGamepadAdapter : AbstractRewiredAdapter
{
    public readonly GCExtendedGamepad gamepad;
    
    public event Action<GCControllerButtonInput, bool> OnButtonValueChanged;
    public event Action<GCControllerAxisInput, float> OnAxisValueChanged;

    public RewiredExtendedGamepadAdapter(GCController controller, int profileId)
        : base(controller, profileId)
    {
        gamepad = controller.ExtendedGamepad;
        if(gamepad == null)
            throw new ArgumentException("controller must implement GCExtended gamepad");

        Debug.Log($"gamepad is: {gamepad.GetType().Name}");
        Debug.Log("attaching value changed handler");
        
        gamepad.ValueChangedHandler = OnValueChanged;
    }

    private void OnValueChanged(GCExtendedGamepad gamepad, GCControllerElement element)
    {
        var btnElement = element as GCControllerButtonInput;
        var dPadElement = element as GCControllerDirectionPad;
        
        Debug.Log(element.LocalizedName);
        Debug.Log(element.UnmappedLocalizedName);

        // find the element in the map
        if (element == gamepad.ButtonA)
        {
            HandleButtonValueChanged(0, btnElement);
        }
        else if (element == gamepad.ButtonB)
        {
            HandleButtonValueChanged(1, btnElement);
        }
        else if (element == gamepad.ButtonX)
        {
            HandleButtonValueChanged(2, btnElement);
        }
        else if (element == gamepad.ButtonY)
        {
            HandleButtonValueChanged(3, btnElement);
        }
        else if (element == gamepad.LeftShoulder)
        {
            HandleButtonValueChanged(4, btnElement);
        }
        else if (element == gamepad.RightShoulder)
        {
            HandleButtonValueChanged(5, btnElement);
        }
        else if (element == gamepad.LeftTrigger)
        {
            HandleButtonValueChanged(6, btnElement);
        }
        else if (element == gamepad.RightTrigger)
        {
            HandleButtonValueChanged(7, btnElement);
        }
        else if (element == gamepad.LeftThumbstickButton)
        {
            HandleButtonValueChanged(8, btnElement);
        }
        else if (element == gamepad.RightThumbstickButton)
        {
            HandleButtonValueChanged(9, btnElement);
        }
        else if (element == gamepad.Dpad)
        {
            HandleButtonValueChanged(10, gamepad.Dpad.Left);
            HandleButtonValueChanged(11, gamepad.Dpad.Right);
            HandleButtonValueChanged(12, gamepad.Dpad.Up);
            HandleButtonValueChanged(13, gamepad.Dpad.Down);
        }
        else if (element == gamepad.ButtonHome)
        {
            HandleButtonValueChanged(14, btnElement);
        }
        else if (element == gamepad.ButtonMenu)
        {
            HandleButtonValueChanged(15, btnElement);
        }
        else if (element == gamepad.ButtonOptions)
        {
            HandleButtonValueChanged(16, btnElement);
        }
        else if (element == gamepad.LeftThumbstick)
        {
            HandleAxisValueChanged(0, dPadElement.XAxis);
            HandleAxisValueChanged(1, dPadElement.YAxis);
        }
        else if (element == gamepad.RightThumbstick)
        {
            HandleAxisValueChanged(2, dPadElement.XAxis);
            HandleAxisValueChanged(3, dPadElement.YAxis);
        }
        
        // Support for special controller buttons
        else if(gamepad is GCXboxGamepad xboxGamepad)
        {
            if (element == xboxGamepad.PaddleButton1)
            {
                HandleButtonValueChanged(17, btnElement);
            }
            else if (element == xboxGamepad.PaddleButton2)
            {
                HandleButtonValueChanged(18, btnElement);
            }
            else if (element == xboxGamepad.PaddleButton3)
            {
                HandleButtonValueChanged(19, btnElement);
            }
            else if (element == xboxGamepad.PaddleButton4)
            {
                HandleButtonValueChanged(20, btnElement);
            }
        }
        else if (gamepad is GCDualShockGamepad dualShockGamepad)
        {
            if (element == dualShockGamepad.TouchpadButton)
            {
                HandleButtonValueChanged(21, btnElement);
            }
            else if (element == dualShockGamepad.TouchpadPrimary)
            {
                HandleAxisValueChanged(4, dPadElement.XAxis);
                HandleAxisValueChanged(5, dPadElement.YAxis);
            }
            else if (element == dualShockGamepad.TouchpadSecondary)
            {
                HandleAxisValueChanged(6, dPadElement.XAxis);
                HandleAxisValueChanged(7, dPadElement.YAxis);
            }
        }
    }

    private void HandleButtonValueChanged(int idx, GCControllerButtonInput buttonInput)
    {
        // hrm - apple buttons are also PRESSURE sensitive
        // var value = buttonInput.Value; 
        // is a float between 0 and 1 
        // Interesting...
        if(buttonInput == null)
            Debug.LogError($"rewired id {idx} is a null element");
        
        var pressed = buttonInput.Pressed;
        VirtualController.SetButtonValue(idx, pressed);
        OnButtonValueChanged?.Invoke(buttonInput, pressed);
    }

    private void HandleAxisValueChanged(int idx, GCControllerAxisInput axisInput)
    {
        var value = axisInput.Value;
        VirtualController.SetAxisValue(idx, value);
        OnAxisValueChanged?.Invoke(axisInput, value);
    }

    public override GCControllerElement GetGCElementForRewiredElementId(
        ControllerElementType elementType,
        int elementId)
    {
        Debug.Log($"GetGCElementForRewiredElementId: {elementType}:{elementId}");
        if (elementType == ControllerElementType.Button)
        {
            switch (elementId)
            {
                case 0:
                    return gamepad.ButtonA;
                case 1:
                    return gamepad.ButtonB;
                case 2:
                    return gamepad.ButtonX;
                case 3:
                    return gamepad.ButtonY;
                case 4:
                    return gamepad.LeftShoulder;
                case 5:
                    return gamepad.RightShoulder;
                case 6:
                    return gamepad.LeftTrigger;
                case 7:
                    return gamepad.RightTrigger;
                case 8:
                    return gamepad.LeftThumbstickButton;
                case 9:
                    return gamepad.RightThumbstickButton;
                case 10:
                    return gamepad.Dpad.Left;
                case 11:
                    return gamepad.Dpad.Right;
                case 12:
                    return gamepad.Dpad.Up;
                case 13:
                    return gamepad.Dpad.Down;
                case 14:
                    return gamepad.ButtonHome;
                case 15:
                    return gamepad.ButtonMenu;
                case 16:
                    return gamepad.ButtonOptions;
                case 22:
                {
                    var xBoxGamepad = gamepad as GCXboxGamepad;
                    return xBoxGamepad != null ? xBoxGamepad.PaddleButton1 : null;
                }
                case 23:
                {
                    var xBoxGamepad = gamepad as GCXboxGamepad;
                    return xBoxGamepad != null ? xBoxGamepad.PaddleButton2 : null;
                }
                case 24:
                {
                    var xBoxGamepad = gamepad as GCXboxGamepad;
                    return xBoxGamepad != null ? xBoxGamepad.PaddleButton3 : null;
                }
                case 25:
                {
                    var xBoxGamepad = gamepad as GCXboxGamepad;
                    return xBoxGamepad != null ? xBoxGamepad.PaddleButton4 : null;
                }
                case 26:
                {
                    var dualShockGamepad = gamepad as GCDualShockGamepad;
                    return dualShockGamepad != null ? dualShockGamepad.TouchpadButton : null;
                }
                // default:
                //     throw new ArgumentOutOfRangeException(nameof(elementId));
            }
        }

        if (elementType == ControllerElementType.Axis)
        {
            // TODO: Why do these start at 18?
            switch (elementId)
            {
                // some split rewired element get mapped back to a parent element
                case 18:
                    return gamepad.LeftThumbstick.XAxis;
                case 19:
                    return gamepad.LeftThumbstick.YAxis;

                case 20:
                    return gamepad.RightThumbstick.XAxis;
                case 21:
                    return gamepad.RightThumbstick.YAxis;

                case 29:
                {
                    var dualShockGamepad = gamepad as GCDualShockGamepad;
                    return dualShockGamepad != null ? dualShockGamepad.TouchpadPrimary.XAxis : null;
                }
                case 30:
                {
                    var dualShockGamepad = gamepad as GCDualShockGamepad;
                    return dualShockGamepad != null ? dualShockGamepad.TouchpadPrimary.YAxis : null;
                }
                case 31:
                {
                    var dualShockGamepad = gamepad as GCDualShockGamepad;
                    return dualShockGamepad != null ? dualShockGamepad.TouchpadSecondary.XAxis : null;
                }
                case 32:
                {
                    var dualShockGamepad = gamepad as GCDualShockGamepad;
                    return dualShockGamepad != null ? dualShockGamepad.TouchpadSecondary.YAxis : null;
                }
                // default:
                //     throw new ArgumentOutOfRangeException(nameof(elementId));
            }
        }

        return null;
    }
}
