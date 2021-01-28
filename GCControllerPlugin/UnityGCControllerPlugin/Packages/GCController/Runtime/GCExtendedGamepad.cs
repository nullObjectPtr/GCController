//
//  GCExtendedGamepad.cs
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
    public class GCExtendedGamepad : GCPhysicalInputProfile, IDisposable
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
        private static extern IntPtr GCExtendedGamepad_GetPropController(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        [DllImport(dll)]
        private static extern void GCExtendedGamepad_SetPropValueChangedHandler(HandleRef ptr, GCExtendedGamepadValueChangedDelegate valueChangedHandler, out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropLeftShoulder(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropRightShoulder(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropDpad(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropButtonA(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropButtonB(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropButtonX(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropButtonY(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropLeftThumbstick(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropRightThumbstick(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropLeftTrigger(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropRightTrigger(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropButtonHome(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropButtonMenu(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropButtonOptions(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropLeftThumbstickButton(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCExtendedGamepad_GetPropRightThumbstickButton(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        

        #endregion

        internal GCExtendedGamepad(IntPtr ptr) : base(ptr) {}
        internal GCExtendedGamepad(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        
        


        
        
        
        /// <value>Controller</value>
        private GCController _controller;
        public GCController Controller
        {
            get
            {
                var controller = GCExtendedGamepad_GetPropController(Handle, out var exceptionPtr);
                
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

        
        /// <value>ValueChangedHandler</value>
        public Action<GCExtendedGamepad,GCControllerElement> ValueChangedHandler
        {
            get 
            {
                if(ValueChangedHandlerCallbacks.TryGetValue(
                    HandleRef.ToIntPtr(Handle), 
                    out ExecutionContext<GCExtendedGamepad,GCControllerElement> value))
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
                    ValueChangedHandlerCallbacks[myPtr] = new ExecutionContext<GCExtendedGamepad,GCControllerElement>(value);
                }
                GCExtendedGamepad_SetPropValueChangedHandler(Handle, ValueChangedHandlerCallback, out IntPtr exceptionPtr);

                if(exceptionPtr != IntPtr.Zero)
                {
                    var nativeException = new NSException(exceptionPtr);
                    throw new GameControllerException(nativeException, nativeException.Reason);
                }
            }
        }

        private static readonly Dictionary<IntPtr,ExecutionContext<GCExtendedGamepad,GCControllerElement>> ValueChangedHandlerCallbacks = new Dictionary<IntPtr,ExecutionContext<GCExtendedGamepad,GCControllerElement>>();
        
        // TODO - this could (theoretically) grow without bound, figure out a way to free-up
        // old elements once the controller's that owns them go out of scope
        private static Dictionary<IntPtr, GCControllerElement> elementCache =
            new Dictionary<IntPtr, GCControllerElement>();

        private static Dictionary<IntPtr, GCExtendedGamepad> profileCache =
            new Dictionary<IntPtr, GCExtendedGamepad>();
        
        [MonoPInvokeCallback(typeof(GCExtendedGamepadValueChangedDelegate))]
        private static void ValueChangedHandlerCallback(IntPtr thisPtr, IntPtr gamepadPtr, long gamepadType, IntPtr elementPtr, long elementType)
        {
            if(ValueChangedHandlerCallbacks.TryGetValue(thisPtr, out ExecutionContext<GCExtendedGamepad,GCControllerElement> callback))
            {
                GCControllerElement element = null;
                GCExtendedGamepad gamepad = null;
                
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
                    switch (gamepadType)
                    {
                        case 1:
                            gamepad = new GCDualShockGamepad(gamepadPtr, ERetainPolicy.Unretained);
                            break;
                        case 2:
                            gamepad = new GCXboxGamepad(gamepadPtr, ERetainPolicy.Unretained);
                            break;
                        default:
                            gamepad = new GCExtendedGamepad(gamepadPtr, ERetainPolicy.Unretained);
                            break;
                    }

                    profileCache[gamepadPtr] = gamepad;
                }
                
                callback.Invoke(gamepad,element);
            }
        }

        
        /// <value>LeftShoulder</value>
        private GCControllerButtonInput _leftShoulder;
        public GCControllerButtonInput LeftShoulder
        {
            get
            {
                var leftShoulder = GCExtendedGamepad_GetPropLeftShoulder(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_leftShoulder == null || leftShoulder != (IntPtr)_leftShoulder.Handle)
                {
                    _leftShoulder = leftShoulder == IntPtr.Zero ? null : new GCControllerButtonInput(leftShoulder, ERetainPolicy.Unretained);
                }
                
                return _leftShoulder;
            }
        }

        
        /// <value>RightShoulder</value>
        private GCControllerButtonInput _rightShoulder;
        public GCControllerButtonInput RightShoulder
        {
            get
            {
                var rightShoulder = GCExtendedGamepad_GetPropRightShoulder(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_rightShoulder == null || rightShoulder != (IntPtr)_rightShoulder.Handle)
                {
                    _rightShoulder = rightShoulder == IntPtr.Zero ? null : new GCControllerButtonInput(rightShoulder, ERetainPolicy.Unretained);
                }
                
                return _rightShoulder;
            }
        }

        
        /// <value>Dpad</value>
        private GCControllerDirectionPad _dpad;
        public GCControllerDirectionPad Dpad
        {
            get
            {
                var dpad = GCExtendedGamepad_GetPropDpad(Handle, out var exceptionPtr);
                
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

        
        /// <value>ButtonA</value>
        private GCControllerButtonInput _buttonA;
        public GCControllerButtonInput ButtonA
        {
            get
            {
                var buttonA = GCExtendedGamepad_GetPropButtonA(Handle, out var exceptionPtr);
                
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

        
        /// <value>ButtonB</value>
        private GCControllerButtonInput _buttonB;
        public GCControllerButtonInput ButtonB
        {
            get
            {
                var buttonB = GCExtendedGamepad_GetPropButtonB(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_buttonB == null || buttonB != (IntPtr)_buttonB.Handle)
                {
                    _buttonB = buttonB == IntPtr.Zero ? null : new GCControllerButtonInput(buttonB, ERetainPolicy.Unretained);
                }
                
                return _buttonB;
            }
        }

        
        /// <value>ButtonX</value>
        private GCControllerButtonInput _buttonX;
        public GCControllerButtonInput ButtonX
        {
            get
            {
                var buttonX = GCExtendedGamepad_GetPropButtonX(Handle, out var exceptionPtr);
                
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

        
        /// <value>ButtonY</value>
        private GCControllerButtonInput _buttonY;
        public GCControllerButtonInput ButtonY
        {
            get
            {
                var buttonY = GCExtendedGamepad_GetPropButtonY(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_buttonY == null || buttonY != (IntPtr)_buttonY.Handle)
                {
                    _buttonY = buttonY == IntPtr.Zero ? null : new GCControllerButtonInput(buttonY, ERetainPolicy.Unretained);
                }
                
                return _buttonY;
            }
        }

        
        /// <value>LeftThumbstick</value>
        private GCControllerDirectionPad _leftThumbstick;
        public GCControllerDirectionPad LeftThumbstick
        {
            get
            {
                var leftThumbstick = GCExtendedGamepad_GetPropLeftThumbstick(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_leftThumbstick == null || leftThumbstick != (IntPtr)_leftThumbstick.Handle)
                {
                    _leftThumbstick = leftThumbstick == IntPtr.Zero ? null : new GCControllerDirectionPad(leftThumbstick, ERetainPolicy.Unretained);
                }
                
                return _leftThumbstick;
            }
        }

        
        /// <value>RightThumbstick</value>
        private GCControllerDirectionPad _rightThumbstick;
        public GCControllerDirectionPad RightThumbstick
        {
            get
            {
                var rightThumbstick = GCExtendedGamepad_GetPropRightThumbstick(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_rightThumbstick == null || rightThumbstick != (IntPtr)_rightThumbstick.Handle)
                {
                    _rightThumbstick = rightThumbstick == IntPtr.Zero ? null : new GCControllerDirectionPad(rightThumbstick, ERetainPolicy.Unretained);
                }
                
                return _rightThumbstick;
            }
        }

        
        /// <value>LeftTrigger</value>
        private GCControllerButtonInput _leftTrigger;
        public GCControllerButtonInput LeftTrigger
        {
            get
            {
                var leftTrigger = GCExtendedGamepad_GetPropLeftTrigger(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_leftTrigger == null || leftTrigger != (IntPtr)_leftTrigger.Handle)
                {
                    _leftTrigger = leftTrigger == IntPtr.Zero ? null : new GCControllerButtonInput(leftTrigger, ERetainPolicy.Unretained);
                }
                
                return _leftTrigger;
            }
        }

        
        /// <value>RightTrigger</value>
        private GCControllerButtonInput _rightTrigger;
        public GCControllerButtonInput RightTrigger
        {
            get
            {
                var rightTrigger = GCExtendedGamepad_GetPropRightTrigger(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_rightTrigger == null || rightTrigger != (IntPtr)_rightTrigger.Handle)
                {
                    _rightTrigger = rightTrigger == IntPtr.Zero ? null : new GCControllerButtonInput(rightTrigger, ERetainPolicy.Unretained);
                }
                
                return _rightTrigger;
            }
        }

        
        /// <value>ButtonHome</value>
        private GCControllerButtonInput _buttonHome;
        public GCControllerButtonInput ButtonHome
        {
            get
            {
                var buttonHome = GCExtendedGamepad_GetPropButtonHome(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_buttonHome == null || buttonHome != (IntPtr)_buttonHome.Handle)
                {
                    _buttonHome = buttonHome == IntPtr.Zero ? null : new GCControllerButtonInput(buttonHome, ERetainPolicy.Unretained);
                }
                
                return _buttonHome;
            }
        }

        
        /// <value>ButtonMenu</value>
        private GCControllerButtonInput _buttonMenu;
        public GCControllerButtonInput ButtonMenu
        {
            get
            {
                var buttonMenu = GCExtendedGamepad_GetPropButtonMenu(Handle, out var exceptionPtr);
                
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

        
        /// <value>ButtonOptions</value>
        private GCControllerButtonInput _buttonOptions;
        public GCControllerButtonInput ButtonOptions
        {
            get
            {
                var buttonOptions = GCExtendedGamepad_GetPropButtonOptions(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_buttonOptions == null || buttonOptions != (IntPtr)_buttonOptions.Handle)
                {
                    _buttonOptions = buttonOptions == IntPtr.Zero ? null : new GCControllerButtonInput(buttonOptions, ERetainPolicy.Unretained);
                }
                
                return _buttonOptions;
            }
        }

        
        /// <value>LeftThumbstickButton</value>
        private GCControllerButtonInput _leftThumbstickButton;
        public GCControllerButtonInput LeftThumbstickButton
        {
            get
            {
                var leftThumbstickButton = GCExtendedGamepad_GetPropLeftThumbstickButton(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_leftThumbstickButton == null || leftThumbstickButton != (IntPtr)_leftThumbstickButton.Handle)
                {
                    _leftThumbstickButton = leftThumbstickButton == IntPtr.Zero ? null : new GCControllerButtonInput(leftThumbstickButton, ERetainPolicy.Unretained);
                }
                
                return _leftThumbstickButton;
            }
        }

        
        /// <value>RightThumbstickButton</value>
        private GCControllerButtonInput _rightThumbstickButton;
        public GCControllerButtonInput RightThumbstickButton
        {
            get
            {
                var rightThumbstickButton = GCExtendedGamepad_GetPropRightThumbstickButton(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_rightThumbstickButton == null || rightThumbstickButton != (IntPtr)_rightThumbstickButton.Handle)
                {
                    _rightThumbstickButton = rightThumbstickButton == IntPtr.Zero ? null : new GCControllerButtonInput(rightThumbstickButton, ERetainPolicy.Unretained);
                }
                
                return _rightThumbstickButton;
            }
        }

        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void GCExtendedGamepad_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        protected override void Dispose(bool disposing)
        {
            if (_retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("GCExtendedGamepad Dispose");
                GCExtendedGamepad_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~GCExtendedGamepad()
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
