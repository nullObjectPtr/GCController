using System.Collections.Generic;
using HovelHouse.GameController;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    public Image anImage;

    private Gamepad lastSeenGamepad;
    private Dictionary<InputDevice,GlyphHelper> Helpers = new Dictionary<InputDevice, GlyphHelper>();
    private HashSet<Gamepad> SeenGamepads = new HashSet<Gamepad>();
    private GlyphProvider _glyphProvider;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initialize");
        GameControllerPlugin.Initialize();
        _glyphProvider = new GlyphProvider(100.0, UIImageSymbolWeight.Bold );
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Gamepad.current != lastSeenGamepad && SeenGamepads.Contains(Gamepad.current) == false)
        {
            lastSeenGamepad = Gamepad.current;
            OnGamepadDetected(lastSeenGamepad);
        }

        foreach (var device in InputSystem.devices)
        {
            foreach (var control in device.allControls)
            {
                // Note that controls are hierarchical, so....sometimes we only
                // want to respond to child most elements or certain button presses
                // this is provided as an example only, you'll want to change this
                
                // Note that the parent elements will often actuate, and we usually don't
                // want the glyphs for these
                if (control.IsActuated())
                {
                    // Ignore certain parent controls....
                    if (IgnoreGlyphForControl(control))
                    {
                        continue;
                    }
                    if(Helpers.TryGetValue(control.device, out var helper))
                    {
                        var symbol = helper.GetSymbolNameForUnityElement(control);
                        Debug.Log($"{GlyphHelper.GetUnityInputControlPath(control)}{symbol}");
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

    private bool IgnoreGlyphForControl(InputControl control)
    {
        var gamepad = control.device as Gamepad;
        
        if (gamepad != null)
        {
            if (control == gamepad.leftStick || control == gamepad.rightStick)
            {
                return true;
            }

            if (control == gamepad.dpad)
            {
                return true;
            }
        }
        
        return false;
    }

    private void OnGamepadDetected(Gamepad gamepad)
    {
        var desc = gamepad.description;
        Debug.Log($"new gamepad detected: {desc.product} {desc.version}");
        SeenGamepads.Add(gamepad);
        
        // Optionally - preload all the glyphs for this controller
        // (will probably result in a hitch)
        // Otherwise the glyphs will be loaded as they are requested
        //_glyphProvider.LoadTexturesForController(GCController.Current);
        
        // Match it up...these should be the same...but I can't say for 100%
        // sure that if two controllers were pressed at roughly the same time
        // that the current value from unity would match the current controller
        // reported from iOS. Depends on how these values are updated
        var helper = new GlyphHelper(GCController.Current);
        Helpers[gamepad] = helper;
    }
}
