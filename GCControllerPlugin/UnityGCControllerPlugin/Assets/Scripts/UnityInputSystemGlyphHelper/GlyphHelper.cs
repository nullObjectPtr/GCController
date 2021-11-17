using System;
using System.Collections.Generic;
using System.Linq;
using HovelHouse.GameController;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using Object = UnityEngine.Object;

public class GlyphHelper : Object 
{ 
    private GCController controller;
    
    public GlyphHelper([NotNull] GCController controller)
    {
        this.controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    public string GetSymbolNameForUnityElement(InputControl inputControl)
    {
        GCControllerElement element = null;

        var device = inputControl.device;
        
        if (device is XInputController)
        {
            element = GetGCElementForXInputController(inputControl);
        }
        else if (device is DualShockGamepad)
        {
            element = GetGCElementForDualShock4(inputControl);
        }
        else if (device is MicroGamepadDevice)
        {
            element = GetGCElementForMicroGamepad(inputControl);
        }
        else if (device is Gamepad)
        {
            element = GetGCElementForExtendedGamepad(inputControl);
        }

        // Nothing found
        if (element == null)
        {
            return "";
        }

        return GetSymbolNameForAppleElement(element);
    }
 
    public string GetSymbolNameForAppleElement([NotNull] GCControllerElement element) 
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        
        var symbolName = element.SfSymbolsName; 
        
        var symbol = symbolName;
 
        if (string.IsNullOrEmpty(symbol) == false) 
            return symbol; 
        
        // Workaround for a bug where BigSur does not return the d-pad 
        // symbols for the d-pad elements,  
        // this is future-proofed a bit, if they ever patch this later 
        // this block of code won't be hit 
 
        var extendedGamepad = controller.ExtendedGamepad; 
        if (extendedGamepad != null) 
        {
            #region DPad
            if (element == extendedGamepad.Dpad.Down) 
            { 
                return "dpad.down.fill"; 
            } 
 
            if (element == extendedGamepad.Dpad.Left) 
            { 
                return "dpad.left.fill"; 
            } 
 
            if (element == extendedGamepad.Dpad.Right) 
            { 
                return "dpad.right.fill"; 
            } 
 
            if (element == extendedGamepad.Dpad.Up) 
            { 
                return "dpad.up.fill"; 
            }
            #endregion

            #region LeftThumbstick
            if (element == extendedGamepad.LeftThumbstick.Left)
            {
                return "l.joystick.tilt.left.fill";
            }

            if (element == extendedGamepad.LeftThumbstick.Right)
            {
                return "l.joystick.tilt.right.fill";
            }
            
            if (element == extendedGamepad.LeftThumbstick.Up)
            {
                return "l.joystick.tilt.up.fill";
            }
            
            if (element == extendedGamepad.LeftThumbstick.Down)
            {
                return "l.joystick.tilt.down.fill";
            }

            if (element == extendedGamepad.LeftThumbstickButton)
            {
                return "l.joystick.press.down.fill";
            }
                
            if (element == extendedGamepad.LeftThumbstick.XAxis 
                || element == extendedGamepad.LeftThumbstick.YAxis) 
            { 
                return "l.joystick"; 
            }
            #endregion

            #region RightThumbstick
            if (element == extendedGamepad.RightThumbstick.Left)
            {
                return "r.joystick.tilt.left.fill";
            }

            if (element == extendedGamepad.RightThumbstick.Right)
            {
                return "r.joystick.tilt.right.fill";
            }
            
            if (element == extendedGamepad.RightThumbstick.Up)
            {
                return "r.joystick.tilt.up.fill";
            }
            
            if (element == extendedGamepad.RightThumbstick.Down)
            {
                return "r.joystick.tilt.down.fill";
            }

            if (element == extendedGamepad.RightThumbstickButton)
            {
                return "r.joystick.press.down.fill";
            }
                
            if (element == extendedGamepad.RightThumbstick.XAxis 
                || element == extendedGamepad.RightThumbstick.YAxis) 
            { 
                return "r.joystick"; 
            }
            #endregion
            
            // Fallthrough to the micro-gamepad case
        } 
 
        
        var microGamepad = controller.MicroGamepad; 
        if (microGamepad != null) 
        { 
            if (element == microGamepad.ButtonA) 
            { 
                return "a.circle"; 
            } 
 
            if (element == microGamepad.ButtonX) 
            { 
                return "x.circle"; 
            } 
 
            if (element == microGamepad.ButtonMenu) 
            { 
                return "line.horizontal.3.circle"; 
            }

            if (element == microGamepad.Dpad.Down)
            {
                return "dpad.down.fill";
            }

            if (element == microGamepad.Dpad.Up)
            {
                return "dpad.up.fill";
            }

            if (element == microGamepad.Dpad.Left)
            {
                return "dpad.left.fill";
            }

            if (element == microGamepad.Dpad.Right)
            {
                return "dpad.right.fill";
            }

            if (microGamepad.DpadRing != null)
            {
                if (element == microGamepad.DpadRing.Down)
                {
                    return "dpad.down.fill";
                }

                if (element == microGamepad.DpadRing.Up)
                {
                    return "dpad.up.fill";
                }

                if (element == microGamepad.DpadRing.Left)
                {
                    return "dpad.left.fill";
                }

                if (element == microGamepad.DpadRing.Right)
                {
                    return "dpad.right.fill";
                }
            }
 
            if (element == microGamepad.Dpad.XAxis 
                || element == microGamepad.Dpad.YAxis 
                || element == microGamepad.DpadRing.XAxis
                || element == microGamepad.DpadRing.YAxis)
            { 
                return "dpad"; 
            } 
        }
 
        Debug.LogWarning($"no sf symbol is available for element: {element.LocalizedName}"); 
        return null; 
    }


