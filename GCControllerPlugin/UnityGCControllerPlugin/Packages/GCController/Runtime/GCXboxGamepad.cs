//
//  GCXboxGamepad.cs
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
    public class GCXboxGamepad : GCExtendedGamepad, IDisposable
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
        private static extern IntPtr GCXboxGamepad_GetPropPaddleButton1(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCXboxGamepad_GetPropPaddleButton2(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCXboxGamepad_GetPropPaddleButton3(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCXboxGamepad_GetPropPaddleButton4(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        

        #endregion

        internal GCXboxGamepad(IntPtr ptr) : base(ptr) {}
        internal GCXboxGamepad(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        
        


        
        
        
        /// <value>PaddleButton1</value>
        private GCControllerButtonInput _paddleButton1;
        public GCControllerButtonInput PaddleButton1
        {
            get
            {
                var paddleButton1 = GCXboxGamepad_GetPropPaddleButton1(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_paddleButton1 == null || paddleButton1 != (IntPtr)_paddleButton1.Handle)
                {
                    _paddleButton1 = paddleButton1 == IntPtr.Zero ? null : new GCControllerButtonInput(paddleButton1, ERetainPolicy.Unretained);
                }
                
                return _paddleButton1;
            }
        }

        
        /// <value>PaddleButton2</value>
        private GCControllerButtonInput _paddleButton2;
        public GCControllerButtonInput PaddleButton2
        {
            get
            {
                var paddleButton2 = GCXboxGamepad_GetPropPaddleButton2(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_paddleButton2 == null || paddleButton2 != (IntPtr)_paddleButton2.Handle)
                {
                    _paddleButton2 = paddleButton2 == IntPtr.Zero ? null : new GCControllerButtonInput(paddleButton2, ERetainPolicy.Unretained);
                }
                
                return _paddleButton2;
            }
        }

        
        /// <value>PaddleButton3</value>
        private GCControllerButtonInput _paddleButton3;
        public GCControllerButtonInput PaddleButton3
        {
            get
            {
                var paddleButton3 = GCXboxGamepad_GetPropPaddleButton3(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_paddleButton3 == null || paddleButton3 != (IntPtr)_paddleButton3.Handle)
                {
                    _paddleButton3 = paddleButton3 == IntPtr.Zero ? null : new GCControllerButtonInput(paddleButton3, ERetainPolicy.Unretained);
                }
                
                return _paddleButton3;
            }
        }

        
        /// <value>PaddleButton4</value>
        private GCControllerButtonInput _paddleButton4;
        public GCControllerButtonInput PaddleButton4
        {
            get
            {
                var paddleButton4 = GCXboxGamepad_GetPropPaddleButton4(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_paddleButton4 == null || paddleButton4 != (IntPtr)_paddleButton4.Handle)
                {
                    _paddleButton4 = paddleButton4 == IntPtr.Zero ? null : new GCControllerButtonInput(paddleButton4, ERetainPolicy.Unretained);
                }
                
                return _paddleButton4;
            }
        }

        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void GCXboxGamepad_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        protected override void Dispose(bool disposing)
        {
            if (GetType() == typeof(GCXboxGamepad) && _retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("GCXboxGamepad Dispose");
                GCXboxGamepad_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~GCXboxGamepad()
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
