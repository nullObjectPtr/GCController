//
//  GCXboxGamepad.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCXboxGamepad.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties
void* GCXboxGamepad_GetPropPaddleButton1(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCXboxGamepad* iGCXboxGamepad = (__bridge GCXboxGamepad*) ptr;
        GCControllerButtonInput* paddleButton1 = [iGCXboxGamepad paddleButton1];
        return (__bridge void*) paddleButton1;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCXboxGamepad_GetPropPaddleButton2(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCXboxGamepad* iGCXboxGamepad = (__bridge GCXboxGamepad*) ptr;
        GCControllerButtonInput* paddleButton2 = [iGCXboxGamepad paddleButton2];
        return (__bridge void*) paddleButton2;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCXboxGamepad_GetPropPaddleButton3(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCXboxGamepad* iGCXboxGamepad = (__bridge GCXboxGamepad*) ptr;
        GCControllerButtonInput* paddleButton3 = [iGCXboxGamepad paddleButton3];
        return (__bridge void*) paddleButton3;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCXboxGamepad_GetPropPaddleButton4(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCXboxGamepad* iGCXboxGamepad = (__bridge GCXboxGamepad*) ptr;
        GCControllerButtonInput* paddleButton4 = [iGCXboxGamepad paddleButton4];
        return (__bridge void*) paddleButton4;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}




void GCXboxGamepad_Dispose(void* ptr)
{
    GCXboxGamepad* val = (__bridge GCXboxGamepad*) ptr;
    if(val != nil)
    {
        CFRelease(ptr);
    }
    NSLog(@"Dispose...GCXboxGamepad");
}

}