    GCControllerElement GetGCElementForMicroGamepad([NotNull] InputControl inputControl)
    {
        if (inputControl == null) throw new ArgumentNullException(nameof(inputControl));
        
        var device = inputControl.device as MicroGamepadDevice;
        var microGamepad = controller.PhysicalInputProfile as GCMicroGamepad;

        if (microGamepad == null)
        {
            throw new InvalidProgramException("Unity Input device is not a MicroGamepadDevice");
        }

        if (inputControl == device.buttonA)
        {
            return microGamepad.ButtonA;
        }

        if (inputControl == device.buttonMenu)
        {
            return microGamepad.ButtonMenu;
        }

        if (inputControl == device.buttonDown)
        {
            return microGamepad.Dpad.Down;
        }

        if (inputControl == device.buttonUp)
        {
            return microGamepad.Dpad.Up;
        }

        if (inputControl == device.buttonLeft)
        {
            return microGamepad.Dpad.Left;
        }

        if (inputControl == device.buttonRight)
        {
            return microGamepad.Dpad.Right;
        }

        if (inputControl == device.xAxis)
        {
            return microGamepad.Dpad.XAxis;
        }

        if (inputControl == device.yAxis)
        {
            return microGamepad.Dpad.YAxis;
        }

        if (inputControl == device.ringXAxis)
        {
            return microGamepad.DpadRing.XAxis;
        }

        if (inputControl == device.ringYAxis)
        {
            return microGamepad.DpadRing.YAxis;
        }

        Debug.LogWarning(
            $"could not find apple controller element for unity element with name {GetUnityInputControlPath(inputControl)}");
        
        return null;
    }

    GCControllerElement GetGCElementForWhatIsProbablyASecondGenSiriRemote(InputControl inputControl)
    {
        if (inputControl == null) throw new ArgumentNullException(nameof(inputControl));
        
        var gamepad = inputControl.device as Gamepad;
        var extendedGamepad = controller.PhysicalInputProfile as GCMicroGamepad;

        if (extendedGamepad == null)
        {
            throw new InvalidProgramException("helper is not mapped to correct controller type");
        }

        #region Dpad
        if (inputControl == gamepad.dpad)
        {
            return extendedGamepad.Dpad;
        }
        
        // Fix for nimbus controller which does not report dPad.left/right
        // but dPad.x and dPad.y as axis values
        if (inputControl == gamepad.dpad.x)
        {
            return extendedGamepad.Dpad.XAxis;
        }

        if (inputControl == gamepad.dpad.y)
        {
            return extendedGamepad.Dpad.YAxis;
        }

        if (inputControl == gamepad.dpad.up)
        {
            return extendedGamepad.Dpad.Up;
        }

        if (inputControl == gamepad.dpad.down)
        {
            return extendedGamepad.Dpad.Down;
        }

        if (inputControl == gamepad.dpad.left)
        {
            return extendedGamepad.Dpad.Left;
        }

        if (inputControl == gamepad.dpad.right)
        {
            return extendedGamepad.Dpad.Right;
        }
        #endregion

        #region FaceButtons

        if (inputControl == gamepad.buttonSouth || inputControl == gamepad.crossButton || inputControl == gamepad.aButton)
        {
            return extendedGamepad.DpadRing.Down;
        }

        if (inputControl == gamepad.buttonEast || inputControl == gamepad.circleButton || inputControl == gamepad.bButton)
        {
            return extendedGamepad.DpadRing.Right;
        }

        if (inputControl == gamepad.buttonNorth || inputControl == gamepad.triangleButton || inputControl == gamepad.yButton)
        {
            return extendedGamepad.DpadRing.Up;
        }

        if (inputControl == gamepad.buttonWest || inputControl == gamepad.squareButton || inputControl == gamepad.xButton)
        {
            return extendedGamepad.DpadRing.Left;
        }
        #endregion
        
        #region MenuButtons
        if (inputControl == gamepad.startButton)
        {
            return extendedGamepad.ButtonMenu;
        }
        #endregion
        
        Debug.LogWarning(
            $"could not find apple controller element for unity element with name {GetUnityInputControlPath(inputControl)}");

        return null;
    }
    
