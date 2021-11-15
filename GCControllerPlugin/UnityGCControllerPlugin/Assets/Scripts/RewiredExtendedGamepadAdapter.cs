using System;
using System.ComponentModel;
using System.Linq;
using HovelHouse.GameController;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

public class RewiredExtendedGamepadAdapter : AbstractRewiredAdapter
{
    public readonly GCExtendedGamepad gamepad;
    
    public event Action<GCControllerButtonInput, bool> OnButtonValueChanged;
    public event Action<GCControllerAxisInput, float> OnAxisValueChanged;

    private readonly RewiredToGCExtendedGamepadElementMap ElementConverterMap;

    public RewiredExtendedGamepadAdapter(
        GCController controller, 
        int profileId, 
        RewiredToGCExtendedGamepadElementMap elementConverterMap)
        : base(controller, profileId)
    {
        gamepad = controller.ExtendedGamepad ?? throw new ArgumentException("controller must implement GCExtended gamepad");
        ElementConverterMap = elementConverterMap;
        
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
        
        if (btnElement != null)
        {
            if (btnElement == gamepad.Dpad)
            {
                HandleButtonValueChanged(gamepad.Dpad.Left);
                HandleButtonValueChanged(gamepad.Dpad.Right);
                HandleButtonValueChanged(gamepad.Dpad.Up);
                HandleButtonValueChanged(gamepad.Dpad.Down);
            }
            else
            {
                HandleButtonValueChanged(btnElement);
            }
        }
        else if (dPadElement != null)
        {
            if (dPadElement == gamepad.LeftThumbstick)
            {
                HandleAxisValueChanged(gamepad.LeftThumbstick.XAxis);
                HandleAxisValueChanged(gamepad.LeftThumbstick.YAxis);
            }
            else if (dPadElement == gamepad.RightThumbstick)
            {
                HandleAxisValueChanged(gamepad.RightThumbstick.XAxis);
                HandleAxisValueChanged(gamepad.RightThumbstick.YAxis);
            }
            else if(gamepad is GCDualShockGamepad ps4Gamepad)
            {
                if (dPadElement == ps4Gamepad.TouchpadPrimary)
                {
                    HandleAxisValueChanged(ps4Gamepad.TouchpadPrimary.XAxis);
                    HandleAxisValueChanged(ps4Gamepad.TouchpadPrimary.YAxis);
                }
                else if (dPadElement == ps4Gamepad.TouchpadSecondary)
                {
                    HandleAxisValueChanged(ps4Gamepad.TouchpadSecondary.XAxis);
                    HandleAxisValueChanged(ps4Gamepad.TouchpadSecondary.YAxis);
                }
            }
        }
    }

