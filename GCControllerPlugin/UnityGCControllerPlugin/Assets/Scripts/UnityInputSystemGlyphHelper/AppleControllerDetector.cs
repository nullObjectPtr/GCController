using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HovelHouse.GameController;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

public class AppleControllerDetector : MonoBehaviour
{
    private List<GCController> ConnectedControllers = new List<GCController>();

    private const string INTERFACE_NAME = "MicroGamepad";
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Add a match that catches any Input Device that reports its
        // interface as "ThirdPartyAPI".
        InputSystem.RegisterLayout<MicroGamepadDevice>(
            matches: new InputDeviceMatcher()
                .WithInterface(INTERFACE_NAME));
        
        Debug.Log("Starting to scan for controllers...");

        while (true)
        {
            var controllers = GCController.Controllers();
            if (controllers == null)
            {
                continue;
            }
            
            foreach (var controller in controllers)
            {
                if (ConnectedControllers.Contains(controller) == false)
                {
                    ConnectedControllers.Add(controller);
                    OnDeviceConnected(controller);
                }
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    private void OnDeviceConnected(GCController controller)
    {
        Debug.Log("Device Connected: " + controller.VendorName);
        
        // Keep in mind that extended gamepads are ALSO micro gamepads
        // so when we want to detect if a siri remote was connected
        // we should rule out that a controller was connected first
        var microGamepadConnected = controller.ExtendedGamepad == null 
                                    && controller.MicroGamepad != null;
        
        if (microGamepadConnected)
        {
            Debug.Log("MicroGamepadConnected");
            var device = InputSystem.AddDevice(
                new InputDeviceDescription
                {
                    interfaceName = INTERFACE_NAME,
                    product = controller.ProductCategory,
                });

            if (device is MicroGamepadDevice microGamepad)
            {
                microGamepad.SetNativeController(controller);
            }
        }
    }

    private void OnDeviceRemoved(GCController controller)
    {
        var device = InputSystem.devices.FirstOrDefault(
            x => x.description == new InputDeviceDescription
            {
                interfaceName = INTERFACE_NAME,
                product = controller.ProductCategory,
            });

        if (device != null)
            InputSystem.RemoveDevice(device);
    }
}
