//
//  GCControllerAxisInput.cs
//
//  Created by Jonathan Culp <jonathanculp@gmail.com> on
//  Copyright © 2021 HovelHouseApps. All rights reserved.
//  Unauthorized copying of this file, via any medium is strictly prohibited
//  Proprietary and confidential
//

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace HovelHouse.GameController
{
    /// <summary>
    /// 
    /// </summary>
    public class GCControllerAxisInput : GCControllerElement, IDisposable
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
        private static extern float GCControllerAxisInput_GetPropValue(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

                [DllImport(dll)]
        private static extern void GCControllerAxisInput_SetPropValueChangedHandler(HandleRef ptr, GCControllerAxisValueChangedDelegate valueChangedHandler, out IntPtr exceptionPtr);

        

        #endregion

        internal GCControllerAxisInput(IntPtr ptr) : base(ptr) {}
        internal GCControllerAxisInput(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        
        


        
        
        
        /// <value>Value</value>
        public float Value
        {
            get
            {
                float value = GCControllerAxisInput_GetPropValue(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return value;
            }
        }

        
        /// <value>ValueChangedHandler</value>
        public Action<GCControllerAxisInput,float> ValueChangedHandler
        {
            get 
            {
                if(ValueChangedHandlerCallbacks.TryGetValue(
                    HandleRef.ToIntPtr(Handle), 
                    out ExecutionContext<GCControllerAxisInput,float> value))
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
                    ValueChangedHandlerCallbacks[myPtr] = new ExecutionContext<GCControllerAxisInput,float>(value);
                }
                GCControllerAxisInput_SetPropValueChangedHandler(Handle, ValueChangedHandlerCallback, out IntPtr exceptionPtr);

                if(exceptionPtr != IntPtr.Zero)
                {
                    var nativeException = new NSException(exceptionPtr);
                    throw new GameControllerException(nativeException, nativeException.Reason);
                }
            }
        }

        private static readonly Dictionary<IntPtr,ExecutionContext<GCControllerAxisInput,float>> ValueChangedHandlerCallbacks = new Dictionary<IntPtr,ExecutionContext<GCControllerAxisInput,float>>();

        [MonoPInvokeCallback(typeof(GCControllerAxisValueChangedDelegate))]
        private static void ValueChangedHandlerCallback(IntPtr thisPtr, IntPtr axis, float value)
        {
            if(ValueChangedHandlerCallbacks.TryGetValue(thisPtr, out ExecutionContext<GCControllerAxisInput,float> callback))
            {
                callback.Invoke(
                        axis == IntPtr.Zero ? null : new GCControllerAxisInput(axis),
                        value);
            }
        }

        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void GCControllerAxisInput_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        private void Dispose(bool disposing)
        {
            if (GetType() == typeof(GCControllerAxisInput) && _retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("GCControllerAxisInput Dispose");
                GCControllerAxisInput_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~GCControllerAxisInput()
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
