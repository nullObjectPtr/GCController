//
//  GCPhysicalInputProfile.h
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
extern "C" const char* GCPhysicalInputProfile_mappedElementAliasForPhysicalInputName(
	void* ptr,
	const char* inputName,
    void** exception
    );


// Void methods 

// Properties 

extern "C" void GCPhysicalInputProfile_GetPropElements(
    const void* ptr,
    const void** keysBuffer,
    const void** valuesBuffer,
    const void** typeTokenBuffer,
    long* const bufferLen,
    const void** exception);


extern "C" void GCPhysicalInputProfile_GetPropAllButtons(const void* ptr, const void** buffer, long* const count);


extern "C" void GCPhysicalInputProfile_GetPropAllDpads(const void* ptr, const void** buffer, long* const count);


extern "C" void GCPhysicalInputProfile_GetPropAllAxes(const void* ptr, const void** buffer, long* const count);


extern "C" void GCPhysicalInputProfile_GetPropAllElements(const void* ptr, const void** buffer, const void** typeTokenBuffer, long* const count);


extern "C" bool GCPhysicalInputProfile_GetPropHasRemappedElements(const void* ptr, const void** exceptionPtr);




extern "C" void GCPhysicalInputProfile_Dispose(void* ptr);
