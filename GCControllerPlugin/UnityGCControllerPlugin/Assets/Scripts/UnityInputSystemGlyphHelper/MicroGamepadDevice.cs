using System;
using System.Linq;
using System.Runtime.InteropServices;
using HovelHouse.GameController;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// A "state struct" describes the memory format that a Device uses. Each Device can
// receive and store memory in its custom format. InputControls then connect to
// the individual pieces of memory and read out values from them.
//
// If it's important for the memory format to match 1:1 at the binary level
// to an external representation, it's generally advisable to use
// LayoutLind.Explicit.
[StructLayout(LayoutKind.Explicit, Size = 32)]
public struct MicroGamepadDeviceState : IInputStateTypeInfo
{
    // You must tag every state with a FourCC code for type
    // checking. The characters can be anything. Choose something that allows
    // you to easily recognize memory that belongs to your own Device
    public FourCC format => new FourCC('S','I','R','I');

    // InputControlAttributes on fields tell the Input System to create Controls
    // for the public fields found in the struct.

    // Assume a 16bit field of buttons. Create one button that is tied to
    // bit #3 (zero-based). Note that buttons don't need to be stored as bits.
    // They can also be stored as floats or shorts, for example. The
    // InputControlAttribute.format property determines which format the
    // data is stored in. If omitted, the system generally infers it from the value
    // type of the field.
    [InputControl(name = "buttonMenu", layout = "Button", bit = 3)]
    [InputControl(name = "buttonA", layout = "Button", bit = 4)]
    [InputControl(name = "buttonX", layout = "Button", bit = 5)]
    [InputControl(name = "buttonLeft", layout = "Button", bit = 6)]
    [InputControl(name = "buttonRight", layout = "Button", bit = 7)]
    [InputControl(name = "buttonUp", layout = "Button", bit = 8)]
    [InputControl(name = "buttonDown", layout = "Button", bit = 9)]
    [FieldOffset(0x00)]
    public ushort buttons;

    // Create a floating-point axis. If a name is not supplied, it is taken
    // from the field.
    //[InputControl(name = "xAxis", layout = "Axis")]
    //public short xAxis;

    //[InputControl(name = "yAxis", layout = "Axis")]
    //public short yAxis;
}

