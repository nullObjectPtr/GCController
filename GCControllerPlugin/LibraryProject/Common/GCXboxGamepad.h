//
//  GCXboxGamepad.h
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

extern "C" void* GCXboxGamepad_GetPropPaddleButton1(const void* ptr, const void** exceptionPtr);


extern "C" void* GCXboxGamepad_GetPropPaddleButton2(const void* ptr, const void** exceptionPtr);


extern "C" void* GCXboxGamepad_GetPropPaddleButton3(const void* ptr, const void** exceptionPtr);


extern "C" void* GCXboxGamepad_GetPropPaddleButton4(const void* ptr, const void** exceptionPtr);




extern "C" void GCXboxGamepad_Dispose(void* ptr);
