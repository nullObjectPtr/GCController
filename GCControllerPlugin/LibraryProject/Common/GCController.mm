//
//  GCController.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>	
#import "GCController.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
void GCController_controllers(
	const void** bufferPtr, long* const bufferLength, const void** exception
    )
{
	@try
    {
		NSLog(@"GCController_controllers()");
	    NSArray<GCController*>* val = [GCController controllers];
        *bufferPtr = [GCControllerConverters NSArrayToRetainedCArray:val];
        *bufferLength = [val count];
	}
	@catch(NSException* ex)
	{
		*exception = (__bridge_retained void*) ex;
	}
}



void* GCController_controllerWithExtendedGamepad(
	void** exception
    )
{
    if(@available(macOS 10.15, iOS 13, tvOS 13, *))
    {
        @try {
            NSLog(@"GCController_controllerWithExtendedGamepad()");
            GCController* val = [GCController controllerWithExtendedGamepad];
            return (__bridge_retained void*) val;
        }
        @catch(NSException* ex)
        {
            *exception = (__bridge_retained void*) ex;
        }
    }

	return nil;
}



void* GCController_controllerWithMicroGamepad(
	void** exception
    )
{
    if(@available(macOS 10.15, iOS 13, tvOS 13, *))
    {
        @try {
            NSLog(@"GCController_controllerWithMicroGamepad()");
            GCController* val = [GCController controllerWithMicroGamepad];
            return (__bridge_retained void*) val;
        }
        @catch(NSException* ex)
        {
            *exception = (__bridge_retained void*) ex;
        }
    }

	return nil;
}



//InitMethods
//InstanceMethods
void* GCController_capture(
    void* ptr,
    void** exception
    )
{
    if(@available(macOS 10.15, iOS 13, tvOS 13, *))
    {
        @try
        {
            GCController* iGCController = (__bridge GCController*) ptr;
            GCController* val = [iGCController capture];
            return (__bridge_retained void*) val;
        }
        @catch(NSException* ex)
        {
            *exception = (__bridge_retained void*) ex;
        }
    }
    return nil;
}

//VoidMethods
//Properties
void* GCController_GetPropPhysicalInputProfile(const void* ptr, long* const typeToken, const void** exceptionPtr)
{
    if(@available(macOS 11, iOS 14, tvOS 14,*))
    {
        @try
        {
            GCController* iGCController = (__bridge GCController*) ptr;
            id val = [iGCController physicalInputProfile];
            
            if([val isKindOfClass:[GCDualShockGamepad class]])
            {
                *typeToken = 1;
            }
            else if([val isKindOfClass:[GCXboxGamepad class]])
            {
                *typeToken = 2;
            }
            else if([val isKindOfClass:[GCExtendedGamepad class]])
            {
                *typeToken = 4;
            }
            else if([val isKindOfClass:[GCMicroGamepad class]])
            {
                *typeToken = 3;
            }
            // Default case - unspecified profile
            else
            {
                *typeToken = 0;
            }
            
            return (__bridge_retained void*) val;
        }
        @catch(NSException* ex)
        {
            *exceptionPtr = (__bridge_retained void*) ex;
        }
    }
    
    return nil;
}


bool GCController_GetPropAttachedToDevice(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCController* iGCController = (__bridge GCController*) ptr;
        BOOL attachedToDevice = [iGCController isAttachedToDevice];
        return attachedToDevice;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return NO;
}


bool GCController_GetPropSnapshot(const void* ptr, const void** exceptionPtr)
{
    if(@available(macOS 10.15, iOS 13, tvOS 13, *))
    {
        @try
        {
            GCController* iGCController = (__bridge GCController*) ptr;
            BOOL snapshot = [iGCController isSnapshot];
            return snapshot;
        }
        @catch(NSException* ex)
        {
            *exceptionPtr = (__bridge_retained void*) ex;
        }
    }
    
    return NO;
}


void* GCController_GetPropExtendedGamepad(const void* ptr, long* const typeToken, const void** exceptionPtr)
{
    @try
    {
        GCController* iGCController = (__bridge GCController*) ptr;
        id extendedGamepad = [iGCController extendedGamepad];
        
        if(@available(macOS 10.16, iOS 14, tvOS 14, *))
        {
            if([extendedGamepad isKindOfClass:[GCDualShockGamepad class]])
            {
                *typeToken = 1;
            }
            else if([extendedGamepad isKindOfClass:[GCXboxGamepad class]])
            {
                *typeToken = 2;
            }
            else if([extendedGamepad isKindOfClass:[GCExtendedGamepad class]])
            {
                *typeToken = 4;
            }
        }
        else
        {
            if([extendedGamepad isKindOfClass:[GCExtendedGamepad class]])
            {
                *typeToken = 4;
            }
        }
        
        return (__bridge void*) extendedGamepad;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCController_GetPropMicroGamepad(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCController* iGCController = (__bridge GCController*) ptr;
        GCMicroGamepad* microGamepad = [iGCController microGamepad];
        return (__bridge void*) microGamepad;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


const char* GCController_GetPropProductCategory(const void* ptr, const void** exceptionPtr)
{
    if(@available(iOS 13, macOS 10.15, tvOS 13, *))
    {
        @try
        {
            GCController* iGCController = (__bridge GCController*) ptr;
            NSString* productCategory = [iGCController productCategory];
            return [productCategory UTF8String];
        }
        @catch(NSException* ex)
        {
            *exceptionPtr = (__bridge_retained void*) ex;
        }
    }
    
    return nil;
}


const char* GCController_GetPropVendorName(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCController* iGCController = (__bridge GCController*) ptr;
        NSString* vendorName = [iGCController vendorName];
        return [vendorName UTF8String];
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCController_GetPropCurrent(const void** exceptionPtr)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        @try
        {
            GCController* current = [GCController current];
            return (__bridge void*) current;
        }
        @catch(NSException* ex)
        {
            *exceptionPtr = (__bridge_retained void*) ex;
        }
    }
    
    return nil;
}


void GCController_Dispose(void* ptr)
{
    GCController* val = (__bridge GCController*) ptr;
    if(val != nil)
    {
        CFRelease(ptr);
    }
    NSLog(@"Dispose...GCController");
}

}
