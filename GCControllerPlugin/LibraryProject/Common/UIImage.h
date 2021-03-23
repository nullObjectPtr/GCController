//
//  UIImage.h
//
//  Created by Jonathan Culp <jonathanculp@gmail.com> on 09/23/2020
//  Copyright © 2021 HovelHouseApps. All rights reserved.
//  Unauthorized copying of this file, via any medium is strictly prohibited
//  Proprietary and confidential
//

#import <Foundation/Foundation.h>
#import "Callbacks.h"

// Class Methods 
extern "C" void* UIImage_systemImageNamed(
	const char* name,
    void** exception
    );

#if TARGET_OS_IOS || TARGET_OS_TV
extern "C" void* UIImage_systemImageNamed_compatibleWithTraitCollection(
	const char* name,
    void* traitCollection,
    void** exception
    );
#endif

#if TARGET_OS_IOS || TARGET_OS_TV
extern "C" void* UIImage_systemImageNamed_withConfiguration(
	const char* name,
    void* configuration,
    void** exception
    );
#endif

extern "C" void* UIImage_PNGRepresentation_GetData(
    const void* image,
    long* const bufferLen,
    void** exception
    );

extern "C" void* UIImage_JPEGRepresentation_GetData(
	const void* image,
    float compressionQuality,
    long* const bufferLen,
    void** exception
    );

extern "C" void UIImage_CopyBufferAndReleaseData(
    const void* data,
    Byte* buffer
    );

extern "C" bool UIImage_SFSymbolsAreAvailable();


// Init Methods 

// Instance methods 

// Void methods 

// Properties 



extern "C" void UIImage_Dispose(void* ptr);
