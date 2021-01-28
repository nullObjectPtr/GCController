//
//  GCMicroGamepad.h
//
//  Created by Jonathan Culp <jonathanculp@gmail.com> on 09/23/2020
//  Copyright © 2021 HovelHouseApps. All rights reserved.
//  Unauthorized copying of this file, via any medium is strictly prohibited
//  Proprietary and confidential
//

#import <Foundation/Foundation.h>
#import "Callbacks.h"

// Class Methods 

// Init Methods 

// Instance methods 

// Void methods 

// Properties 

extern "C" void* GCMicroGamepad_GetPropController(const void* ptr, const void** exceptionPtr);


extern "C" void* GCMicroGamepad_GetPropDpad(const void* ptr, const void** exceptionPtr);


extern "C" bool GCMicroGamepad_GetPropReportsAbsoluteDpadValues(const void* ptr, const void** exceptionPtr);
extern "C" void GCMicroGamepad_SetPropReportsAbsoluteDpadValues(const void* ptr, bool reportsAbsoluteDpadValues, const void** exceptionPtr);



extern "C" bool GCMicroGamepad_GetPropAllowsRotation(const void* ptr, const void** exceptionPtr);
extern "C" void GCMicroGamepad_SetPropAllowsRotation(const void* ptr, bool allowsRotation, const void** exceptionPtr);



extern "C" void* GCMicroGamepad_GetPropButtonA(const void* ptr, const void** exceptionPtr);


extern "C" void* GCMicroGamepad_GetPropButtonX(const void* ptr, const void** exceptionPtr);


extern "C" void* GCMicroGamepad_GetPropButtonMenu(const void* ptr, const void** exceptionPtr);

extern "C" void GCMicroGamepad_SetPropValueChangedHandler(const void* ptr, GCMicroGamepadValueChangedCallback valueChangedHandler, const void** exceptionPtr);




extern "C" void GCMicroGamepad_Dispose(void* ptr);
