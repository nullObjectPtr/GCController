//
//  GCControllerTouchpad.cs
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
    public class GCControllerTouchpad : GCControllerElement, IDisposable
    {
        #region dll
        
        #if !UNITY_EDITOR && UNITY_IPHONE || UNITY_TVOS
        const string dll = "__Internal";
        #else
        const string dll = "HHGCControllerMacOS";
        #endif

        // Class Methods
        

        

        

        

        // Properties
        

        #endregion

        internal GCControllerTouchpad(IntPtr ptr) : base(ptr) {}
        internal GCControllerTouchpad(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        
        


        
        
        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void GCControllerTouchpad_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        private void Dispose(bool disposing)
        {
            if (GetType() == typeof(GCControllerTouchpad) && _retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                Debug.Log("GCControllerTouchpad Dispose");
                GCControllerTouchpad_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~GCControllerTouchpad()
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
