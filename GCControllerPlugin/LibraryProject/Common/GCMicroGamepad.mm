//
//  GCMicroGamepad.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCMicroGamepad.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties
void* GCMicroGamepad_GetPropController(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCMicroGamepad* iGCMicroGamepad = (__bridge GCMicroGamepad*) ptr;
        GCController* controller = [iGCMicroGamepad controller];
        return (__bridge void*) controller;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCMicroGamepad_GetPropDpad(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCMicroGamepad* iGCMicroGamepad = (__bridge GCMicroGamepad*) ptr;
        GCControllerDirectionPad* dpad = [iGCMicroGamepad dpad];
        return (__bridge void*) dpad;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


bool GCMicroGamepad_GetPropReportsAbsoluteDpadValues(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCMicroGamepad* iGCMicroGamepad = (__bridge GCMicroGamepad*) ptr;
        BOOL reportsAbsoluteDpadValues = [iGCMicroGamepad reportsAbsoluteDpadValues];
        return reportsAbsoluteDpadValues;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return NO;
}

void GCMicroGamepad_SetPropReportsAbsoluteDpadValues(const void* ptr, bool reportsAbsoluteDpadValues, const void** exceptionPtr)
{
	@try
	{
		GCMicroGamepad* iGCMicroGamepad = (__bridge GCMicroGamepad*) ptr;
		[iGCMicroGamepad setReportsAbsoluteDpadValues:reportsAbsoluteDpadValues];
	}
	@catch(NSException* ex) 
	{
		*exceptionPtr = (__bridge_retained void*) ex;
	}
}


bool GCMicroGamepad_GetPropAllowsRotation(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCMicroGamepad* iGCMicroGamepad = (__bridge GCMicroGamepad*) ptr;
        BOOL allowsRotation = [iGCMicroGamepad allowsRotation];
        return allowsRotation;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return NO;
}

void GCMicroGamepad_SetPropAllowsRotation(const void* ptr, bool allowsRotation, const void** exceptionPtr)
{
	@try
	{
		GCMicroGamepad* iGCMicroGamepad = (__bridge GCMicroGamepad*) ptr;
		[iGCMicroGamepad setAllowsRotation:allowsRotation];
	}
	@catch(NSException* ex) 
	{
		*exceptionPtr = (__bridge_retained void*) ex;
	}
}


void* GCMicroGamepad_GetPropButtonA(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCMicroGamepad* iGCMicroGamepad = (__bridge GCMicroGamepad*) ptr;
        GCControllerButtonInput* buttonA = [iGCMicroGamepad buttonA];
        return (__bridge void*) buttonA;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCMicroGamepad_GetPropButtonX(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCMicroGamepad* iGCMicroGamepad = (__bridge GCMicroGamepad*) ptr;
        GCControllerButtonInput* buttonX = [iGCMicroGamepad buttonX];
        return (__bridge void*) buttonX;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCMicroGamepad_GetPropButtonMenu(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCMicroGamepad* iGCMicroGamepad = (__bridge GCMicroGamepad*) ptr;
        GCControllerButtonInput* buttonMenu = [iGCMicroGamepad buttonMenu];
        return (__bridge void*) buttonMenu;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void GCMicroGamepad_SetPropValueChangedHandler(const void* ptr, GCMicroGamepadValueChangedCallback valueChangedHandler, const void** exceptionPtr)
{
	@try
	{
		id iGCMicroGamepad = (__bridge GCMicroGamepad*) ptr;
		[iGCMicroGamepad setValueChangedHandler:^(GCMicroGamepad* gamepad,
                                                  GCControllerElement* element)
			{
                long elementClassType = 0;
                if([element isKindOfClass:[GCControllerButtonInput class]])
                {
                    elementClassType = 1;
                }
                else if([element isKindOfClass:[GCControllerAxisInput class]])
                {
                    elementClassType = 2;
                }
                else if([element isKindOfClass:[GCDeviceCursor class]])
                {
                    elementClassType = 3;
                }
                else if([element isKindOfClass:[GCControllerDirectionPad class]])
                {
                    elementClassType = 4;
                }
                else if([element isKindOfClass:[GCControllerTouchpad class]])
                {
                    elementClassType = 5;
                }
            
				valueChangedHandler(ptr, (__bridge_retained void*) gamepad, (__bridge_retained void*) element, elementClassType);
				
			}];
	}
	@catch(NSException* ex)
	{
		*exceptionPtr = (__bridge_retained void*)ex;
	}
}



void GCMicroGamepad_Dispose(void* ptr)
{
    GCMicroGamepad* val = (__bridge GCMicroGamepad*) ptr;
    if(val != nil)
    {
        CFRelease(ptr);
    }
    NSLog(@"Dispose...GCMicroGamepad");
}

}
