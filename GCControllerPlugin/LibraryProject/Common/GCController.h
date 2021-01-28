//
//  GCController.h
//
//  Created by Jonathan Culp <jonathanculp@gmail.com> on 09/23/2020
//  Copyright © 2021 HovelHouseApps. All rights reserved.
//  Unauthorized copying of this file, via any medium is strictly prohibited
//  Proprietary and confidential
//

#import <Foundation/Foundation.h>
#import "Callbacks.h"

// Class Methods 
extern "C" void GCController_controllers(
	const void** buffer,
    long* const bufferLength,
    const void** exception
    );


extern "C" void* GCController_controllerWithExtendedGamepad(
	void** exception
    );

extern "C" void* GCController_controllerWithMicroGamepad(
	void** exception
    );


// Init Methods 

// Instance methods 
extern "C" void* GCController_capture(
	void* ptr,
	void** exception
    );


// Void methods 

// Properties 

extern "C" void* GCController_GetPropPhysicalInputProfile(const void* ptr, long* const typeToken, const void** exceptionPtr);


extern "C" bool GCController_GetPropAttachedToDevice(const void* ptr, const void** exceptionPtr);


extern "C" bool GCController_GetPropSnapshot(const void* ptr, const void** exceptionPtr);


extern "C" void* GCController_GetPropExtendedGamepad(const void* ptr, long* const typeToken, const void** exceptionPtr);


extern "C" void* GCController_GetPropMicroGamepad(const void* ptr, const void** exceptionPtr);


extern "C" const char* GCController_GetPropProductCategory(const void* ptr, const void** exceptionPtr);


extern "C" const char* GCController_GetPropVendorName(const void* ptr, const void** exceptionPtr);


extern "C" void* GCController_GetPropCurrent(const void** exceptionPtr);




extern "C" void GCController_Dispose(void* ptr);
