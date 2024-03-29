//
//  GCDualSenseGamepad.h
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

extern "C" void* GCDualSenseGamepad_GetPropTouchpadButton(const void* ptr, const void** exceptionPtr);


extern "C" void* GCDualSenseGamepad_GetPropTouchpadPrimary(const void* ptr, const void** exceptionPtr);


extern "C" void* GCDualSenseGamepad_GetPropTouchpadSecondary(const void* ptr, const void** exceptionPtr);




extern "C" void GCDualSenseGamepad_Dispose(void* ptr);
