//
//  GCControllerButtonInput.cs
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
    public class GCControllerButtonInput : GCControllerElement, IDisposable
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
        private static extern bool GCControllerButtonInput_GetPropPressed(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern float GCControllerButtonInput_GetPropValue(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern bool GCControllerButtonInput_GetPropTouched(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

                [DllImport(dll)]
        private static extern void GCControllerButtonInput_SetPropValueChangedHandler(HandleRef ptr, GCControllerButtonValueChangedDelegate valueChangedHandler, out IntPtr exceptionPtr);

                [DllImport(dll)]
        private static extern void GCControllerButtonInput_SetPropPressedChangedHandler(HandleRef ptr, GCControllerButtonValueChangedDelegate pressedChangedHandler, out IntPtr exceptionPtr);

        

        #endregion

        internal GCControllerButtonInput(IntPtr ptr) : base(ptr) {}
        internal GCControllerButtonInput(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        
        


        
        
        
        /// <value>Pressed</value>
        public bool Pressed
        {
            get
            {
                bool pressed = GCControllerButtonInput_GetPropPressed(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return pressed;
            }
        }

        
        /// <value>Value</value>
        public float Value
        {
            get
            {
                float value = GCControllerButtonInput_GetPropValue(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return value;
            }
        }

        
        /// <value>Touched</value>
        public bool Touched
        {
            get
            {
                bool touched = GCControllerButtonInput_GetPropTouched(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return touched;
            }
        }

        
        /// <value>ValueChangedHandler</value>
        public Action<GCControllerButtonInput,float,bool> ValueChangedHandler
        {
            get 
            {
                if(ValueChangedHandlerCallbacks.TryGetValue(
                    HandleRef.ToIntPtr(Handle), 
                    out ExecutionContext<GCControllerButtonInput,float,bool> value))
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
                    ValueChangedHandlerCallbacks[myPtr] = new ExecutionContext<GCControllerButtonInput,float,bool>(value);
                }
                GCControllerButtonInput_SetPropValueChangedHandler(Handle, ValueChangedHandlerCallback, out IntPtr exceptionPtr);

                if(exceptionPtr != IntPtr.Zero)
                {
                    var nativeException = new NSException(exceptionPtr);
                    throw new GameControllerException(nativeException, nativeException.Reason);
                }
            }
        }

        private static readonly Dictionary<IntPtr,ExecutionContext<GCControllerButtonInput,float,bool>> ValueChangedHandlerCallbacks = new Dictionary<IntPtr,ExecutionContext<GCControllerButtonInput,float,bool>>();

        [MonoPInvokeCallback(typeof(GCControllerButtonValueChangedDelegate))]
        private static void ValueChangedHandlerCallback(IntPtr thisPtr, IntPtr button, float value, bool pressed)
        {
            if(ValueChangedHandlerCallbacks.TryGetValue(thisPtr, out ExecutionContext<GCControllerButtonInput,float,bool> callback))
            {
                callback.Invoke(
                        button == IntPtr.Zero ? null : new GCControllerButtonInput(button),
                        value,
                        pressed);
            }
        }

        
        /// <value>PressedChangedHandler</value>
        public Action<GCControllerButtonInput,float,bool> PressedChangedHandler
        {
            get 
            {
                if(PressedChangedHandlerCallbacks.TryGetValue(
                    HandleRef.ToIntPtr(Handle), 
                    out ExecutionContext<GCControllerButtonInput,float,bool> value))
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
                    PressedChangedHandlerCallbacks.Remove(myPtr);
                }
                else
                {
                    PressedChangedHandlerCallbacks[myPtr] = new ExecutionContext<GCControllerButtonInput,float,bool>(value);
                }
                GCControllerButtonInput_SetPropPressedChangedHandler(Handle, PressedChangedHandlerCallback, out IntPtr exceptionPtr);

                if(exceptionPtr != IntPtr.Zero)
                {
                    var nativeException = new NSException(exceptionPtr);
                    throw new GameControllerException(nativeException, nativeException.Reason);
                }
            }
        }

        private static readonly Dictionary<IntPtr,ExecutionContext<GCControllerButtonInput,float,bool>> PressedChangedHandlerCallbacks = new Dictionary<IntPtr,ExecutionContext<GCControllerButtonInput,float,bool>>();

        [MonoPInvokeCallback(typeof(GCControllerButtonValueChangedDelegate))]
        private static void PressedChangedHandlerCallback(IntPtr thisPtr, IntPtr button, float value, bool pressed)
        {
            if(PressedChangedHandlerCallbacks.TryGetValue(thisPtr, out ExecutionContext<GCControllerButtonInput,float,bool> callback))
            {
                callback.Invoke(
                        button == IntPtr.Zero ? null : new GCControllerButtonInput(button),
                        value,
                        pressed);
            }
        }

        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void GCControllerButtonInput_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        private void Dispose(bool disposing)
        {
            if (GetType() == typeof(GCControllerButtonInput) && _retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("GCControllerButtonInput Dispose");
                GCControllerButtonInput_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~GCControllerButtonInput()
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
