//
//  GCPhysicalInputProfile.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCPhysicalInputProfile.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties
void GCPhysicalInputProfile_GetPropElements(
	const void* ptr, 
	const void** keysBuffer,
	const void** valuesBuffer,
	long* const bufferLen)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        GCPhysicalInputProfile* iGCPhysicalInputProfile = (__bridge GCPhysicalInputProfile*) ptr;
        NSDictionary<NSString*,GCControllerElement*>* val = [iGCPhysicalInputProfile elements];
        
        // Template assumes key is always a string
        const char** _keyBuffer = (const char**) malloc(sizeof(char*) * [val count]);
        const void** _valueBuffer = (const void**) malloc(sizeof(void*) * [val count]);
        
        int i = 0;
        
        for(id key in val)
        {
            _keyBuffer[i] = [key UTF8String];
            _valueBuffer[i] = (__bridge void*) [val valueForKey:key];
            ++i;
        }
        
        *bufferLen = [val count];
        *keysBuffer = _keyBuffer;
        *valuesBuffer = _valueBuffer;
    }
    else
    {
        *bufferLen = 0;
        *keysBuffer = NULL;
        *valuesBuffer = NULL;
    }
}

void GCPhysicalInputProfile_GetPropAllButtons(const void* ptr, const void** buffer, long* const count)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        GCPhysicalInputProfile* iGCPhysicalInputProfile = (__bridge GCPhysicalInputProfile*) ptr;
        NSSet<GCControllerButtonInput*>* val = [iGCPhysicalInputProfile allButtons];

        *buffer = [GCControllerConverters NSSetToRetainedCArray:val];
        *count = [val count];
    }
    else
    {
        *buffer = NULL;
        *count = 0;
    }
}

void GCPhysicalInputProfile_GetPropAllDpads(const void* ptr, const void** buffer, long* const count)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        GCPhysicalInputProfile* iGCPhysicalInputProfile = (__bridge GCPhysicalInputProfile*) ptr;
        NSSet<GCControllerDirectionPad*>* val = [iGCPhysicalInputProfile allDpads];

        *buffer = [GCControllerConverters NSSetToRetainedCArray:val];
        *count = [val count];
    }
    else
    {
        *buffer = NULL;
        *count = 0;
    }
}

void GCPhysicalInputProfile_GetPropAllAxes(const void* ptr, const void** buffer, long* count)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        GCPhysicalInputProfile* iGCPhysicalInputProfile = (__bridge GCPhysicalInputProfile*) ptr;
        NSSet<GCControllerAxisInput*>* val = [iGCPhysicalInputProfile allAxes];

        *buffer = [GCControllerConverters NSSetToRetainedCArray:val];
        *count = [val count];
    }
    else
    {
        *buffer = NULL;
        *count = 0;
    }
}


void GCPhysicalInputProfile_GetPropAllElements(const void* ptr, const void** buffer, long* count)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        GCPhysicalInputProfile* iGCPhysicalInputProfile = (__bridge GCPhysicalInputProfile*) ptr;
        NSSet<GCControllerElement*>* val = [iGCPhysicalInputProfile allElements];

        *buffer = [GCControllerConverters NSSetToRetainedCArray:val];
        *count = [val count];
    }
    else
    {
        *buffer = NULL;
        *count = 0;
    }
}

const void* GCPhysicalInputProfile_ObjectForKeyedSubscript(
   void* ptr,
   const char* key,
   const void** exceptionPtr)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        try
        {
            GCPhysicalInputProfile* iGCPhysicalInputProfile = (__bridge GCPhysicalInputProfile*) ptr;
            NSString* nsKey = [NSString stringWithUTF8String:key];
            return (__bridge_retained const void*) iGCPhysicalInputProfile[nsKey];
        }
        catch(NSException* ex)
        {
            *exceptionPtr = (__bridge_retained const void*) ex;
        }
    }
    
    return nil;
}

void GCPhysicalInputProfile_Dispose(void* ptr)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        GCPhysicalInputProfile* val = (__bridge GCPhysicalInputProfile*) ptr;
        if(val != nil)
        {
            CFRelease(ptr);
        }
        //NSLog(@"Dispose...GCPhysicalInputProfile");
    }
}

}
