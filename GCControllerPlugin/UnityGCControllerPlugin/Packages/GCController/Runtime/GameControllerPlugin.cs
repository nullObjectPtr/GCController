//
//  ExecutionContext.cs
//
//  Created by Jonathan Culp <jonathanculp@gmail.com> on 05/14/2020
//  Copyright © 2020 HovelHouseApps. All rights reserved.
//  Unauthorized copying of this file, via any medium is strictly prohibited
//  Proprietary and confidential
//

using System;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using UnityEngine;

namespace HovelHouse.GameController
{
            
    public delegate void GCControllerDelegate(IntPtr controllerPtr);
    
    public class GameControllerPlugin
    {
#if !UNITY_EDITOR && UNITY_IPHONE || UNITY_TVOS
        const string dll = "__Internal";
#else
        const string dll = "HHGCControllerMacOS";
#endif

        // Class Methods
        [DllImport(dll)]
        private static extern void RegisterDelegateFor_GCController_ControllerConnected(
            GCControllerDelegate del
            );

        [DllImport(dll)]
        private static extern void RegisterDelegateFor_GCController_ControllerDisconnected(
            GCControllerDelegate del
            );
        
        [DllImport(dll)]
        private static extern void GCControllerPluignInit();
        
        private static ExecutionContext<GCController> ConnectedCallbackContext;
        private static ExecutionContext<GCController> DisconnectedCallbackContext;
        
        public static Action<GCController> OnControllerConnected;
        public static Action<GCController> OnControllerDisconnected;

        // TODO should you get these from the API? They may be const but the value
        // could change between API releases, and these could be outdated
        public const string GCInputDirectionalDpad = "Direction Pad";
        public const string GCInputDirectionalCardinalDpad = "Cardinal Direction Pad";
        public const string GCInputDirectionalTouchSurfaceButton = "Button A";
        public const string GCInputDirectionalCenterButton = "Button Center";

        public static void Initialize()
        {
            ConnectedCallbackContext = new ExecutionContext<GCController>(OnControllerConnected);
            DisconnectedCallbackContext = new ExecutionContext<GCController>(OnControllerDisconnected);
            
            GCControllerPluignInit();
            //RegisterDelegateFor_GCController_ControllerConnected(ControllerConnectedNativeCallback);
            //RegisterDelegateFor_GCController_ControllerDisconnected(ControllerDisconnectedNativeCallback);
        }

        [MonoPInvokeCallback(typeof(GCControllerDelegate))]
        public static void ControllerConnectedNativeCallback(IntPtr controllerPtr)
        {
            try
            {
                ConnectedCallbackContext?.Invoke(
                    controllerPtr != IntPtr.Zero ? new GCController(controllerPtr) : null);
            }
            catch (Exception ex)
            {
                Debug.Log("managed callback threw an exception: " + ex.ToString());
            }
        }

        [MonoPInvokeCallback(typeof(GCControllerDelegate))]
        public static void ControllerDisconnectedNativeCallback(IntPtr controllerPtr)
        {
            try
            {
                DisconnectedCallbackContext?.Invoke(
                    controllerPtr != IntPtr.Zero ? new GCController(controllerPtr) : null);
            }
            catch (Exception ex)
            {
                Debug.Log("managed callback threw an exception: " + ex.ToString());
            }
        }
    }
}
