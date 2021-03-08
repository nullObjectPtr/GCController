//
//  UIImage.cs
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
    public class UIImage : UnmanagedObject, IDisposable
    {
        #region dll
        
        #if !UNITY_EDITOR && UNITY_IPHONE || UNITY_TVOS
        const string dll = "__Internal";
        #else
        const string dll = "HHGCControllerMacOS";
        #endif

        // Class Methods
        
        
        [DllImport(dll)]
        private static extern IntPtr UIImage_systemImageNamed(
            string name,
            out IntPtr exceptionPtr);

        
        
        [DllImport(dll)]
        private static extern IntPtr UIImage_systemImageNamed_compatibleWithTraitCollection(
            string name,
            IntPtr traitCollection,
            out IntPtr exceptionPtr);

        
        
        [DllImport(dll)]
        private static extern IntPtr UIImage_systemImageNamed_withConfiguration(
            string name,
            IntPtr configuration,
            out IntPtr exceptionPtr);

        
        
        [DllImport(dll)]
        private static extern IntPtr UIImage_PNGRepresentation_GetData(
            IntPtr image,
            out long bufferLen,
            out IntPtr exceptionPtr);


        [DllImport(dll)]
        private static extern IntPtr UIImage_JPEGRepresentation_GetData(
            IntPtr image,
            float compressionQuality,
            out long bufferLen,
            out IntPtr exceptionPtr);

        [DllImport(dll)]
        private static extern void UIImage_CopyBufferAndReleaseData(
            IntPtr nsDataPtr,
            byte[] buffer
            );
        
        [DllImport(dll)]
        private static extern bool UIImage_SFSymbolsAreAvailable();
        
        // Properties
        

        #endregion

        internal UIImage(IntPtr ptr) : base(ptr) {}
        internal UIImage(IntPtr ptr, ERetainPolicy retainPolicy) : base(ptr, retainPolicy){}
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns>val</returns>
        public static UIImage SystemImageNamed(
            string name)
        { 
            
            var val = UIImage_systemImageNamed(
                name,
                out var exceptionPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return val == IntPtr.Zero ? null : new UIImage(val);
        }
        

        /// <summary>
        /// </summary>
        /// <param name="name"></param><param name="traitCollection"></param>
        /// <returns>val</returns>
        public static UIImage SystemImageNamed(
            string name, 
            UITraitCollection traitCollection)
        {
#if UNITY_STANALONE_OSX || UNITY_EDITOR_OSX
            Debug.LogWarning("SystemImageName_compatibleWithTrait collection is not implemented in OSX");
            return null;
#else
            var val = UIImage_systemImageNamed_compatibleWithTraitCollection(
                name,
                traitCollection != null ? HandleRef.ToIntPtr(traitCollection.Handle) : IntPtr.Zero,
                out var exceptionPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return val == IntPtr.Zero ? null : new UIImage(val);
#endif
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param><param name="configuration"></param>
        /// <returns>val</returns>
        public static UIImage SystemImageNamed(
            string name, 
            UIImageConfiguration configuration)
        {
            var val = UIImage_systemImageNamed_withConfiguration(
                name,
                configuration != null ? HandleRef.ToIntPtr(configuration.Handle) : IntPtr.Zero,
                out var exceptionPtr);

            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }
            
            return val == IntPtr.Zero ? null : new UIImage(val);
        }
        

        /// <summary>
        /// </summary>
        /// <param name="image"></param>
        /// <returns>val</returns>
        public static byte[] PNGRepresentation(
            UIImage image)
        { 
            
            var dataPtr = UIImage_PNGRepresentation_GetData(
                image != null ? HandleRef.ToIntPtr(image.Handle) : IntPtr.Zero,
                out var bufferLen,
                out var exceptionPtr);
            
            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }

            var bytes = new byte[bufferLen];

            UIImage_CopyBufferAndReleaseData(dataPtr, bytes);

            return bytes;
        }
        

        
        /// <summary>
        /// </summary>
        /// <param name="image"></param>
        /// <returns>val</returns>
        public static byte[] JPEGRepresentation(
            UIImage image,
            float compressionQuality)
        { 
            var nsDataPtr = UIImage_JPEGRepresentation_GetData(
                image != null ? HandleRef.ToIntPtr(image.Handle) : IntPtr.Zero,
                compressionQuality,
                out var bufferLen,
                out var exceptionPtr);
            
            if(exceptionPtr != IntPtr.Zero)
            {
                var nativeException = new NSException(exceptionPtr);
                throw new GameControllerException(nativeException, nativeException.Reason);
            }

            var bytes = new byte[bufferLen];
            
            UIImage_CopyBufferAndReleaseData(nsDataPtr, bytes);

            return bytes;
        }

        public static bool SFSymbolsAreAvailable()
        {
            return UIImage_SFSymbolsAreAvailable();
        }
        
        
        


        
        
        

        

        
        #region IDisposable Support
        [DllImport(dll)]
        private static extern void UIImage_Dispose(HandleRef handle);
            
        private bool disposedValue = false; // To detect redundant calls
        
        protected virtual void Dispose(bool disposing)
        {
            if (_retainPolicy == ERetainPolicy.Retained && !disposedValue)
            {
                //Debug.Log("UIImage Dispose");
                UIImage_Dispose(Handle);
                disposedValue = true;
            }
        }

        ~UIImage()
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
