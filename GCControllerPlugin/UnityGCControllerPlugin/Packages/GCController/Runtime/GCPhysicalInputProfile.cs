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
        

        

        
        
        [DllImport(dll)]
        private static extern IntPtr GCPhysicalInputProfile_mappedElementAliasForPhysicalInputName(
            HandleRef ptr, 
            string inputName,
            out IntPtr exceptionPtr);

        

        

        // Properties
        
        [DllImport(dll)]
        private static extern void GCPhysicalInputProfile_GetPropElements(
            HandleRef ptr,
            ref IntPtr keysPtr,
            ref IntPtr valuesPtr,
            ref IntPtr typeTokenPtr,
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
            ref IntPtr tokenTypes,
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
        
        


        
        /// <summary>
        /// </summary>
        /// <param name="inputName"></param>
        /// <returns>val</returns>
        public string MappedElementAliasForPhysicalInputName(
            string inputName)
        { 
            
            var val = GCPhysicalInputProfile_mappedElementAliasForPhysicalInputName(
                Handle,
                inputName,
                out var exceptionPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return Marshal.PtrToStringAuto(val);
        }
        

        
        
        
        /// <value>Elements</value>
        public Tuple<string,GCControllerElement>[] Elements
        {
            get 
            { 
                var keysPtr = IntPtr.Zero;
                var valuesPtr = IntPtr.Zero;
                var typeTokenPtr = IntPtr.Zero;
                var exceptionPtr = IntPtr.Zero;
                long count = 0;

                GCPhysicalInputProfile_GetPropElements(Handle, ref keysPtr, ref valuesPtr, ref typeTokenPtr, ref count, ref exceptionPtr);

                byte[] typeTokens = new byte[count];
                if(typeTokenPtr != IntPtr.Zero)
                    Marshal.Copy(typeTokenPtr, typeTokens, 0, (int)count);

                var keyValuePairs = new Tuple<string, GCControllerElement>[count];
                for (int i = 0; i < count; i++)
                {
                    //TODO - templates should distinguish between string keys, and pointer keys
                    var keyPtr = Marshal.ReadIntPtr(keysPtr + (i * IntPtr.Size));
                    var valuePtr = Marshal.ReadIntPtr(valuesPtr + (i * IntPtr.Size));
                    
                    keyValuePairs[i] = new Tuple<string, GCControllerElement>(
                        Marshal.PtrToStringAuto(keyPtr),
                        MakeElement(valuePtr, typeTokens[i])
                    );
                }

                Marshal.FreeHGlobal(keysPtr);
                Marshal.FreeHGlobal(valuesPtr);
                Marshal.FreeHGlobal(typeTokenPtr);

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
                IntPtr typeTokenPtr = IntPtr.Zero;
                
                long bufferLen = 0;

                GCPhysicalInputProfile_GetPropAllElements(Handle, ref bufferPtr, ref typeTokenPtr, ref bufferLen);

                var allElements = new GCControllerElement[bufferLen];
                
                byte[] typeTokens = new byte[bufferLen];
                Marshal.Copy(typeTokenPtr, typeTokens, 0, (int)bufferLen);

                for (var i = 0; i < bufferLen; i++)
                {
                    IntPtr ptr2 = Marshal.ReadIntPtr(bufferPtr + (i * IntPtr.Size));
                    allElements[i] = ptr2 == IntPtr.Zero ? null : MakeElement(ptr2, typeTokens[i]);
                }

                Marshal.FreeHGlobal(bufferPtr);

                return allElements;
            }
        }

        
        /// <value>HasRemappedElements</value>
        public bool HasRemappedElements
        {
            get
            {
                bool hasRemappedElements = false; //GCPhysicalInputProfile_GetPropHasRemappedElements(Handle, out var exceptionPtr);
                /*
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                */
                
                return hasRemappedElements;
            }
        }


        private GCControllerElement MakeElement(IntPtr valuePtr, Byte typeToken)
        {
            if (valuePtr == IntPtr.Zero)
                return null;
            
            GCControllerElement element = null;
            switch (typeToken)
            {
                case 1:
                    // TODO
                    //return new GCDualSenseAdaptiveTrigger(valuePtr);
                    return new GCControllerButtonInput(valuePtr);
                case 2:
                    return new GCDeviceCursor(valuePtr);
                case 3:
                    return new GCControllerDirectionPad(valuePtr);
                case 4:
                    return new GCControllerTouchpad(valuePtr);
                case 5:
                    return new GCControllerButtonInput(valuePtr);
                case 6:
                    return new GCControllerAxisInput(valuePtr);
                default:
                    return new GCControllerElement(valuePtr);
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
