//
//  GCControllerElement.h
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

extern "C" const char* GCControllerElement_GetPropLocalizedName(const void* ptr, const void** exceptionPtr);
extern "C" void GCControllerElement_SetPropLocalizedName(const void* ptr, const char* localizedName, const void** exceptionPtr);



extern "C" const char* GCControllerElement_GetPropSfSymbolsName(const void* ptr, const void** exceptionPtr);
extern "C" void GCControllerElement_SetPropSfSymbolsName(const void* ptr, const char* sfSymbolsName, const void** exceptionPtr);



extern "C" const char* GCControllerElement_GetPropUnmappedLocalizedName(const void* ptr, const void** exceptionPtr);
extern "C" void GCControllerElement_SetPropUnmappedLocalizedName(const void* ptr, const char* unmappedLocalizedName, const void** exceptionPtr);



extern "C" const char* GCControllerElement_GetPropUnmappedSfSymbolsName(const void* ptr, const void** exceptionPtr);
extern "C" void GCControllerElement_SetPropUnmappedSfSymbolsName(const void* ptr, const char* unmappedSfSymbolsName, const void** exceptionPtr);



extern "C" bool GCControllerElement_GetPropBoundToSystemGesture(const void* ptr, const void** exceptionPtr);


extern "C" bool GCControllerElement_GetPropAnalog(const void* ptr, const void** exceptionPtr);


extern "C" void* GCControllerElement_GetPropCollection(const void* ptr, const void** exceptionPtr);




extern "C" void GCControllerElement_Dispose(void* ptr);
