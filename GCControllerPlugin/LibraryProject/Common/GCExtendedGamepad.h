//
//  GCExtendedGamepad.h
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

extern "C" void* GCExtendedGamepad_GetPropController(const void* ptr, const void** exceptionPtr);

extern "C" void GCExtendedGamepad_SetPropValueChangedHandler(const void* ptr, GCExtendedGamepadValueChangedCallback valueChangedHandler, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropLeftShoulder(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropRightShoulder(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropDpad(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropButtonA(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropButtonB(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropButtonX(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropButtonY(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropLeftThumbstick(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropRightThumbstick(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropLeftTrigger(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropRightTrigger(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropButtonHome(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropButtonMenu(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropButtonOptions(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropLeftThumbstickButton(const void* ptr, const void** exceptionPtr);


extern "C" void* GCExtendedGamepad_GetPropRightThumbstickButton(const void* ptr, const void** exceptionPtr);




extern "C" void GCExtendedGamepad_Dispose(void* ptr);
