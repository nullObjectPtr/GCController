//
//  GCDualShockGamepad.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCDualShockGamepad.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties
void* GCDualShockGamepad_GetPropTouchpadButton(const void* ptr, const void** exceptionPtr)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        @try
        {
            GCDualShockGamepad* iGCDualShockGamepad = (__bridge GCDualShockGamepad*) ptr;
            GCControllerButtonInput* touchpadButton = [iGCDualShockGamepad touchpadButton];
            return (__bridge void*) touchpadButton;
        }
        @catch(NSException* ex)
        {
            *exceptionPtr = (__bridge_retained void*) ex;
        }
    }
    
    return nil;
}


void* GCDualShockGamepad_GetPropTouchpadPrimary(const void* ptr, const void** exceptionPtr)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        @try
        {
            GCDualShockGamepad* iGCDualShockGamepad = (__bridge GCDualShockGamepad*) ptr;
            GCControllerDirectionPad* touchpadPrimary = [iGCDualShockGamepad touchpadPrimary];
            return (__bridge void*) touchpadPrimary;
        }
        @catch(NSException* ex)
        {
            *exceptionPtr = (__bridge_retained void*) ex;
        }
    }
    
    return nil;
}


void* GCDualShockGamepad_GetPropTouchpadSecondary(const void* ptr, const void** exceptionPtr)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        @try
        {
            GCDualShockGamepad* iGCDualShockGamepad = (__bridge GCDualShockGamepad*) ptr;
            GCControllerDirectionPad* touchpadSecondary = [iGCDualShockGamepad touchpadSecondary];
            return (__bridge void*) touchpadSecondary;
        }
        @catch(NSException* ex)
        {
            *exceptionPtr = (__bridge_retained void*) ex;
        }
    }
    
    return nil;
}




void GCDualShockGamepad_Dispose(void* ptr)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        GCDualShockGamepad* val = (__bridge GCDualShockGamepad*) ptr;
        if(val != nil)
        {
            CFRelease(ptr);
        }
        //NSLog(@"Dispose...GCDualShockGamepad");
    }
}

}
