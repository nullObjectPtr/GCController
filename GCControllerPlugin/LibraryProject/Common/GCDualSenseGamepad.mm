//
//  GCDualSenseGamepad.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCDualSenseGamepad.h"
#import "GCControllerConverters.h"



extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties
void* GCDualSenseGamepad_GetPropTouchpadButton(const void* ptr, const void** exceptionPtr)
{

    @try
    {
        GCDualSenseGamepad* iGCDualSenseGamepad = (__bridge GCDualSenseGamepad*) ptr;
        GCControllerButtonInput* touchpadButton = [iGCDualSenseGamepad touchpadButton];
        return (__bridge void*) touchpadButton;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    return nil;
}

void* GCDualSenseGamepad_GetPropTouchpadPrimary(const void* ptr, const void** exceptionPtr)
{

    @try
    {
        GCDualSenseGamepad* iGCDualSenseGamepad = (__bridge GCDualSenseGamepad*) ptr;
        GCControllerDirectionPad* touchpadPrimary = [iGCDualSenseGamepad touchpadPrimary];
        return (__bridge void*) touchpadPrimary;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    return nil;
}

void* GCDualSenseGamepad_GetPropTouchpadSecondary(const void* ptr, const void** exceptionPtr)
{

    @try
    {
        GCDualSenseGamepad* iGCDualSenseGamepad = (__bridge GCDualSenseGamepad*) ptr;
        GCControllerDirectionPad* touchpadSecondary = [iGCDualSenseGamepad touchpadSecondary];
        return (__bridge void*) touchpadSecondary;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    return nil;
}



void GCDualSenseGamepad_Dispose(void* ptr)
{ 
        GCDualSenseGamepad* val = (__bridge GCDualSenseGamepad*) ptr;
        if(val != nil)
        {
            CFRelease(ptr);
        }
        //NSLog(@"Dispose...GCDualSenseGamepad");
}

}
