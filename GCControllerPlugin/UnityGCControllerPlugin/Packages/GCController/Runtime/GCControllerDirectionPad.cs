//
//  GCControllerDirectionPad.cs
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
    public class GCControllerDirectionPad : GCControllerElement, IDisposable
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
        private static extern IntPtr GCControllerDirectionPad_GetPropUp(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCControllerDirectionPad_GetPropDown(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCControllerDirectionPad_GetPropLeft(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCControllerDirectionPad_GetPropRight(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCControllerDirectionPad_GetPropXAxis(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCControllerDirectionPad_GetPropYAxis(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        

        #endregion

        internal GCControllerDirectionPad(IntPtr ptr) : base(ptr) {}
        internal GCControllerDirectionPad(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        
        


        
        
        
        /// <value>Up</value>
        private GCControllerButtonInput _up;
        public GCControllerButtonInput Up
        {
            get
            {
                var up = GCControllerDirectionPad_GetPropUp(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_up == null || up != (IntPtr)_up.Handle)
                {
                    _up = up == IntPtr.Zero ? null : new GCControllerButtonInput(up, ERetainPolicy.Unretained);
                }
                
                return _up;
            }
        }

        
        /// <value>Down</value>
        private GCControllerButtonInput _down;
        public GCControllerButtonInput Down
        {
            get
            {
                var down = GCControllerDirectionPad_GetPropDown(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_down == null || down != (IntPtr)_down.Handle)
                {
                    _down = down == IntPtr.Zero ? null : new GCControllerButtonInput(down, ERetainPolicy.Unretained);
                }
                
                return _down;
            }
        }

        
        /// <value>Left</value>
        private GCControllerButtonInput _left;
        public GCControllerButtonInput Left
        {
            get
            {
                var left = GCControllerDirectionPad_GetPropLeft(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_left == null || left != (IntPtr)_left.Handle)
                {
                    _left = left == IntPtr.Zero ? null : new GCControllerButtonInput(left, ERetainPolicy.Unretained);
                }
                
                return _left;
            }
        }

        
        /// <value>Right</value>
        private GCControllerButtonInput _right;
        public GCControllerButtonInput Right
        {
            get
            {
                var right = GCControllerDirectionPad_GetPropRight(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_right == null || right != (IntPtr)_right.Handle)
                {
                    _right = right == IntPtr.Zero ? null : new GCControllerButtonInput(right, ERetainPolicy.Unretained);
                }
                
                return _right;
            }
        }

        
        /// <value>XAxis</value>
        private GCControllerAxisInput _xAxis;
        public GCControllerAxisInput XAxis
        {
            get
            {
                var xAxis = GCControllerDirectionPad_GetPropXAxis(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_xAxis == null || xAxis != (IntPtr)_xAxis.Handle)
                {
                    _xAxis = xAxis == IntPtr.Zero ? null : new GCControllerAxisInput(xAxis, ERetainPolicy.Unretained);
                }
                
                return _xAxis;
            }
        }

        
        /// <value>YAxis</value>
        private GCControllerAxisInput _yAxis;
        public GCControllerAxisInput YAxis
        {
            get
            {
                var yAxis = GCControllerDirectionPad_GetPropYAxis(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_yAxis == null || yAxis != (IntPtr)_yAxis.Handle)
                {
                    _yAxis = yAxis == IntPtr.Zero ? null : new GCControllerAxisInput(yAxis, ERetainPolicy.Unretained);
                }
                
                return _yAxis;
            }
        }

        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void GCControllerDirectionPad_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        protected override void Dispose(bool disposing)
        {
            if (_retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("GCControllerDirectionPad Dispose");
                GCControllerDirectionPad_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~GCControllerDirectionPad()
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