    GCControllerElement GetGCElementForExtendedGamepad([NotNull] InputControl inputControl)
    {
        if (inputControl == null) throw new ArgumentNullException(nameof(inputControl));
        
        var gamepad = inputControl.device as Gamepad;
        var extendedGamepad = controller.PhysicalInputProfile as GCExtendedGamepad;

        if (extendedGamepad == null)
        {
            throw new InvalidProgramException("helper is not mapped to correct controller type");
        }

        #region Dpad
        if (inputControl == gamepad.dpad)
        {
            return extendedGamepad.Dpad;
        }
        
        // Fix for nimbus controller which does not report dPad.left/right
        // but dPad.x and dPad.y as axis values
        if (inputControl == gamepad.dpad.x)
        {
            return extendedGamepad.Dpad.XAxis;
        }

        if (inputControl == gamepad.dpad.y)
        {
            return extendedGamepad.Dpad.YAxis;
        }

        if (inputControl == gamepad.dpad.up)
        {
            return extendedGamepad.Dpad.Up;
        }

        if (inputControl == gamepad.dpad.down)
        {
            return extendedGamepad.Dpad.Down;
        }

        if (inputControl == gamepad.dpad.left)
        {
            return extendedGamepad.Dpad.Left;
        }

        if (inputControl == gamepad.dpad.right)
        {
            return extendedGamepad.Dpad.Right;
        }
        #endregion

        #region FaceButtons
        if (inputControl == gamepad.dpad.right)
        {
            return extendedGamepad.Dpad.Right;
        }

        if (inputControl == gamepad.dpad.up)
        {
            return extendedGamepad.Dpad.Up;
        }

        if (inputControl == gamepad.buttonSouth || inputControl == gamepad.crossButton || inputControl == gamepad.aButton)
        {
            return extendedGamepad.ButtonA;
        }

        if (inputControl == gamepad.buttonEast || inputControl == gamepad.circleButton || inputControl == gamepad.bButton)
        {
            return extendedGamepad.ButtonB;
        }

        if (inputControl == gamepad.buttonNorth || inputControl == gamepad.triangleButton || inputControl == gamepad.yButton)
        {
            return extendedGamepad.ButtonY;
        }

        if (inputControl == gamepad.buttonWest || inputControl == gamepad.squareButton || inputControl == gamepad.xButton)
        {
            return extendedGamepad.ButtonX;
        }
        #endregion

        #region ShoulderButtons
        if (inputControl == gamepad.leftShoulder)
        {
            return extendedGamepad.LeftShoulder;
        }

        if (inputControl == gamepad.rightShoulder)
        {
            return extendedGamepad.RightShoulder;
        }
        #endregion

        #region TriggerButtons
        if (inputControl == gamepad.leftTrigger)
        {
            return extendedGamepad.LeftTrigger;
        }

        if (inputControl == gamepad.rightTrigger)
        {
            return extendedGamepad.RightTrigger;
        }
        #endregion

        #region  LeftThumbStick
        if (inputControl == gamepad.leftStick)
        {
            return extendedGamepad.LeftThumbstick;
        }

        if (inputControl == gamepad.leftStick.down)
        {
            return extendedGamepad.LeftThumbstick.Down;
        }

        if (inputControl == gamepad.leftStick.left)
        {
            return extendedGamepad.LeftThumbstick.Left;
        }

        if (inputControl == gamepad.leftStick.right)
        {
            return extendedGamepad.LeftThumbstick.Right;
        }

        if (inputControl == gamepad.leftStick.up)
        {
            return extendedGamepad.LeftThumbstick.Up;
        }

        if (inputControl == gamepad.leftStick.x)
        {
            return extendedGamepad.LeftThumbstick.XAxis;
        }

        if (inputControl == gamepad.leftStick.y)
        {
            return extendedGamepad.LeftThumbstick.YAxis;
        }
        
        if (inputControl == gamepad.leftStickButton)
        {
            return extendedGamepad.LeftThumbstickButton;
        }
        #endregion

        #region RightThumbStick
        if (inputControl == gamepad.rightStick)
        {
            return extendedGamepad.RightThumbstick;
        }
        
        if (inputControl == gamepad.rightStick.down)
        {
            return extendedGamepad.RightThumbstick.Down;
        }

        if (inputControl == gamepad.rightStick.left)
        {
            return extendedGamepad.RightThumbstick.Left;
        }

        if (inputControl == gamepad.rightStick.right)
        {
            return extendedGamepad.RightThumbstick.Right;
        }

        if (inputControl == gamepad.rightStick.up)
        {
            return extendedGamepad.RightThumbstick.Up;
        }

        if (inputControl == gamepad.rightStick.x)
        {
            return extendedGamepad.RightThumbstick.XAxis;
        }

        if (inputControl == gamepad.rightStick.y)
        {
            return extendedGamepad.RightThumbstick.YAxis;
        }
        
        if (inputControl == gamepad.rightStickButton)
        {
            return extendedGamepad.RightThumbstickButton;
        }
        
        #endregion

        #region MenuButtons
        if (inputControl == gamepad.selectButton)
        {
            return extendedGamepad.ButtonOptions;
        }

        if (inputControl == gamepad.startButton)
        {
            return extendedGamepad.ButtonMenu;
        }
        #endregion
        
        Debug.LogWarning(
            $"could not find apple controller element for unity element with name {GetUnityInputControlPath(inputControl)}");

        return null;
    }
    
