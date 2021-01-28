//
//  GCControllerButtonInput.h
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

extern "C" bool GCControllerButtonInput_GetPropPressed(const void* ptr, const void** exceptionPtr);


extern "C" float GCControllerButtonInput_GetPropValue(const void* ptr, const void** exceptionPtr);


extern "C" bool GCControllerButtonInput_GetPropTouched(const void* ptr, const void** exceptionPtr);

extern "C" void GCControllerButtonInput_SetPropValueChangedHandler(const void* ptr, GCControllerButtonValueChangedCallback valueChangedHandler, const void** exceptionPtr);

extern "C" void GCControllerButtonInput_SetPropPressedChangedHandler(const void* ptr, GCControllerButtonValueChangedCallback pressedChangedHandler, const void** exceptionPtr);




extern "C" void GCControllerButtonInput_Dispose(void* ptr);
