//
//  GCDualSenseGamepad.cs
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
    public class GCDualSenseGamepad : GCExtendedGamepad, IDisposable
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
        private static extern IntPtr GCDualSenseGamepad_GetPropTouchpadButton(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCDualSenseGamepad_GetPropTouchpadPrimary(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCDualSenseGamepad_GetPropTouchpadSecondary(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        

        #endregion

        internal GCDualSenseGamepad(IntPtr ptr) : base(ptr) {}
        internal GCDualSenseGamepad(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        
        


        
        
        
        /// <value>TouchpadButton</value>
        private GCControllerButtonInput _touchpadButton;
        public GCControllerButtonInput TouchpadButton
        {
            get
            {
                var touchpadButton = GCDualSenseGamepad_GetPropTouchpadButton(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_touchpadButton == null || touchpadButton != (IntPtr)_touchpadButton.Handle)
                {
                    _touchpadButton = touchpadButton == IntPtr.Zero ? null : new GCControllerButtonInput(touchpadButton, ERetainPolicy.Unretained);
                }
                
                return _touchpadButton;
            }
        }

        
        /// <value>TouchpadPrimary</value>
        private GCControllerDirectionPad _touchpadPrimary;
        public GCControllerDirectionPad TouchpadPrimary
        {
            get
            {
                var touchpadPrimary = GCDualSenseGamepad_GetPropTouchpadPrimary(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_touchpadPrimary == null || touchpadPrimary != (IntPtr)_touchpadPrimary.Handle)
                {
                    _touchpadPrimary = touchpadPrimary == IntPtr.Zero ? null : new GCControllerDirectionPad(touchpadPrimary, ERetainPolicy.Unretained);
                }
                
                return _touchpadPrimary;
            }
        }

        
        /// <value>TouchpadSecondary</value>
        private GCControllerDirectionPad _touchpadSecondary;
        public GCControllerDirectionPad TouchpadSecondary
        {
            get
            {
                var touchpadSecondary = GCDualSenseGamepad_GetPropTouchpadSecondary(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_touchpadSecondary == null || touchpadSecondary != (IntPtr)_touchpadSecondary.Handle)
                {
                    _touchpadSecondary = touchpadSecondary == IntPtr.Zero ? null : new GCControllerDirectionPad(touchpadSecondary, ERetainPolicy.Unretained);
                }
                
                return _touchpadSecondary;
            }
        }

        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void GCDualSenseGamepad_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        protected override void Dispose(bool disposing)
        {
            if (GetType() == typeof(GCDualSenseGamepad) && _retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("GCDualSenseGamepad Dispose");
                GCDualSenseGamepad_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~GCDualSenseGamepad()
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
