//
//  UIImageSymbolConfiguration.cs
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
    public class UIImageSymbolConfiguration : UIImageConfiguration, IDisposable
    {
        #region dll
        
        #if !UNITY_EDITOR && UNITY_IPHONE || UNITY_TVOS
        const string dll = "__Internal";
        #else
        const string dll = "HHGCControllerMacOS";
        #endif

        // Class Methods
        
        
        [DllImport(dll)]
        private static extern IntPtr UIImageSymbolConfiguration_configurationWithPointSize_weight(
            double pointSize,
            long weight,
            out IntPtr exceptionPtr);

        
        
        [DllImport(dll)]
        private static extern IntPtr UIImageSymbolConfiguration_configurationWithPointSize_weight_scale(
            double pointSize,
            long weight,
            long scale,
            out IntPtr exceptionPtr);

        
        
        [DllImport(dll)]
        private static extern IntPtr UIImageSymbolConfiguration_configurationWithScale(
            long scale,
            out IntPtr exceptionPtr);

        
        
        [DllImport(dll)]
        private static extern IntPtr UIImageSymbolConfiguration_configurationWithTextStyle(
            string textStyle,
            out IntPtr exceptionPtr);

        
        
        [DllImport(dll)]
        private static extern IntPtr UIImageSymbolConfiguration_configurationWithTextStyle_scale(
            string textStyle,
            long scale,
            out IntPtr exceptionPtr);

        

        

        

        

        // Properties
        

        #endregion

        internal UIImageSymbolConfiguration(IntPtr ptr) : base(ptr) {}
        internal UIImageSymbolConfiguration(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        /// <summary>
        /// </summary>
        /// <param name="pointSize"></param><param name="weight"></param>
        /// <returns>val</returns>
        public static UIImageSymbolConfiguration ConfigurationWithPointSize(
            double pointSize, 
            UIImageSymbolWeight weight)
        { 
            
            
            var val = UIImageSymbolConfiguration_configurationWithPointSize_weight(
                pointSize,
                (long) weight,
                out var exceptionPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return val == IntPtr.Zero ? null : new UIImageSymbolConfiguration(val);
        }
        

        /// <summary>
        /// </summary>
        /// <param name="pointSize"></param><param name="weight"></param><param name="scale"></param>
        /// <returns>val</returns>
        public static UIImageSymbolConfiguration ConfigurationWithPointSize(
            double pointSize, 
            UIImageSymbolWeight weight, 
            UIImageSymbolScale scale)
        { 
            
            
            
            var val = UIImageSymbolConfiguration_configurationWithPointSize_weight_scale(
                pointSize,
                (long) weight,
                (long) scale,
                out var exceptionPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return val == IntPtr.Zero ? null : new UIImageSymbolConfiguration(val);
        }
        

        /// <summary>
        /// </summary>
        /// <param name="scale"></param>
        /// <returns>val</returns>
        public static UIImageSymbolConfiguration ConfigurationWithScale(
            UIImageSymbolScale scale)
        { 
            
            var val = UIImageSymbolConfiguration_configurationWithScale(
                (long) scale,
                out var exceptionPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return val == IntPtr.Zero ? null : new UIImageSymbolConfiguration(val);
        }
        

        /// <summary>
        /// </summary>
        /// <param name="textStyle"></param>
        /// <returns>val</returns>
        public static UIImageSymbolConfiguration ConfigurationWithTextStyle(
            string textStyle)
        { 
            
            var val = UIImageSymbolConfiguration_configurationWithTextStyle(
                textStyle,
                out var exceptionPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return val == IntPtr.Zero ? null : new UIImageSymbolConfiguration(val);
        }
        

        /// <summary>
        /// </summary>
        /// <param name="textStyle"></param><param name="scale"></param>
        /// <returns>val</returns>
        public static UIImageSymbolConfiguration ConfigurationWithTextStyle(
            string textStyle, 
            UIImageSymbolScale scale)
        { 
            
            
            var val = UIImageSymbolConfiguration_configurationWithTextStyle_scale(
                textStyle,
                (long) scale,
                out var exceptionPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return val == IntPtr.Zero ? null : new UIImageSymbolConfiguration(val);
        }
        

        
        


        
        
        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void UIImageSymbolConfiguration_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        protected void Dispose(bool disposing)
        {
            if (_retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("UIImageSymbolConfiguration Dispose");
                UIImageSymbolConfiguration_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~UIImageSymbolConfiguration()
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
