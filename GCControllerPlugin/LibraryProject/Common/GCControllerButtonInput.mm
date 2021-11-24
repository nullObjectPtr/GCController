//
//  GCControllerButtonInput.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCControllerButtonInput.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties
bool GCControllerButtonInput_GetPropPressed(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerButtonInput* iGCControllerButtonInput = (__bridge GCControllerButtonInput*) ptr;
        BOOL pressed = [iGCControllerButtonInput isPressed];
        return pressed;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return NO;
}


float GCControllerButtonInput_GetPropValue(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerButtonInput* iGCControllerButtonInput = (__bridge GCControllerButtonInput*) ptr;
        float value = [iGCControllerButtonInput value];
        return value;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return 0;
}


bool GCControllerButtonInput_GetPropTouched(const void* ptr, const void** exceptionPtr)
{
    if(@available(macOS 10.16, iOS 14.0, tvOS 14.0, *))
    {
        @try
        {
            GCControllerButtonInput* iGCControllerButtonInput = (__bridge GCControllerButtonInput*) ptr;
            BOOL touched = [iGCControllerButtonInput isTouched];
            return touched;
        }
        @catch(NSException* ex)
        {
            *exceptionPtr = (__bridge_retained void*) ex;
        }
    }
    
    return NO;
}


void GCControllerButtonInput_SetPropValueChangedHandler(const void* ptr, GCControllerButtonValueChangedCallback valueChangedHandler, const void** exceptionPtr)
{
	@try 
	{
		id iGCControllerButtonInput = (__bridge GCControllerButtonInput*) ptr;
		[iGCControllerButtonInput setValueChangedHandler:^(GCControllerButtonInput* button,
            float value,
            BOOL pressed)
			{
				
				valueChangedHandler(ptr, (__bridge_retained void*) button, value, pressed);
				
			}];
	}
	@catch(NSException* ex)
	{
		*exceptionPtr = (__bridge_retained void*)ex;
	}
}

void GCControllerButtonInput_SetPropPressedChangedHandler(const void* ptr, GCControllerButtonValueChangedCallback pressedChangedHandler, const void** exceptionPtr)
{
	@try 
	{
		id iGCControllerButtonInput = (__bridge GCControllerButtonInput*) ptr;
		[iGCControllerButtonInput setPressedChangedHandler:^(GCControllerButtonInput* button,
	float value,
	BOOL pressed)
			{
				
				pressedChangedHandler(ptr, (__bridge_retained void*) button, value, pressed);
				
			}];
	}
	@catch(NSException* ex)
	{
		*exceptionPtr = (__bridge_retained void*)ex;
	}
}



void GCControllerButtonInput_Dispose(void* ptr)
{
    GCControllerButtonInput* val = (__bridge GCControllerButtonInput*) ptr;
    if(val != nil)
    {
        CFRelease(ptr);
    }
    NSLog(@"Dispose...GCControllerButtonInput");
}

}
