//
//  NSException.h
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

extern "C" const char* NSException_GetPropName(const void* ptr, const void** exceptionPtr);


extern "C" const char* NSException_GetPropReason(const void* ptr, const void** exceptionPtr);

// TODO: HEADERPROPERTYSTRINGARRAY



extern "C" void NSException_Dispose(void* ptr);
