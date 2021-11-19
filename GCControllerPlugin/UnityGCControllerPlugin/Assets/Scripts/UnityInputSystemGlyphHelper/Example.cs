using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HovelHouse.GameController;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    public Image anImage;

    private Gamepad lastSeenGamepad;
    private Dictionary<InputDevice,GlyphHelper> Helpers = new Dictionary<InputDevice, GlyphHelper>();
    private HashSet<InputDevice> SeenInputDevices = new HashSet<InputDevice>();
    private GlyphProvider _glyphProvider;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initialize");
        GameControllerPlugin.Initialize();
        
        // These events only fire for devices that are (dis)connected after the app launches
        // So they're useless for detecting already connected controllers
        GameControllerPlugin.SetControllerConnectedCallback(controller =>
        {
            Debug.Log("native controller connected");
        });

        GameControllerPlugin.SetControllerDisconnectedCallback(controller =>
        {
            Debug.Log("native controller disconnected");
        });
        
        InputSystem.onDeviceChange += (device, change) =>
        {
            Debug.Log($"unity input device change: {device.displayName} {change}");
            
            if (change == InputDeviceChange.Added)
            {
                OnUnityInputDeviceAdded(device);
            }
        };
        
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
        
        Debug.Log("OnUnityInptDeviceAdded: " + device.description.product);
        
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
            // IOS 14.0+ check, will fail on 13.x
            else if (device is XInputController)
            {
                Debug.Log("matched device to xbox profile");
                nativeController = allGamekitControllers.FirstOrDefault(
                    ctrl => ctrl.PhysicalInputProfile is GCXboxGamepad);
            }
            // IOS 14.0+ check, will fail on 13.x
            else if (device is DualShockGamepad)
            {
                Debug.Log("matched device to dual shock profile");
                nativeController = allGamekitControllers.FirstOrDefault(
                    ctrl => ctrl.PhysicalInputProfile is GCDualShockGamepad);
            }
            // Check for generic gamepad...
            // IOS 13.0+ check. On iOS 13.0 the SSFSymbolPropery of the controller elements is not
            // there, and returns null from the plugin
            else if (device is Gamepad)
            {
                Debug.Log("matched device to extended gamepad profile");
                nativeController = allGamekitControllers.First(
                    ctrl => ctrl.ExtendedGamepad != null);
            }
            else
            {
                Debug.Log($"Could not match unknown device: {device.shortDisplayName}");
            }

            // Matched our device
            if (nativeController != null)
            {
                var helper = new GlyphHelper(nativeController);
                Helpers[device] = helper;
                SeenInputDevices.Add(device);
                yield break;
            }
            
            yield return new WaitForFixedUpdate();
        }
    }
}
