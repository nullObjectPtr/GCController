//
//  GCExtendedGamepad.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCExtendedGamepad.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties
void* GCExtendedGamepad_GetPropController(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCController* controller = [iGCExtendedGamepad controller];
        return (__bridge void*) controller;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void GCExtendedGamepad_SetPropValueChangedHandler(const void* ptr, GCExtendedGamepadValueChangedCallback callback, const void** exceptionPtr)
{
	@try 
	{
		id iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
		[iGCExtendedGamepad setValueChangedHandler:^(
                                                     GCExtendedGamepad* gamepad,
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
            
            long gamepadClassType = 0;
            if([gamepad isKindOfClass:[GCDualShockGamepad class]])
            {
                gamepadClassType = 1;
            }
            else if([gamepad isKindOfClass:[GCXboxGamepad class]])
            {
                gamepadClassType = 2;
            }
        
            // default case - type token = 0 means that the element is of the base-class type GCControllerElement
            // shouldn't ever happen, but we want to handle future updates cleanly.

            callback(ptr, (__bridge void*) gamepad, gamepadClassType, (__bridge void*) element, elementClassType);
        }];
	}
	@catch(NSException* ex)
	{
		*exceptionPtr = (__bridge_retained void*)ex;
	}
}

void* GCExtendedGamepad_GetPropLeftShoulder(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* leftShoulder = [iGCExtendedGamepad leftShoulder];
        return (__bridge void*) leftShoulder;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropRightShoulder(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* rightShoulder = [iGCExtendedGamepad rightShoulder];
        return (__bridge void*) rightShoulder;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropDpad(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerDirectionPad* dpad = [iGCExtendedGamepad dpad];
        return (__bridge void*) dpad;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropButtonA(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* buttonA = [iGCExtendedGamepad buttonA];
        return (__bridge void*) buttonA;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropButtonB(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* buttonB = [iGCExtendedGamepad buttonB];
        return (__bridge void*) buttonB;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropButtonX(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* buttonX = [iGCExtendedGamepad buttonX];
        return (__bridge void*) buttonX;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropButtonY(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* buttonY = [iGCExtendedGamepad buttonY];
        return (__bridge void*) buttonY;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropLeftThumbstick(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerDirectionPad* leftThumbstick = [iGCExtendedGamepad leftThumbstick];
        return (__bridge void*) leftThumbstick;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropRightThumbstick(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerDirectionPad* rightThumbstick = [iGCExtendedGamepad rightThumbstick];
        return (__bridge void*) rightThumbstick;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropLeftTrigger(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* leftTrigger = [iGCExtendedGamepad leftTrigger];
        return (__bridge void*) leftTrigger;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropRightTrigger(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* rightTrigger = [iGCExtendedGamepad rightTrigger];
        return (__bridge void*) rightTrigger;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropButtonHome(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* buttonHome = [iGCExtendedGamepad buttonHome];
        return (__bridge void*) buttonHome;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropButtonMenu(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* buttonMenu = [iGCExtendedGamepad buttonMenu];
        return (__bridge void*) buttonMenu;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropButtonOptions(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* buttonOptions = [iGCExtendedGamepad buttonOptions];
        return (__bridge void*) buttonOptions;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropLeftThumbstickButton(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* leftThumbstickButton = [iGCExtendedGamepad leftThumbstickButton];
        return (__bridge void*) leftThumbstickButton;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCExtendedGamepad_GetPropRightThumbstickButton(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCExtendedGamepad* iGCExtendedGamepad = (__bridge GCExtendedGamepad*) ptr;
        GCControllerButtonInput* rightThumbstickButton = [iGCExtendedGamepad rightThumbstickButton];
        return (__bridge void*) rightThumbstickButton;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}




void GCExtendedGamepad_Dispose(void* ptr)
{
    GCExtendedGamepad* val = (__bridge GCExtendedGamepad*) ptr;
    if(val != nil)
    {
        CFRelease(ptr);
    }
    NSLog(@"Dispose...GCExtendedGamepad");
}

}
