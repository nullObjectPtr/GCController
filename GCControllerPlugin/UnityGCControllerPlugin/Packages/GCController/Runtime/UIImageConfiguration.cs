//
//  UIImageConfiguration.cs
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
    public class UIImageConfiguration : UnmanagedObject
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

        internal UIImageConfiguration(IntPtr ptr) : base(ptr) {}
        internal UIImageConfiguration(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        











        #region IDisposable Support
        // #if !UNITY_EDITOR && UNITY_IPHONE || UNITY_TVOS
        // [DllImport("__Internal")]
        // #else
        // [DllImport("HHGCControllerMacOS")]
        // #endif
        // private static extern void UIImageConfiguration_Dispose(HandleRef handle);
        //     
        // private bool disposedValue = false; // To detect redundant calls
        //
        // protected virtual void Dispose(bool disposing)
        // {
        //     if (!disposedValue)
        //     {
        //         //Debug.Log("UIImageConfiguration Dispose");
        //         UIImageConfiguration_Dispose(Handle);
        //         disposedValue = true;
        //     }
        // }

        // ~UIImageConfiguration()
        // {
        //     // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //     Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        // public void Dispose()
        // {
        //     // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //     Dispose(true);
        //     GC.SuppressFinalize(this);
        // }
        #endregion
        
    }
}