    private GCExtendedGamepadElementType GetElementType(GCControllerElement element)
    {
        // find the element in the map
        if (element == gamepad.ButtonA)
        {
            return GCExtendedGamepadElementType.ButtonA;
        }
        if (element == gamepad.ButtonB)
        {
            return GCExtendedGamepadElementType.ButtonB;
        }
        if (element == gamepad.ButtonX)
        {
            return GCExtendedGamepadElementType.ButtonX;
        }
        if (element == gamepad.ButtonY)
        {
            return GCExtendedGamepadElementType.ButtonY;
        }
        if (element == gamepad.LeftShoulder)
        {
            return GCExtendedGamepadElementType.LeftShoulder;
        }
        if (element == gamepad.RightShoulder)
        {
            return GCExtendedGamepadElementType.RightShoulder;
        }
        if (element == gamepad.LeftTrigger)
        {
            return GCExtendedGamepadElementType.LeftTrigger;
        }
        if (element == gamepad.RightTrigger)
        {
            return GCExtendedGamepadElementType.RightTrigger;
        }
        if (element == gamepad.LeftThumbstickButton)
        {
            return GCExtendedGamepadElementType.LeftThumbstickButton;
        }
        if (element == gamepad.RightThumbstickButton)
        {
            return GCExtendedGamepadElementType.RightThumbstickButton;
        }
        if (element == gamepad.Dpad)
        {
            return GCExtendedGamepadElementType.DPad;
        }
        if (element == gamepad.Dpad.Left)
        {
            return GCExtendedGamepadElementType.DPadLeft;
        }
        if (element == gamepad.Dpad.Right)
        {
            return GCExtendedGamepadElementType.DPadRight;
        }
        if (element == gamepad.Dpad.Up)
        {
            return GCExtendedGamepadElementType.DPadUp;
        }
        if (element == gamepad.Dpad.Down)
        {
            return GCExtendedGamepadElementType.DPadDown;
        }
        if (element == gamepad.ButtonHome)
        {
            return GCExtendedGamepadElementType.ButtonHome;
        }
        if (element == gamepad.ButtonMenu)
        {
            return GCExtendedGamepadElementType.ButtonMenu;
        }
        if (element == gamepad.ButtonOptions)
        {
            return GCExtendedGamepadElementType.ButtonOptions;
        }
        
        if (element == gamepad.LeftThumbstick)
        {
            return GCExtendedGamepadElementType.LeftThumbstick;
        }

        if (element == gamepad.LeftThumbstick.XAxis)
        {
            return GCExtendedGamepadElementType.LeftThumbstickX;
        }

        if (element == gamepad.LeftThumbstick.YAxis)
        {
            return GCExtendedGamepadElementType.LeftThumbstickY;
        }

        if (element == gamepad.RightThumbstick)
        {
            return GCExtendedGamepadElementType.RightThumbstick;
        }
        
        if (element == gamepad.RightThumbstick.XAxis)
        {
            return GCExtendedGamepadElementType.RightThumbstickX;
        }
        
        if (element == gamepad.RightThumbstick.YAxis)
        {
            return GCExtendedGamepadElementType.RightThumbstickY;
        }
        
        // Support for special controller buttons
        if(gamepad is GCXboxGamepad xboxGamepad)
        {
            if (element == xboxGamepad.PaddleButton1)
            {
                return GCExtendedGamepadElementType.PaddleButton1;
            }

            if (element == xboxGamepad.PaddleButton2)
            {
                return GCExtendedGamepadElementType.PaddleButton2;
            }

            if (element == xboxGamepad.PaddleButton3)
            {
                return GCExtendedGamepadElementType.PaddleButton3;
            }

            if (element == xboxGamepad.PaddleButton4)
            {
                return GCExtendedGamepadElementType.PaddleButton4;
            }
        }
        else if (gamepad is GCDualShockGamepad dualShockGamepad)
        {
            if (element == dualShockGamepad.TouchpadButton)
            {
                return GCExtendedGamepadElementType.TouchpadButton;
            }

            if (element == dualShockGamepad.TouchpadPrimary)
            {
                return GCExtendedGamepadElementType.TouchpadPrimary;
            }
            
            if (element == dualShockGamepad.TouchpadPrimary.XAxis)
            {
                return GCExtendedGamepadElementType.TouchpadPrimaryX;
            }
            
            if (element == dualShockGamepad.TouchpadPrimary.YAxis)
            {
                return GCExtendedGamepadElementType.TouchpadPrimaryY;
            }

            if (element == dualShockGamepad.TouchpadSecondary)
            {
                return GCExtendedGamepadElementType.TouchpadSecondary;
            }
            
            if (element == dualShockGamepad.TouchpadSecondary.XAxis)
            {
                return GCExtendedGamepadElementType.TouchpadSecondaryX;
            }
            
            if (element == dualShockGamepad.TouchpadSecondary.YAxis)
            {
                return GCExtendedGamepadElementType.TouchpadSecondaryY;
            }
        }
        
        throw new ArgumentException($"unknown controller element :{element}");
    }

    public override string GetElementName(GCControllerElement element)
    {
        if(element == null)
            throw new ArgumentNullException(nameof(element));

        var type = GetElementType(element);
        return type.ToString();
    }

    private void HandleButtonValueChanged(GCControllerButtonInput buttonInput)
    {
        var elementType = GetElementType(buttonInput);
        var record = ElementConverterMap.Records
            .FirstOrDefault( r => r.extendedGamepadElementType == elementType);

        // If we can't find a rewired element in the converter map, then this element is not used by the rewired system, so we can skip the update for it.
        if (record == null)
        {
            Debug.Log($"could not find an element map entry for controller element: {buttonInput.LocalizedName}");
            return;
        }

        var pressed = buttonInput.Pressed;
        VirtualController.SetButtonValue(record.RewiredElementName, pressed);
        OnButtonValueChanged?.Invoke(buttonInput, pressed);
    }

    private void HandleAxisValueChanged(GCControllerAxisInput axisInput)
    {
        var elementType = GetElementType(axisInput);
        var record = ElementConverterMap.Records
            .FirstOrDefault( r => r.extendedGamepadElementType == elementType);
        
        if(record == null)
            throw new Exception($"could not find an element map entry for controller element: {axisInput.LocalizedName}");
        
        var value = axisInput.Value;
        VirtualController.SetAxisValue(record.RewiredElementName, value);
        OnAxisValueChanged?.Invoke(axisInput, value);
    }

    public override GCControllerElement GetGCElementForRewiredElementName(string rewiredElementName)
    {
        Debug.Log($"GetGCElementForRewiredElementName:{rewiredElementName}");

        var record = ElementConverterMap.Records.FirstOrDefault(r => r.RewiredElementName == rewiredElementName);
        if (record != null) return GetGCElement(record.extendedGamepadElementType);

        Debug.LogWarning($"no entry for rewired element named:'{rewiredElementName}' found in element map");
        return null;
    }

