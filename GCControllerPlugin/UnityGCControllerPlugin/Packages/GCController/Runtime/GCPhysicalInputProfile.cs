//
//  GCPhysicalInputProfile.cs
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
    public class GCPhysicalInputProfile : UnmanagedObject, IDisposable
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
        private static extern void GCPhysicalInputProfile_GetPropElements(
            HandleRef ptr,
            ref IntPtr keysPtr,
            ref IntPtr valuesPtr,
            ref long count,
            ref IntPtr exceptionPtr);
        
        [DllImport(dll)]
        private static extern void GCPhysicalInputProfile_GetPropAllButtons(
            HandleRef ptr,
            ref IntPtr bufferPtr,
            ref long count);
        
        [DllImport(dll)]
        private static extern void GCPhysicalInputProfile_GetPropAllDpads(
            HandleRef ptr, 
            ref IntPtr buffer, 
            ref long count);
        
        [DllImport(dll)]
        private static extern void GCPhysicalInputProfile_GetPropAllAxes(
            HandleRef ptr,
            ref IntPtr buffer,
            ref long count);
        
        [DllImport(dll)]
        private static extern void GCPhysicalInputProfile_GetPropAllElements(
            HandleRef ptr,
            ref IntPtr buffer,
            ref long count);

        [DllImport(dll)]
        private static extern IntPtr GCPhysicalInputProfile_ObjectForKeyedSubscript(
            HandleRef ptr,
            string key,
            out IntPtr exceptionPtr
            );

        #endregion

        internal GCPhysicalInputProfile(IntPtr ptr) : base(ptr) {}
        internal GCPhysicalInputProfile(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        
        


        
        
        
        /// <value>Elements</value>
        public Tuple<string,GCControllerElement>[] Elements
        {
            get 
            { 
                var keysPtr = IntPtr.Zero;
                var valuesPtr = IntPtr.Zero;
                var exceptionPtr = IntPtr.Zero;
                long count = 0;

                GCPhysicalInputProfile_GetPropElements(Handle, ref keysPtr, ref valuesPtr, ref count, ref exceptionPtr);

                var keyValuePairs = new Tuple<string, GCControllerElement>[count];
                for (int i = 0; i < count; i++)
                {
                    //TODO - templates should distinguish between string keys, and pointer keys
                    var keyPtr = Marshal.ReadIntPtr(keysPtr + (i * IntPtr.Size));
                    var valuePtr = Marshal.ReadIntPtr(valuesPtr + (i * IntPtr.Size));

                    keyValuePairs[i] = new Tuple<string, GCControllerElement>(
                        Marshal.PtrToStringAuto(keyPtr),
                        new GCControllerElement(valuePtr)
                    );
                }

                Marshal.FreeHGlobal(keysPtr);
                Marshal.FreeHGlobal(valuesPtr);

                return keyValuePairs;
            }
        }
        
        /// <value>AllButtons</value>
        public GCControllerButtonInput[] AllButtons
        {
            get 
            { 
                IntPtr bufferPtr = IntPtr.Zero;
                long bufferLen = 0;

                GCPhysicalInputProfile_GetPropAllButtons(Handle, ref bufferPtr, ref bufferLen);

                var allButtons = new GCControllerButtonInput[bufferLen];

                for (int i = 0; i < bufferLen; i++)
                {
                    IntPtr ptr2 = Marshal.ReadIntPtr(bufferPtr + (i * IntPtr.Size));
                    allButtons[i] = ptr2 == IntPtr.Zero ? null : new GCControllerButtonInput(ptr2);
                }

                Marshal.FreeHGlobal(bufferPtr);

                return allButtons;
            }
        }
        
        
        /// <value>AllDpads</value>
        public GCControllerDirectionPad[] AllDpads
        {
            get 
            { 
                IntPtr bufferPtr = IntPtr.Zero;
                long bufferLen = 0;

                GCPhysicalInputProfile_GetPropAllDpads(Handle, ref bufferPtr, ref bufferLen);

                var allDpads = new GCControllerDirectionPad[bufferLen];

                for (int i = 0; i < bufferLen; i++)
                {
                    IntPtr ptr2 = Marshal.ReadIntPtr(bufferPtr + (i * IntPtr.Size));
                    allDpads[i] = ptr2 == IntPtr.Zero ? null : new GCControllerDirectionPad(ptr2);
                }

                Marshal.FreeHGlobal(bufferPtr);

                return allDpads;
            }
        }
        
        
        /// <value>AllAxes</value>
        public GCControllerAxisInput[] AllAxes
        {
            get 
            { 
                IntPtr bufferPtr = IntPtr.Zero;
                long bufferLen = 0;

                GCPhysicalInputProfile_GetPropAllAxes(Handle, ref bufferPtr, ref bufferLen);

                var allAxes = new GCControllerAxisInput[bufferLen];

                for (int i = 0; i < bufferLen; i++)
                {
                    IntPtr ptr2 = Marshal.ReadIntPtr(bufferPtr + (i * IntPtr.Size));
                    allAxes[i] = ptr2 == IntPtr.Zero ? null : new GCControllerAxisInput(ptr2);
                }

                Marshal.FreeHGlobal(bufferPtr);

                return allAxes;
            }
        }


        /// <value>AllElements</value>
        public GCControllerElement[] AllElements
        {
            get
            {
                IntPtr bufferPtr = IntPtr.Zero;
                long bufferLen = 0;

                GCPhysicalInputProfile_GetPropAllElements(Handle, ref bufferPtr, ref bufferLen);

                var allElements = new GCControllerElement[bufferLen];

                for (int i = 0; i < bufferLen; i++)
                {
                    IntPtr ptr2 = Marshal.ReadIntPtr(bufferPtr + (i * IntPtr.Size));
                    allElements[i] = ptr2 == IntPtr.Zero ? null : new GCControllerElement(ptr2);
                }

                Marshal.FreeHGlobal(bufferPtr);

                return allElements;
            }
        }

        public GCControllerElement this[string key] 
        {
            get
            {
                var ptr = GCPhysicalInputProfile_ObjectForKeyedSubscript(Handle, key, out var exceptionPtr);
                
                if (exceptionPtr != IntPtr.Zero)
                {
                    var nsException = new NSException(exceptionPtr);
                    throw new GameControllerException(nsException,nsException.Reason);
                }

                return ptr == IntPtr.Zero ? null : new GCControllerElement(ptr);
            }
        }

        #region IDisposable Support
        [DllImport(dll)]
        private static extern void GCPhysicalInputProfile_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        protected virtual void Dispose(bool disposing)
        {
            if (_retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("GCPhysicalInputProfile Dispose");
                GCPhysicalInputProfile_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~GCPhysicalInputProfile()
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
