using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HovelHouse.GameController;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    public Image anImage;
    private Dictionary<InputDevice,GlyphHelper> Helpers = new Dictionary<InputDevice, GlyphHelper>();
    private GlyphProvider _glyphProvider;
    
    private const string INTERFACE_NAME = "MicroGamepad";

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initialize");
        
        #if UNITY_TVOS
        UnityEngine.tvOS.Remote.allowExitToHome = false;
        #endif
        
        // Register our custom virtual controller for the second gen siri remote
        InputSystem.RegisterLayout<MicroGamepadDevice>(
            matches: new InputDeviceMatcher()
                .WithInterface(INTERFACE_NAME));
        
        GameControllerPlugin.Initialize();

        // Start the device matching process. For every unity input system device
        // we need to match it up with a native device from the GCController framework
        // to get the glyphs working
        // 1. Process any already connected devices
        // 2. Add a listener to newly connected devices
        // 3. Some additional setup for the siri remote
        
        // Matching unity input devices
        foreach (var inputDevice in InputSystem.devices)
        {
            Debug.Log($"adding already connected input device: {inputDevice.displayName}");
            OnUnityInputDeviceAdded(inputDevice);
        }

        InputSystem.onDeviceChange += (device, change) =>
        {
            Debug.Log($"unity input device change: {device.displayName} {change}");
            
            if (change == InputDeviceChange.Added)
            {
                OnUnityInputDeviceAdded(device);
            }
        };

        // Set up siri remotes
        foreach (var nativeDevice in GCController.Controllers())
        {
            OnNativeInputDeviceAdded(nativeDevice);
        }
        
        GameControllerPlugin.SetControllerConnectedCallback(OnNativeInputDeviceAdded);
        
        _glyphProvider = new GlyphProvider(100.0, UIImageSymbolWeight.Bold );
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (var device in InputSystem.devices)
        {
            if(Helpers.TryGetValue(device, out var helper))
            {
                foreach (var control in device.allControls)
                {
                    // Note that controls are hierarchical, so....sometimes we only
                    // want to respond to child most elements or certain button presses
                    // this is provided as an example only, you'll want to change this
                
                    if (control.IsActuated())
                    {
                        Debug.Log($"{GlyphHelper.GetUnityInputControlPath(control)}");

                        // Note that the parent elements actuate if their children do
                        // and we usually don't want the glyphs for these
                        // Ignore certain parent controls....
                        // You'll want to make sure of this in your code too
                        if (IgnoreGlyphForControl(control))
                        {
                            continue;
                        }
                        
                        var symbol = helper.GetSymbolNameForUnityElement(control);
                        Debug.Log($"-{symbol}-");
                        
                        if (string.IsNullOrEmpty(symbol) == false)
                        {
                            var sprite = _glyphProvider.GetSprite(symbol, false);

                            if (sprite != null)
                            {
                                Debug.Log(symbol);
                                anImage.sprite = sprite;
                            }
                        }
                    }
                }
            }
        }
    }

    private bool IgnoreGlyphForControl(InputControl control)
    {
        var gamepad = control.device as Gamepad;
        
        if (gamepad != null)
        {
            if (control == gamepad.leftStick 
                || control == gamepad.rightStick 
                || control == gamepad.leftStick.x
                || control == gamepad.leftStick.y
                || control == gamepad.rightStick.x
                || control == gamepad.rightStick.y)
            {
                return true;
            }

            if (control == gamepad.dpad || control == gamepad.dpad.x || control == gamepad.dpad.y)
            {
                return true;
            }
        }

        var microGamepad = control.device as MicroGamepadDevice;
        if (microGamepad != null)
        {
            if (control == microGamepad.ringXAxis || control == microGamepad.ringYAxis)
            {
                return true;
            }
        }
            
        
        return false;
    }

    private void OnUnityInputDeviceAdded(InputDevice device)
    {
        // We need to match the unity device to a native gamekit device
        // (which may not be available yet) so run an async task to match them
        
        Debug.Log("OnUnityInputDeviceAdded: " + device.description.product);
        
        // There may be some unity controllers we don't want to match
        // Don't match the HID best-guess version of the SiriRemote
        // We wrote a custom virtual controller for this, and we don't want to double
        // process it
        // That controller is NOT a gamepad, it inherits from input device
        var shouldIgnoreMatchingThisDevice = device is Gamepad 
                                     && device.description.product.Contains("Remote");

        if (shouldIgnoreMatchingThisDevice) return;
        
        // Otherwise, we should create a glyph helper for it
        StartCoroutine(MatchDeviceCo(device));
    }

    private void OnNativeInputDeviceAdded(GCController nativeController)
    {
        // Additional setup is needed for the 2nd gen sire remote
        // Check for it here...
                
        // Keep in mind that extended gamepads are ALSO micro gamepads
        // so when we want to detect if a siri remote was connected
        // we should rule out that a controller was connected first
        
        var microGamepadConnected = nativeController.ExtendedGamepad == null 
                                    && nativeController.MicroGamepad != null;
        
        if (microGamepadConnected)
        {
            Debug.Log("MicroGamepadConnected");
            var device = InputSystem.AddDevice(
                new InputDeviceDescription
                {
                    interfaceName = INTERFACE_NAME,
                    product = nativeController.ProductCategory,
                });

            if (device is MicroGamepadDevice microGamepad)
            {
                microGamepad.SetNativeController(nativeController);
            }
        }
        
        Debug.Log("Device Connected: " + nativeController.VendorName);

    }

    private IEnumerator MatchDeviceCo(InputDevice device)
    {
        var allGamekitControllers = GCController.Controllers();
        while (Helpers.ContainsKey(device) == false)
        {
            GCController nativeController = null;

            if (device is MicroGamepadDevice)
            {
                Debug.Log("matched device to micro gamepad profile");
                nativeController = allGamekitControllers.FirstOrDefault(
                    ctrl => ctrl.ExtendedGamepad == null
                            && ctrl.MicroGamepad != null
                            && ctrl.MicroGamepad.DpadRing != null);
            }
            else if (device is XInputController)
            {
                Debug.Log("matched device to xbox profile");

                // the GCXBoxGamepad is not available to check on iOS 13.x...
                // so fallback to checking if the inputProfileIs null and the vendor name for a substring to match
                nativeController = allGamekitControllers.FirstOrDefault(
                    ctrl => (ctrl.PhysicalInputProfile is GCXboxGamepad) 
                        || (ctrl.PhysicalInputProfile == null && ctrl.VendorName.IndexOf("xbox", StringComparison.OrdinalIgnoreCase) > 0)
                    );
            }
            else if (device is DualShockGamepad)
            {
                Debug.Log("matched device to dual shock profile");
                
                // the GCDualShockGamepad profile is not available to check on iOS 13.x...
                // so fallback to checking if the inputProfileIs null and the vendor name for a substring to match
                nativeController = allGamekitControllers.FirstOrDefault(
                    ctrl => (ctrl.PhysicalInputProfile is GCDualShockGamepad) 
                        || (ctrl.PhysicalInputProfile == null && ctrl.VendorName.IndexOf("dualshock", StringComparison.OrdinalIgnoreCase) >0 )
                    );
            }
            else if (device is Gamepad)
            {
                Debug.Log("matched device to extended gamepad profile");
                nativeController = allGamekitControllers.First(
                    ctrl => ctrl.ExtendedGamepad != null);
            }
            else
            {
                Debug.Log($"Could not match unknown device: {device.shortDisplayName}");
                yield break;
            }

            // Matched our device
            if (nativeController != null)
            {
                var helper = new GlyphHelper(nativeController);
                Helpers[device] = helper;
                yield break;
            }
            
            yield return new WaitForFixedUpdate();
        }
    }
}
