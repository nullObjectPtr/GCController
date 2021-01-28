//
//  NSException.cs
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
    /// An exception object
    /// </summary>
    /// <remarks>
    /// Apple makes a heavy distinction between errors and exceptions. Exceptions are typically generated when incorrect paramaters are sent to a method. Whenever a native exception is generated, the exception is caught and sent up to the plugin to be re-thrown as a managed CloudKitException. The CloudKitException will contain a reference to the NativeException which can be used to get it&apos;s type, and callstack if needed.
    /// </remarks>
    public class NSException : UnmanagedObject, IDisposable
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
        private static extern IntPtr NSException_GetPropName(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr NSException_GetPropReason(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        // TODO: DLLPROPERTYSTRINGARRAY

        

        #endregion

        internal NSException(IntPtr ptr) : base(ptr) {}
        internal NSException(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        
        


        
        
        
        /// <value>Name</value>
        public string Name
        {
            get
            {
                IntPtr name = NSException_GetPropName(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return Marshal.PtrToStringAuto(name);
            }
        }

        
        /// <value>Reason</value>
        public string Reason
        {
            get
            {
                IntPtr reason = NSException_GetPropReason(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return Marshal.PtrToStringAuto(reason);
            }
        }

        
        // TODO: PROPERTYSTRINGARRAY

        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void NSException_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        protected virtual void Dispose(bool disposing)
        {
            if (_retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("NSException Dispose");
                NSException_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~NSException()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        
    }
}
