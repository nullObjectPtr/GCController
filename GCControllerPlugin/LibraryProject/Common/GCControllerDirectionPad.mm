//
//  GCControllerDirectionPad.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCControllerDirectionPad.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties
void* GCControllerDirectionPad_GetPropUp(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerDirectionPad* iGCControllerDirectionPad = (__bridge GCControllerDirectionPad*) ptr;
        GCControllerButtonInput* up = [iGCControllerDirectionPad up];
        return (__bridge void*) up;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCControllerDirectionPad_GetPropDown(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerDirectionPad* iGCControllerDirectionPad = (__bridge GCControllerDirectionPad*) ptr;
        GCControllerButtonInput* down = [iGCControllerDirectionPad down];
        return (__bridge void*) down;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCControllerDirectionPad_GetPropLeft(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerDirectionPad* iGCControllerDirectionPad = (__bridge GCControllerDirectionPad*) ptr;
        GCControllerButtonInput* left = [iGCControllerDirectionPad left];
        return (__bridge void*) left;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCControllerDirectionPad_GetPropRight(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerDirectionPad* iGCControllerDirectionPad = (__bridge GCControllerDirectionPad*) ptr;
        GCControllerButtonInput* right = [iGCControllerDirectionPad right];
        return (__bridge void*) right;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCControllerDirectionPad_GetPropXAxis(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerDirectionPad* iGCControllerDirectionPad = (__bridge GCControllerDirectionPad*) ptr;
        GCControllerAxisInput* xAxis = [iGCControllerDirectionPad xAxis];
        return (__bridge void*) xAxis;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCControllerDirectionPad_GetPropYAxis(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerDirectionPad* iGCControllerDirectionPad = (__bridge GCControllerDirectionPad*) ptr;
        GCControllerAxisInput* yAxis = [iGCControllerDirectionPad yAxis];
        return (__bridge void*) yAxis;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}




void GCControllerDirectionPad_Dispose(void* ptr)
{
    GCControllerDirectionPad* val = (__bridge GCControllerDirectionPad*) ptr;
    if(val != nil)
    {
        CFRelease(ptr);
    }
    NSLog(@"Dispose...GCControllerDirectionPad");
}

}
