//
//  GCController.cs
//
//  Created by Jonathan Culp <jonathanculp@gmail.com> on
//  Copyright © 2021 HovelHouseApps. All rights reserved.
//  Unauthorized copying of this file, via any medium is strictly prohibited
//  Proprietary and confidential
//

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace HovelHouse.GameController
{
    /// <summary>
    /// 
    /// </summary>
    public class GCController : UnmanagedObject, IDisposable
    {
        #region dll
        
        #if !UNITY_EDITOR && UNITY_IPHONE || UNITY_TVOS
        const string dll = "__Internal";
        #else
        const string dll = "HHGCControllerMacOS";
        #endif

        // Class Methods
        
        
        [DllImport(dll)]
        private static extern void GCController_controllers(
            out IntPtr bufferPtr,
            out long bufferLength,
            out IntPtr exceptionPtr
            );
        

        
        
        [DllImport(dll)]
        private static extern IntPtr GCController_controllerWithExtendedGamepad(
            out IntPtr exceptionPtr);

        
        
        [DllImport(dll)]
        private static extern IntPtr GCController_controllerWithMicroGamepad(
            out IntPtr exceptionPtr);

        

        

        
        [DllImport(dll)]
        private static extern IntPtr GCController_capture(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        

        

        // Properties
        
        [DllImport(dll)]
        private static extern IntPtr GCController_GetPropPhysicalInputProfile(
            HandleRef ptr, 
            out long typeToken,
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern bool GCController_GetPropAttachedToDevice(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern bool GCController_GetPropSnapshot(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCController_GetPropExtendedGamepad(
            HandleRef ptr, 
            out long typeToken,
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCController_GetPropMicroGamepad(
            HandleRef ptr, 
            out long typeToken,
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCController_GetPropProductCategory(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCController_GetPropVendorName(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCController_GetPropCurrent(
            out IntPtr exceptionPtr);

        

        #endregion

        internal GCController(IntPtr ptr) : base(ptr) {}
        internal GCController(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        /// <summary>
        /// </summary>
        /// 
        /// <returns>val</returns>
        public static GCController[] Controllers()
        { 
            
            GCController_controllers(
                out var bufferPtr,
                out var bufferLen,
                out var exceptionPtr);
                
            var val = new GCController[bufferLen];

            for (var i = 0; i < bufferLen; i++)
            {
                var ptr2 = Marshal.ReadIntPtr(bufferPtr + (i * IntPtr.Size));
                val[i] = new GCController(ptr2);
            }
                        
            Marshal.FreeHGlobal(bufferPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return val;
        }
        

        /// <summary>
        /// </summary>
        /// 
        /// <returns>val</returns>
        public static GCController ControllerWithExtendedGamepad()
        { 
            var val = GCController_controllerWithExtendedGamepad(
                out var exceptionPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return val == IntPtr.Zero ? null : new GCController(val);
        }
        

        /// <summary>
        /// </summary>
        /// 
        /// <returns>val</returns>
        public static GCController ControllerWithMicroGamepad()
        { 
            var val = GCController_controllerWithMicroGamepad(
                out var exceptionPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return val == IntPtr.Zero ? null : new GCController(val);
        }
        

        
        
        


        
        /// <summary>
        /// </summary>
        /// 
        /// <returns>val</returns>
        public GCController Capture()
        { 
            var val = GCController_capture(
                Handle,
                out var exceptionPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return val == IntPtr.Zero ? null : new GCController(val);
        }
        

        
        
        
        /// <value>PhysicalInputProfile</value>
        private GCPhysicalInputProfile _physicalInputProfile;
        public GCPhysicalInputProfile PhysicalInputProfile
        {
            get
            {
                var profilePtr = GCController_GetPropPhysicalInputProfile(Handle, out var typeToken, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }

                if (profilePtr == IntPtr.Zero)
                    return null;

                if (_physicalInputProfile != null && profilePtr == (IntPtr) _physicalInputProfile.Handle)
                    return _physicalInputProfile;

                switch (typeToken)
                {
                    case 1:
                        _physicalInputProfile = new GCDualSenseGamepad(profilePtr);
                        break;
                    case 2:
                        _physicalInputProfile = new GCDualShockGamepad(profilePtr);
                        break;
                    case 3:
                        _physicalInputProfile = new GCXboxGamepad(profilePtr);
                        break;
                    case 4:
                        _physicalInputProfile = new GCExtendedGamepad(profilePtr);
                        break;
                    case 5:
                        _physicalInputProfile = new GCDirectionalGamepad(profilePtr);
                        break;
                    case 6:
                        _physicalInputProfile = new GCMicroGamepad(profilePtr);
                        break;
                    case 7:
                        _physicalInputProfile = new GCPhysicalInputProfile(profilePtr);
                        break;
                    case 8:
                        _physicalInputProfile = new GCKeyboardInput(profilePtr);
                        break;
                    case 9:
                        _physicalInputProfile = new GCMouseInput(profilePtr);
                        break;
                    case 10:
                        throw new NotSupportedException("the GCMotion profile is not supported");
                }
                
                return _physicalInputProfile;
            }
        }

        
        /// <value>AttachedToDevice</value>
        public bool AttachedToDevice
        {
            get
            {
                bool attachedToDevice = GCController_GetPropAttachedToDevice(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return attachedToDevice;
            }
        }

        
        /// <value>Snapshot</value>
        public bool Snapshot
        {
            get
            {
                bool snapshot = GCController_GetPropSnapshot(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return snapshot;
            }
        }

        
        /// <value>ExtendedGamepad</value>
        private GCExtendedGamepad _extendedGamepad;
        public GCExtendedGamepad ExtendedGamepad
        {
            get
            {
                var extendedGamepad = GCController_GetPropExtendedGamepad(
                    Handle, out var typeToken, out var exceptionPtr);

                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }

                // Pointer value has not changed, send cached version
                if (extendedGamepad == (_extendedGamepad == null ? IntPtr.Zero : (IntPtr) _extendedGamepad.Handle))
                    return _extendedGamepad;

                if (extendedGamepad == IntPtr.Zero)
                {
                    _extendedGamepad = null;
                }
                else
                {
                    switch (typeToken)
                    {
                        case 1:
                            _extendedGamepad = new GCDualSenseGamepad(extendedGamepad);
                            break;
                        case 2:
                            _extendedGamepad = new GCDualShockGamepad(extendedGamepad);
                            break;
                        case 3:
                            _extendedGamepad = new GCXboxGamepad(extendedGamepad);
                            break;
                        case 4:
                            _extendedGamepad = new GCExtendedGamepad(extendedGamepad);
                            break;
                        default:
                            throw new InvalidProgramException("unsupported extended gamepad type");
                    }
                }

                return _extendedGamepad;
            }
        }

        
        /// <value>MicroGamepad</value>
        private GCMicroGamepad _microGamepad;
        public GCMicroGamepad MicroGamepad
        {
            get
            {
                var microGamepad = GCController_GetPropMicroGamepad(Handle, out var typeToken, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                // pointer value has not changed, send cached version
                // Pointer value has not changed, send cached version
                if (microGamepad == (_microGamepad == null ? IntPtr.Zero : (IntPtr) _microGamepad.Handle))
                    return _microGamepad;
                
                if (microGamepad == IntPtr.Zero)
                {
                    _microGamepad = null;
                }
                else
                {
                    switch (typeToken)
                    {
                        case 5:
                            _microGamepad = new GCDirectionalGamepad(microGamepad);
                            break;
                        case 6:
                            _microGamepad = new GCMicroGamepad(microGamepad);
                            break;
                        default:
                            throw new InvalidProgramException("unsupported extended gamepad type");
                    }
                }

                return _microGamepad;
            }
        }

        
        /// <value>ProductCategory</value>
        public string ProductCategory
        {
            get
            {
                IntPtr productCategory = GCController_GetPropProductCategory(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return Marshal.PtrToStringAuto(productCategory);
            }
        }

        
        /// <value>VendorName</value>
        public string VendorName
        {
            get
            {
                IntPtr vendorName = GCController_GetPropVendorName(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return Marshal.PtrToStringAuto(vendorName);
            }
        }

        
        /// <value>Current</value>
        private static GCController _current;
        public static GCController Current
        {
            get
            {
                var current = GCController_GetPropCurrent(out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_current == null || current != (IntPtr)_current.Handle)
                {
                    _current = current == IntPtr.Zero ? null : new GCController(current, ERetainPolicy.Unretained);
                }
                
                return _current;
            }
        }

        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void GCController_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        protected virtual void Dispose(bool disposing)
        {
            if (_retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("GCController Dispose");
                GCController_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~GCController()
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
