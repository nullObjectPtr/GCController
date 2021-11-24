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

int GCController_GetTypeTokenForPhysicalProfile(id physicalProfile)
{
    NSLog(@"GetTypeToken");
    
    if(physicalProfile == nil)
    {
        return 0;
    }
    
    // Since some of these profiles inherit from the others, it's important to try to detect these in
    // child-most order in terms of inheritence...
    
    // GCDualSenseGamepad   = 1
    // GCDualShockGamepad   = 2
    // GCXBoxGamepad        = 3
    // GCExtendedGamepad    = 4
    
    // GCDirectionalGamepad = 5
    // GCMicroGamepad       = 6
    
    // GCKeyboardInput      = 7
    // GCMouseInput         = 8
    // GCMotion             = 9
    
    // GCPhysicalInputProfile = 10;
    
    // Dual Sense
    if(@available(iOS 14.5, macOS 11.3, macCatalyst 14.5, tvOS 14.5, *)) // iPadOS 14.5+,
    {
        //NSLog(@"Checking dual sense");
        if([physicalProfile isKindOfClass:[GCDualSenseGamepad class]])
        {
            return 1;
        }
    }
    
    // Dual Shock
    if(@available(iOS 14.0, macOS 10.16, macCatalyst 14.0, tvOS 14.0, *)) // iPadOS 14.0,
    {
        //NSLog(@"Checking Dual Shock");
        if([physicalProfile isKindOfClass:[GCDualShockGamepad class]])
        {
            return 2;
        }
    }
    
    // XBox
    if(@available(iOS 14.0, macOS 10.16, macCatalyst 14.0, tvOS 14.0, *)) // iPadOS 14.0,
    {
        //NSLog(@"Checking XBox");
        if([physicalProfile isKindOfClass:[GCXboxGamepad class]])
        {
            return 3;
        }
    }
       
    // Check for GCExtendedGamepad
    if(@available(macOS 10.9, macCatalyst 13.0, iOS 7.0, tvOS 9.0, *)) // iPadOS 7.0
    {
        //NSLog(@"Checking ExtendedGamepad");
        if([physicalProfile isKindOfClass:[GCExtendedGamepad class]])
        {
           return 4;
        }
    }
    
    // Directional gamepad
    if(@available(macOS 11.1, macCatalyst 14.3, iOS 14.3, tvOS 14.3, *))
    {
        //NSLog(@"Checking Directional Gamepad");
        if([physicalProfile isKindOfClass:[GCDirectionalGamepad class]])
        {
            return 5;
        }
    }
    
    // Micro Gamepad
    if(@available(iOS 9.0, macOS 10.11, macCatalyst 13.0, tvOS 9.0, *)) // iPadOS 9.0,
    {
        //NSLog(@"Checking micro gamepad");
        if([physicalProfile isKindOfClass:[GCMicroGamepad class]])
        {
            return 6;
        }
    }

    
    // Is just a plain old PhysicalInputProfile?
    // Strangely, can't check this if before 14.0.... even though the GCExetendedGamepad inherits from it
    // and is available as early as iOS7
    // Seems like a contradiction in the apple docs
    // Not sure what to make of it
    if(@available(macOS 10.16, macCatalyst 14.0, iOS 14.0, tvOS 14.0, *))
    {
        //NSLog(@"Checking physical input profile");
        if([physicalProfile isKindOfClass:[GCPhysicalInputProfile class]])
        {
            return 7;
        }
    }
    
    // Keyboard
    if(@available(iOS 14.0, macOS 10.16, macCatalyst 14.0, tvOS 14.0, *))
    {
        //NSLog(@"Checking keyboard");
        if([physicalProfile isKindOfClass:[GCKeyboardInput class]])
        {
            return 8;
        }
    }
    
    // Mouse
    if(@available(iOS 14.0, macOS 10.16, macCatalyst 14.0, tvOS 14.0, *))
    {
        //NSLog(@"checking mouse");
        if([physicalProfile isKindOfClass:[GCMouseInput class]])
        {
            return 9;
        }
    }
    
    // GCMotion - odd duck, it's a profile but it doesn't extend from physicalInputProfile
    // extends from NSObject, not even sure how you'd get a pointer to it from GCController
    if(@available(iOS 8.0, macOS 10.10, macCatalyst 13.0, tvOS 9.0, *)) //  iPadOS 8.0,
    {
        //NSLog(@"checking motion");
        if([physicalProfile isKindOfClass:[GCMotion class]])
        {
            return 10;
        }
    }
    
    // Unknown type...shouldn't happen
    NSLog(@"Error - unknown profile type detected");
    return -1;
}

//ClassMethods
void GCController_controllers(
	const void** bufferPtr, long* const bufferLength, const void** exception
    )
{
	@try
    {
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
    if(@available(macOS 10.16, iOS 14, tvOS 14.0, *))
    {
        @try
        {
            GCController* iGCController = (__bridge GCController*) ptr;
            id val = [iGCController physicalInputProfile];
            *typeToken = GCController_GetTypeTokenForPhysicalProfile(val);
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
        *typeToken = GCController_GetTypeTokenForPhysicalProfile(extendedGamepad);
        return (__bridge void*) extendedGamepad;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


void* GCController_GetPropMicroGamepad(const void* ptr, long* const typeToken, const void** exceptionPtr)
{
    @try
    {
        GCController* iGCController = (__bridge GCController*) ptr;
        id microGamepad = [iGCController microGamepad];
        *typeToken = GCController_GetTypeTokenForPhysicalProfile(microGamepad);
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
    //NSLog(@"Dispose...GCController");
}

}