    GCControllerElement GetGCElementForDualShock4(InputControl inputControl)
    {
        var gamepad = inputControl.device as DualShockGamepad;
        var gcDualShockGamepad = controller.PhysicalInputProfile as GCDualShockGamepad;

        if (gcDualShockGamepad == null)
        {
            throw new InvalidProgramException("glyph helper is not mapped to correct type");
        }
        
        if (inputControl == gamepad.L1)
        {
            return gcDualShockGamepad.LeftShoulder;
        }

        if (inputControl == gamepad.R1)
        {
            return gcDualShockGamepad.RightShoulder;
        }

        if (inputControl == gamepad.L2)
        {
            return gcDualShockGamepad.LeftTrigger;
        }

        if (inputControl == gamepad.R2)
        {
            return gcDualShockGamepad.RightTrigger;
        }

        if (inputControl == gamepad.L3)
        {
            return gcDualShockGamepad.LeftThumbstick;
        }

        if (inputControl == gamepad.R3)
        {
            return gcDualShockGamepad.RightThumbstick;
        }

        if (inputControl == gamepad.optionsButton)
        {
            return gcDualShockGamepad.ButtonMenu;
        }

        if (inputControl == gamepad.shareButton)
        {
            return gcDualShockGamepad.ButtonOptions;
        }

        if (inputControl == gamepad.touchpadButton)
        {
            return gcDualShockGamepad.TouchpadButton;
        }

        // element is a standard extended gamepad button
        return GetGCElementForExtendedGamepad(inputControl);
    }

    GCControllerElement GetGCElementForXInputController(InputControl inputControl)
    {
        var xInputController = inputControl.device as XInputController;
        var xboxGamepad = controller.PhysicalInputProfile as GCXboxGamepad;

        if (xboxGamepad == null)
        {
            throw new InvalidProgramException("device is not mapped to correct type");
        }

        if (inputControl == xInputController.menu || inputControl == xInputController.startButton)
        {
            return xboxGamepad.ButtonMenu;
        }

        if (inputControl == xInputController.view || inputControl == xInputController.view)
        {
            return xboxGamepad.ButtonOptions;
        }
        
        // Fall through to regular controller case
        return GetGCElementForExtendedGamepad(inputControl);
    }
    
    public static string GetUnityInputControlPath(InputControl control)
    {
        var current = control;

        var controls = new List<InputControl>();
        while (current != null)
        {
            controls.Add(current);
            current = current.parent;
        }

        var longName = String.Join(".", controls.Select(x => x.name).ToArray().Reverse());
        return longName;
    }
} 