// InputControlLayoutAttribute attribute is only necessary if you want
// to override the default behavior that occurs when you register your Device
// as a layout.
// The most common use of InputControlLayoutAttribute is to direct the system
// to a custom "state struct" through the `stateType` property. See below for details.
[InputControlLayout(displayName = "GCMicroGamepad", stateType = typeof(MicroGamepadDeviceState))]
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class MicroGamepadDevice : InputDevice, IInputUpdateCallbackReceiver
{
    // In the state struct, you added two Controls that you now want to
    // surface on the Device, for convenience. The Controls
    // get added to the Device either way. When you expose them as properties,
    // it is easier to get to the Controls in code.

    [InputControl]
    public ButtonControl buttonMenu { get; private set; }
    
    [InputControl]
    public ButtonControl buttonA { get; private set; }
    
    [InputControl]
    public ButtonControl buttonX { get; private set; }
    
    [InputControl]
    public ButtonControl buttonLeft { get; private set; }
    
    [InputControl]
    public ButtonControl buttonRight { get; private set; }
    
    [InputControl]
    public ButtonControl buttonUp { get; private set; }
    
    [InputControl]
    public ButtonControl buttonDown { get; private set; }

    public AxisControl xAxis { get; private set; }
    public AxisControl yAxis { get; private set; }

    private GCMicroGamepad gcMicroGamepad;
    
    // Cached pointer to certain elements
    private GCControllerDirectionPad DPadRing { get; set; }
    private GCControllerButtonInput CenterButton { get; set; }
    private GCControllerButtonInput TouchSurfaceButton { get; set; }

    static MicroGamepadDevice()
    {
        InputSystem.RegisterLayout<MicroGamepadDevice>();
    }
    
    // You still need a way to trigger execution of the static constructor
    // in the Player. To do this, you can add the RuntimeInitializeOnLoadMethod
    // to an empty method.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeInPlayer() {}

    // The Input System calls this method after it constructs the Device,
    // but before it adds the device to the system. Do any last-minute setup
    // here.
    protected override void FinishSetup()
    {
        base.FinishSetup();

        // NOTE: The Input System creates the Controls automatically.
        //       This is why don't do `new` here but rather just look
        //       the Controls up.
        buttonMenu = GetChildControl<ButtonControl>("buttonMenu");
        buttonA = GetChildControl<ButtonControl>("buttonA");
        buttonX = GetChildControl<ButtonControl>("buttonX");
        buttonLeft = GetChildControl<ButtonControl>("buttonLeft");
        buttonRight = GetChildControl<ButtonControl>("buttonRight");
        buttonUp = GetChildControl<ButtonControl>("buttonUp");
        buttonDown = GetChildControl<ButtonControl>("buttonDown");

        //xAxis = GetChildControl<AxisControl>("xAxis");
        //yAxis = GetChildControl<AxisControl>("yAxis");
    }

    [UsedImplicitly]
    public void OnUpdate()
    {
        // In practice, this would read out data from an external
        // API. This example uses some empty input.
        var state = new MicroGamepadDeviceState();
        if(gcMicroGamepad != null)
        {
            state.buttons |= (ushort) (
                ((gcMicroGamepad.ButtonMenu.Pressed ? 1 : 0) << 3)
                | ((gcMicroGamepad.ButtonA.Pressed ? 1 : 0) << 4)
                | ((gcMicroGamepad.ButtonX.Pressed ? 1 : 0) << 5)
                //| ((gcMicroGamepad.Dpad.Left.Pressed ? 1 : 0) << 4)
                //| ((gcMicroGamepad.Dpad.Right.Pressed ? 1 : 0) << 5)
                //| ((gcMicroGamepad.Dpad.Up.Pressed ? 1 : 0) << 6)
                //| ((gcMicroGamepad.Dpad.Down.Pressed ? 1 : 0) << 7)
            );

            if (DPadRing != null)
            {
                state.buttons |=
                    (ushort) (((DPadRing.Left.Pressed ? 1 : 0) << 6)
                    | ((DPadRing.Right.Pressed ? 1 : 0) << 7)
                    | ((DPadRing.Up.Pressed ? 1 : 0) << 8)
                    | ((DPadRing.Down.Pressed ? 1 : 0) << 9)
                    );
            }

            if(state.buttons != 0)
                Debug.Log($"state:{Convert.ToString(state.buttons, 2)}");
            //state.xAxis = gcMicroGamepad.xAxis.value;
            //state.yAxis = gcMicroGamepad.yAxis.value;
        }
        
        InputSystem.QueueStateEvent(this, state);
    }

    public void SetNativeController(GCController controller)
    {
        Debug.Log("Set native controller");
        gcMicroGamepad = controller.MicroGamepad;
        var elements = gcMicroGamepad.Elements;
        if (elements != null)
        {
            foreach (var id in elements)
            {
                if(id != null)
                    Debug.Log($"element '{id.Item1}'");
            }
            
            // Grab some specific elements that aren't part of the profile. ugh.
            DPadRing = elements
                .FirstOrDefault(x => x.Item1 == GameControllerPlugin.GCInputDirectionalCardinalDpad)
                ?.Item2 as GCControllerDirectionPad;

            TouchSurfaceButton = elements
                .FirstOrDefault(x => x.Item1 == GameControllerPlugin.GCInputDirectionalTouchSurfaceButton)
                ?.Item2 as GCControllerButtonInput;
            
            CenterButton = elements
                .FirstOrDefault(x => x.Item1 == GameControllerPlugin.GCInputDirectionalCenterButton)
                ?.Item2 as GCControllerButtonInput;
        }
    }
}
