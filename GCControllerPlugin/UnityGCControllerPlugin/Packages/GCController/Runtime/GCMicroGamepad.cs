//
//  GCMicroGamepad.cs
//
//  Created by Jonathan Culp <jonathanculp@gmail.com> on
//  Copyright © 2021 HovelHouseApps. All rights reserved.
//  Unauthorized copying of this file, via any medium is strictly prohibited
//  Proprietary and confidential
//

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace HovelHouse.GameController
{
    /// <summary>
    /// 
    /// </summary>
    public class GCMicroGamepad : GCPhysicalInputProfile, IDisposable
    {
        #region dll
        
        #if !UNITY_EDITOR && UNITY_IPHONE || UNITY_TVOS
        const string dll = "__Internal";
        #else
        const string dll = "HHGCControllerMacOS";
        #endif

        // Class Methods
        

        

        

        

        // Properties
        
        [DllImport(dll)]
        private static extern IntPtr GCMicroGamepad_GetPropController(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCMicroGamepad_GetPropDpad(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern bool GCMicroGamepad_GetPropReportsAbsoluteDpadValues(
            HandleRef ptr, 
            out IntPtr exceptionPtr);
        
        [DllImport(dll)]
        private static extern void GCMicroGamepad_SetPropReportsAbsoluteDpadValues(HandleRef ptr, bool reportsAbsoluteDpadValues, out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern bool GCMicroGamepad_GetPropAllowsRotation(
            HandleRef ptr, 
            out IntPtr exceptionPtr);
        
        [DllImport(dll)]
        private static extern void GCMicroGamepad_SetPropAllowsRotation(HandleRef ptr, bool allowsRotation, out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCMicroGamepad_GetPropButtonA(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCMicroGamepad_GetPropButtonX(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCMicroGamepad_GetPropButtonMenu(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

                [DllImport(dll)]
        private static extern void GCMicroGamepad_SetPropValueChangedHandler(HandleRef ptr, GCMicroGamepadValueChangedDelegate valueChangedHandler, out IntPtr exceptionPtr);

        

        #endregion

        internal GCMicroGamepad(IntPtr ptr) : base(ptr) {}
        internal GCMicroGamepad(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        
        


        
        
        
        /// <value>Controller</value>
        private GCController _controller;
        public GCController Controller
        {
            get
            {
                var controller = GCMicroGamepad_GetPropController(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_controller == null || controller != (IntPtr)_controller.Handle)
                {
                    _controller = controller == IntPtr.Zero ? null : new GCController(controller, ERetainPolicy.Unretained);
                }
                
                return _controller;
            }
        }

        /// <value>Dpad</value>
        private GCControllerDirectionPad _dpad;
        public GCControllerDirectionPad Dpad
        {
            get
            {
                var dpad = GCMicroGamepad_GetPropDpad(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_dpad == null || dpad != (IntPtr)_dpad.Handle)
                {
                    _dpad = dpad == IntPtr.Zero ? null : new GCControllerDirectionPad(dpad, ERetainPolicy.Unretained);
                }
                
                return _dpad;
            }
        }
        
        private GCControllerDirectionPad _dpadRing;

        /**
         * returns the dPad ring controller element if this is a 2nd gen siri remote
         * if it's not, this will return null;
         */
        public GCControllerDirectionPad DpadRing
        {
            get
            {
                if (_dpadRing == null)
                {
                    // The apple API has no hard reference to the new controller elements on the
                    // 2nd gen siri remote, so we need to retrieve them by name, we use the 
                    // const string from the API to do this
                    _dpadRing = Elements
                        .FirstOrDefault(x => x.Item1 == GameControllerPlugin.GCInputDirectionalCardinalDpad)
                        ?.Item2 as GCControllerDirectionPad;
                }

                return _dpadRing;
            }
        }

        /// <value>ReportsAbsoluteDpadValues</value>
        public bool ReportsAbsoluteDpadValues
        {
            get
            {
                bool reportsAbsoluteDpadValues = GCMicroGamepad_GetPropReportsAbsoluteDpadValues(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return reportsAbsoluteDpadValues;
            }
            set
            {
                GCMicroGamepad_SetPropReportsAbsoluteDpadValues(Handle, value, out IntPtr exceptionPtr);
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
            }
        }

        
        /// <value>AllowsRotation</value>
        public bool AllowsRotation
        {
            get
            {
                bool allowsRotation = GCMicroGamepad_GetPropAllowsRotation(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return allowsRotation;
            }
            set
            {
                GCMicroGamepad_SetPropAllowsRotation(Handle, value, out IntPtr exceptionPtr);
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
            }
        }

        
        /// <value>ButtonA</value>
        private GCControllerButtonInput _buttonA;
        public GCControllerButtonInput ButtonA
        {
            get
            {
                var buttonA = GCMicroGamepad_GetPropButtonA(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_buttonA == null || buttonA != (IntPtr)_buttonA.Handle)
                {
                    _buttonA = buttonA == IntPtr.Zero ? null : new GCControllerButtonInput(buttonA, ERetainPolicy.Unretained);
                }
                
                return _buttonA;
            }
        }

        
        /// <value>ButtonX</value>
        private GCControllerButtonInput _buttonX;
        public GCControllerButtonInput ButtonX
        {
            get
            {
                var buttonX = GCMicroGamepad_GetPropButtonX(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_buttonX == null || buttonX != (IntPtr)_buttonX.Handle)
                {
                    _buttonX = buttonX == IntPtr.Zero ? null : new GCControllerButtonInput(buttonX, ERetainPolicy.Unretained);
                }
                
                return _buttonX;
            }
        }

        
        /// <value>ButtonMenu</value>
        private GCControllerButtonInput _buttonMenu;
        public GCControllerButtonInput ButtonMenu
        {
            get
            {
                var buttonMenu = GCMicroGamepad_GetPropButtonMenu(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_buttonMenu == null || buttonMenu != (IntPtr)_buttonMenu.Handle)
                {
                    _buttonMenu = buttonMenu == IntPtr.Zero ? null : new GCControllerButtonInput(buttonMenu, ERetainPolicy.Unretained);
                }
                
                return _buttonMenu;
            }
        }

        
        /// <value>ValueChangedHandler</value>
        public Action<GCMicroGamepad,GCControllerElement> ValueChangedHandler
        {
            get 
            {
                if(ValueChangedHandlerCallbacks.TryGetValue(
                    HandleRef.ToIntPtr(Handle), 
                    out ExecutionContext<GCMicroGamepad,GCControllerElement> value))
                {
                    return value.Callback;
                }
                
                return null;
            }    
            set 
            {
                IntPtr myPtr = HandleRef.ToIntPtr(Handle);
                if(value == null)
                {
                    ValueChangedHandlerCallbacks.Remove(myPtr);
                }
                else
                {
                    ValueChangedHandlerCallbacks[myPtr] = new ExecutionContext<GCMicroGamepad,GCControllerElement>(value);
                }
                GCMicroGamepad_SetPropValueChangedHandler(Handle, ValueChangedHandlerCallback, out IntPtr exceptionPtr);

                if(exceptionPtr != IntPtr.Zero)
                {
                    var nativeException = new NSException(exceptionPtr);
                    throw new GameControllerException(nativeException, nativeException.Reason);
                }
            }
        }
        
        // TODO - this could (theoretically) grow without bound, figure out a way to free-up
        // old elements once the controller's that owns them go out of scope
        private static Dictionary<IntPtr, GCControllerElement> elementCache =
            new Dictionary<IntPtr, GCControllerElement>();

        private static Dictionary<IntPtr, GCMicroGamepad> profileCache =
            new Dictionary<IntPtr, GCMicroGamepad>();

        private static readonly Dictionary<IntPtr,ExecutionContext<GCMicroGamepad,GCControllerElement>> ValueChangedHandlerCallbacks = new Dictionary<IntPtr,ExecutionContext<GCMicroGamepad,GCControllerElement>>();

        [MonoPInvokeCallback(typeof(GCMicroGamepadValueChangedDelegate))]
        private static void ValueChangedHandlerCallback(IntPtr thisPtr, IntPtr gamepadPtr, IntPtr elementPtr, long elementType)
        {
            if(ValueChangedHandlerCallbacks.TryGetValue(thisPtr, out ExecutionContext<GCMicroGamepad,GCControllerElement> callback))
            {
                GCMicroGamepad gamepad = null;
                GCControllerElement element = null;

                if (elementPtr != IntPtr.Zero && elementCache.TryGetValue(elementPtr, out element) == false)
                {
                    switch (elementType)
                    {
                        case 1:
                            element = new GCControllerButtonInput(elementPtr, ERetainPolicy.Unretained);
                            break;
                        case 2:
                            element = new GCControllerAxisInput(elementPtr, ERetainPolicy.Unretained);
                            break;
                        case 3:
                            // element = new GCDeviceCursor(elementPtr);
                            break;
                        case 4:
                            element = new GCControllerDirectionPad(elementPtr, ERetainPolicy.Unretained);
                            break;
                        case 5:
                            // element = new GCControllerTouchpad(elementPtr, RetainPolicy.Unretained);
                            break;
                        default:
                            element = new GCControllerElement(elementPtr, ERetainPolicy.Unretained);
                            break;
                    }
                    
                    elementCache[elementPtr] = element;
                }

                if (gamepadPtr != IntPtr.Zero && profileCache.TryGetValue(elementPtr, out gamepad) == false)
                {
                    gamepad = new GCMicroGamepad(gamepadPtr, ERetainPolicy.Unretained);
                    profileCache[gamepadPtr] = gamepad;
                }
                
                callback.Invoke(gamepad,element);
            }
        }

        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void GCMicroGamepad_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        protected override void Dispose(bool disposing)
        {
            if (_retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("GCMicroGamepad Dispose");
                GCMicroGamepad_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~GCMicroGamepad()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public new void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        
    }
}
