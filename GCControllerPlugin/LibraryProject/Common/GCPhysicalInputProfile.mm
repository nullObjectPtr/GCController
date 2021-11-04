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

Byte GCPhysicalInputProfile_GetTypeTokenForControllerElement(id element)
{
    // GCDualSenseAdaptiveTrigger - ButtonInput = 1
    // GCDeviceCursor - GCControllerDirectionPad = 2
    // GCControllerDirectionPad - GCControllerElement = 3
    // GCControllerTouchpad - GCControllerElement = 4
    // GCControllerButtonInput - GCControllerElement = 5
    // GCControllerAxisInput - GCControllerElement = 6
    
    if(@available(iOS 14.5, macOS 11.3, macCatalyst 14.5, tvOS 14.5, *))
    {
        if([element isKindOfClass:[GCDualSenseAdaptiveTrigger class]])
        {
            return 1;
        }
    }

    if(@available(iOS 14.0, macOS 10.16, macCatalyst 14.0, tvOS 14.0, *))
    {
        if([element isKindOfClass:[GCDeviceCursor class]])
        {
            return 2;
        }
    }
    
    if(@available(iOS 7.0, macOS 10.9, macCatalyst 13.0, tvOS 9.0, *))
    {
        if([element isKindOfClass:[GCControllerDirectionPad class]])
        {
            return 3;
        }
    }
    
    if(@available(iOS 14.0, macOS 10.16, macCatalyst 14.0, tvOS 14.0, *))
    {
        if([element isKindOfClass:[GCControllerTouchpad class]])
        {
            return 4;
        }
    }
    
    if(@available(iOS 7.0, macOS 10.9, macCatalyst 13.0, tvOS 9.0, *))
    {
        if([element isKindOfClass:[GCControllerButtonInput class]])
        {
            return 5;
        }
    }
    
    if(@available(iOS 7.0, macOS 10.9, macCatalyst 13.0, tvOS 9.0, *))
    {
        if([element isKindOfClass:[GCControllerAxisInput class]])
        {
            return 6;
        }
    }
    
    if(@available(iOS 7.0, macOS 10.9, macCatalyst 13.0, tvOS 9.0, *))
    {
        if([element isKindOfClass:[GCControllerElement class]])
        {
            return 7;
        }
    }
    
    NSLog(@"Encountered unknown Controller Element Type");
    return -1;
}

//ClassMethods
//InitMethods
//InstanceMethods
const char* GCPhysicalInputProfile_mappedElementAliasForPhysicalInputName(
    void* ptr,
    const char* inputName,
    void** exception
    )
{ 
    if(@available(iOS 15.0, macOS 12.0, macCatalyst 15.0, tvOS 15.0, * ))
    { 
        @try
        {
            GCPhysicalInputProfile* iGCPhysicalInputProfile = (__bridge GCPhysicalInputProfile*) ptr;
            NSString* val = [iGCPhysicalInputProfile mappedElementAliasForPhysicalInputName:[NSString stringWithUTF8String:inputName]];
            return [val UTF8String];
        }
        @catch(NSException* ex)
        {
            *exception = (__bridge_retained void*) ex;
        }
        return nil;
    }
    else
    {
        return nil;
    }
}



//VoidMethods
//Properties
void GCPhysicalInputProfile_GetPropElements(
	const void* ptr, 
	const void** keysBuffer,
	const void** valuesBuffer,
    const void** typeTokenBuffer,
	long* const bufferLen,
    const void** exceptionPtr)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        try {
            GCPhysicalInputProfile* iGCPhysicalInputProfile = (__bridge GCPhysicalInputProfile*) ptr;
            NSDictionary<NSString*,GCControllerElement*>* val = [iGCPhysicalInputProfile elements];
            
            // Template assumes key is always a string
            const char** _keyBuffer = (const char**) malloc(sizeof(char*) * [val count]);
            const void** _valueBuffer = (const void**) malloc(sizeof(void*) * [val count]);
            Byte* _typeTokenBuffer = (Byte*) malloc(sizeof(Byte) * [val count]);
            
            NSLog(@"type token %d", *_typeTokenBuffer);
            
            int i = 0;
            
            for(id key in val)
            {
                _keyBuffer[i] = [key UTF8String];
                id element = [val valueForKey:key];
                _valueBuffer[i] = (__bridge void*) element;
                _typeTokenBuffer[i] = GCPhysicalInputProfile_GetTypeTokenForControllerElement(element);
                ++i;
            }
            
            *bufferLen = [val count];
            *keysBuffer = _keyBuffer;
            *valuesBuffer = _valueBuffer;
            *typeTokenBuffer = _typeTokenBuffer;
        }
        catch(NSException* ex)
        {
            *exceptionPtr = (__bridge_retained const void*)ex;
        }
    }
    else
    {
        *bufferLen = 0;
        *keysBuffer = NULL;
        *valuesBuffer = NULL;
        typeTokenBuffer = NULL;
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


void GCPhysicalInputProfile_GetPropAllElements(const void* ptr, const void** buffer, const void** typeTokenBuffer, long* count)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        GCPhysicalInputProfile* iGCPhysicalInputProfile = (__bridge GCPhysicalInputProfile*) ptr;
        NSSet<GCControllerElement*>* allElements = [iGCPhysicalInputProfile allElements];

        *buffer = [GCControllerConverters NSSetToRetainedCArray:allElements];
        Byte* _typeTokenBuffer = (Byte*) malloc(sizeof(Byte) * [allElements count]);
        
        int i = 0;
        for(id element in allElements)
        {
            _typeTokenBuffer[i] = GCPhysicalInputProfile_GetTypeTokenForControllerElement(element);
            ++i;
        }
        
        *typeTokenBuffer = _typeTokenBuffer;
        *count = [allElements count];
    }
    else
    {
        *buffer = NULL;
        *typeTokenBuffer = NULL;
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
