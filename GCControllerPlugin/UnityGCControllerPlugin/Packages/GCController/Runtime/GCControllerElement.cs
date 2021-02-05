//
//  GCControllerElement.cs
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
    public class GCControllerElement : UnmanagedObject, IDisposable
    {
        #region dll
        
        #if !UNITY_EDITOR && UNITY_IPHONE || UNITY_TVOS
        const string dll = "__Internal";
        #else
        const string dll = "HHGCControllerMacOS";
        #endif

        // Class Methods

        [DllImport(dll)]
        private static extern bool GCControllerElement_SupportsSFSymbols();


        // Properties
        
        [DllImport(dll)]
        private static extern IntPtr GCControllerElement_GetPropLocalizedName(
            HandleRef ptr, 
            out IntPtr exceptionPtr);
        
        [DllImport(dll)]
        private static extern void GCControllerElement_SetPropLocalizedName(HandleRef ptr, string localizedName, out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCControllerElement_GetPropSfSymbolsName(
            HandleRef ptr, 
            out IntPtr exceptionPtr);
        
        [DllImport(dll)]
        private static extern void GCControllerElement_SetPropSfSymbolsName(HandleRef ptr, string sfSymbolsName, out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCControllerElement_GetPropUnmappedLocalizedName(
            HandleRef ptr, 
            out IntPtr exceptionPtr);
        
        [DllImport(dll)]
        private static extern void GCControllerElement_SetPropUnmappedLocalizedName(HandleRef ptr, string unmappedLocalizedName, out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCControllerElement_GetPropUnmappedSfSymbolsName(
            HandleRef ptr, 
            out IntPtr exceptionPtr);
        
        [DllImport(dll)]
        private static extern void GCControllerElement_SetPropUnmappedSfSymbolsName(HandleRef ptr, string unmappedSfSymbolsName, out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern bool GCControllerElement_GetPropBoundToSystemGesture(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern bool GCControllerElement_GetPropAnalog(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        
        [DllImport(dll)]
        private static extern IntPtr GCControllerElement_GetPropCollection(
            HandleRef ptr, 
            out IntPtr exceptionPtr);

        

        #endregion

        internal GCControllerElement(IntPtr ptr) : base(ptr) {}
        internal GCControllerElement(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}

        public static bool SupportsSFSymbols()
        {
            return GCControllerElement_SupportsSFSymbols();
        }
        
        /// <value>LocalizedName</value>
        public string LocalizedName
        {
            get
            {
                IntPtr localizedName = GCControllerElement_GetPropLocalizedName(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return Marshal.PtrToStringAuto(localizedName);
            }
            set
            {
                GCControllerElement_SetPropLocalizedName(Handle, value, out IntPtr exceptionPtr);
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
            }
        }

        
        /// <value>SfSymbolsName</value>
        public string SfSymbolsName
        {
            get
            {
                IntPtr sfSymbolsName = GCControllerElement_GetPropSfSymbolsName(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return Marshal.PtrToStringAuto(sfSymbolsName);
            }
            set
            {
                GCControllerElement_SetPropSfSymbolsName(Handle, value, out IntPtr exceptionPtr);
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
            }
        }

        
        /// <value>UnmappedLocalizedName</value>
        public string UnmappedLocalizedName
        {
            get
            {
                IntPtr unmappedLocalizedName = GCControllerElement_GetPropUnmappedLocalizedName(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return Marshal.PtrToStringAuto(unmappedLocalizedName);
            }
            set
            {
                GCControllerElement_SetPropUnmappedLocalizedName(Handle, value, out IntPtr exceptionPtr);
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
            }
        }

        
        /// <value>UnmappedSfSymbolsName</value>
        public string UnmappedSfSymbolsName
        {
            get
            {
                IntPtr unmappedSfSymbolsName = GCControllerElement_GetPropUnmappedSfSymbolsName(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return Marshal.PtrToStringAuto(unmappedSfSymbolsName);
            }
            set
            {
                GCControllerElement_SetPropUnmappedSfSymbolsName(Handle, value, out IntPtr exceptionPtr);
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
            }
        }

        
        /// <value>BoundToSystemGesture</value>
        public bool BoundToSystemGesture
        {
            get
            {
                bool boundToSystemGesture = GCControllerElement_GetPropBoundToSystemGesture(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return boundToSystemGesture;
            }
        }

        
        /// <value>Analog</value>
        public bool Analog
        {
            get
            {
                bool analog = GCControllerElement_GetPropAnalog(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                return analog;
            }
        }

        
        /// <value>Collection</value>
        private GCControllerElement _collection;
        public GCControllerElement Collection
        {
            get
            {
                var collection = GCControllerElement_GetPropCollection(Handle, out var exceptionPtr);
                
                if(exceptionPtr != IntPtr.Zero)
                {
                    var nsexception = new NSException(exceptionPtr);
                    throw new GameControllerException(nsexception, nsexception.Reason);
                }
                
                if(_collection == null || collection != (IntPtr)_collection.Handle)
                {
                    _collection = collection == IntPtr.Zero ? null : new GCControllerElement(collection, ERetainPolicy.Unretained);
                }
                
                return _collection;
            }
        }

        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void GCControllerElement_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        protected virtual void Dispose(bool disposing)
        {
            if (_retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("GCControllerElement Dispose");
                GCControllerElement_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~GCControllerElement()
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
