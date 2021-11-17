using System.Collections.Generic;
using System.Linq;
using HovelHouse.GameController;
using UnityEngine;
using UnityEngine.InputSystem;
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
        // Whenever a new unity input gamepad becomes current
        // set up it's glyph helper by trying to match it up with the
        // current native GameKit gamepad
        if (Gamepad.current != lastSeenGamepad && SeenInputDevices.Contains(Gamepad.current) == false)
        {
            Debug.Log($"last seen gamepad: {Gamepad.current}");
            lastSeenGamepad = Gamepad.current;
            OnGamepadDetected(lastSeenGamepad);
        }

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
        
        return false;
    }

    private void OnGamepadDetected(InputDevice inputDevice)
    {
        var desc = inputDevice.description;
        Debug.Log($"new gamepad detected: {desc.product} {desc.version}");
        Debug.Log($"matching with {GCController.Current.ProductCategory}");
        
        SeenInputDevices.Add(inputDevice);

        // Don't create an adapter for 2nd gen remote here
        if (desc.product.Contains("Remote"))
            return;
        
        // Optionally - preload all the glyphs for this controller
        // (will probably result in a hitch)
        // Otherwise the glyphs will be loaded as they are requested
        //_glyphProvider.LoadTexturesForController(GCController.Current);
        
        // Match it up...these should be the same...but I can't say for 100%
        // sure that if two controllers were pressed at roughly the same time
        // that the current value from unity would match the current controller
        // reported from iOS. Depends on how these values are updated
        var helper = new GlyphHelper(GCController.Current);
        Helpers[inputDevice] = helper;
    }

    private void OnUnityInputDeviceAdded(InputDevice device)
    {
        Debug.Log("OnUnityInptDeviceAdded: " + device.description.product);
        if (device is MicroGamepadDevice)
        {
            var first2ndGenSiriRemoteFound = GCController.Controllers().FirstOrDefault(
                ctrl => ctrl.ExtendedGamepad == null
                        && ctrl.MicroGamepad != null
                        && ctrl.MicroGamepad.DpadRing != null);

            if (first2ndGenSiriRemoteFound != null)
            {
                var helper = new GlyphHelper(first2ndGenSiriRemoteFound);
                Helpers[device] = helper;
                SeenInputDevices.Add(device);
            }
        }
    }
}