    public GCControllerElement GetGCElement(GCExtendedGamepadElementType elementType)
    {
        switch (elementType)
        {
            case GCExtendedGamepadElementType.ButtonA:
                return gamepad.ButtonA;
            case GCExtendedGamepadElementType.ButtonB:
                return gamepad.ButtonB;
            case GCExtendedGamepadElementType.ButtonX:
                return gamepad.ButtonX;
            case GCExtendedGamepadElementType.ButtonY:
                return gamepad.ButtonY;
            case GCExtendedGamepadElementType.LeftShoulder:
                return gamepad.LeftShoulder;
            case GCExtendedGamepadElementType.RightShoulder:
                return gamepad.RightShoulder;
            case GCExtendedGamepadElementType.LeftTrigger:
                return gamepad.LeftTrigger;
            case GCExtendedGamepadElementType.RightTrigger:
                return gamepad.RightTrigger;
            case GCExtendedGamepadElementType.LeftThumbstickButton:
                return gamepad.LeftThumbstickButton;
            case GCExtendedGamepadElementType.RightThumbstickButton:
                return gamepad.RightThumbstickButton;
            case GCExtendedGamepadElementType.DPad:
                return gamepad.Dpad;
            case GCExtendedGamepadElementType.DPadLeft:
                return gamepad.Dpad.Left;
            case GCExtendedGamepadElementType.DPadRight:
                return gamepad.Dpad.Right;
            case GCExtendedGamepadElementType.DPadUp:
                return gamepad.Dpad.Up;
            case GCExtendedGamepadElementType.DPadDown:
                return gamepad.Dpad.Down;
            case GCExtendedGamepadElementType.ButtonHome:
                return gamepad.ButtonHome;
            case GCExtendedGamepadElementType.ButtonMenu:
                return gamepad.ButtonMenu;
            case GCExtendedGamepadElementType.ButtonOptions:
                return gamepad.ButtonOptions;
            case GCExtendedGamepadElementType.LeftThumbstick:
                return gamepad.LeftThumbstick;
            case GCExtendedGamepadElementType.LeftThumbstickX:
                return gamepad.LeftThumbstick.XAxis;
            case GCExtendedGamepadElementType.LeftThumbstickY:
                return gamepad.LeftThumbstick.YAxis;
            case GCExtendedGamepadElementType.RightThumbstick:
                return gamepad.RightThumbstick;
            case GCExtendedGamepadElementType.RightThumbstickX:
                return gamepad.RightThumbstick.XAxis;
            case GCExtendedGamepadElementType.RightThumbstickY:
                return gamepad.RightThumbstick.YAxis;

            case GCExtendedGamepadElementType.TouchpadPrimary:
            {
                var dualShockGamepad = gamepad as GCDualShockGamepad;
                return dualShockGamepad != null ? dualShockGamepad.TouchpadPrimary.XAxis : null;
            }
            case GCExtendedGamepadElementType.TouchpadPrimaryX:
            {
                var dualShockGamepad = gamepad as GCDualShockGamepad;
                return dualShockGamepad != null ? dualShockGamepad.TouchpadPrimary.XAxis : null;
            }
            case GCExtendedGamepadElementType.TouchpadPrimaryY:
            {
                var dualShockGamepad = gamepad as GCDualShockGamepad;
                return dualShockGamepad != null ? dualShockGamepad.TouchpadPrimary.YAxis : null;
            }
            case GCExtendedGamepadElementType.TouchpadSecondary:
            {
                var dualShockGamepad = gamepad as GCDualShockGamepad;
                return dualShockGamepad != null ? dualShockGamepad.TouchpadSecondary : null;
            }
            case GCExtendedGamepadElementType.TouchpadSecondaryX:
            {
                var dualShockGamepad = gamepad as GCDualShockGamepad;
                return dualShockGamepad != null ? dualShockGamepad.TouchpadSecondary.XAxis : null;
            }
            case GCExtendedGamepadElementType.TouchpadSecondaryY:
            {
                var dualShockGamepad = gamepad as GCDualShockGamepad;
                return dualShockGamepad != null ? dualShockGamepad.TouchpadSecondary.YAxis : null;
            }
            case GCExtendedGamepadElementType.PaddleButton1:
            {
                var xBoxGamepad = gamepad as GCXboxGamepad;
                return xBoxGamepad != null ? xBoxGamepad.PaddleButton1 : null;
            }
            case GCExtendedGamepadElementType.PaddleButton2:
            {
                var xBoxGamepad = gamepad as GCXboxGamepad;
                return xBoxGamepad != null ? xBoxGamepad.PaddleButton2 : null;
            }
            case GCExtendedGamepadElementType.PaddleButton3:
            {
                var xBoxGamepad = gamepad as GCXboxGamepad;
                return xBoxGamepad != null ? xBoxGamepad.PaddleButton3 : null;
            }
            case GCExtendedGamepadElementType.PaddleButton4:
            {
                var xBoxGamepad = gamepad as GCXboxGamepad;
                return xBoxGamepad != null ? xBoxGamepad.PaddleButton4 : null;
            }
            case GCExtendedGamepadElementType.TouchpadButton:
            {
                var dualShockGamepad = gamepad as GCDualShockGamepad;
                return dualShockGamepad != null ? dualShockGamepad.TouchpadButton : null;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(elementType));
        }
    }
}
