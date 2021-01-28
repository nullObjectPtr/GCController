//
//  GCControllerDirectionPad.h
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

extern "C" void* GCControllerDirectionPad_GetPropUp(const void* ptr, const void** exceptionPtr);


extern "C" void* GCControllerDirectionPad_GetPropDown(const void* ptr, const void** exceptionPtr);


extern "C" void* GCControllerDirectionPad_GetPropLeft(const void* ptr, const void** exceptionPtr);


extern "C" void* GCControllerDirectionPad_GetPropRight(const void* ptr, const void** exceptionPtr);


extern "C" void* GCControllerDirectionPad_GetPropXAxis(const void* ptr, const void** exceptionPtr);


extern "C" void* GCControllerDirectionPad_GetPropYAxis(const void* ptr, const void** exceptionPtr);




extern "C" void GCControllerDirectionPad_Dispose(void* ptr);
