//
//  GCControllerElement.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCControllerElement.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
bool GCControllerElement_SupportsSFSymbols()
{
    return [[GCControllerElement class] instancesRespondToSelector:@selector(sfSymbolsName)];
}

//InitMethods
//InstanceMethods
//VoidMethods
//Properties
const char* GCControllerElement_GetPropLocalizedName(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerElement* iGCControllerElement = (__bridge GCControllerElement*) ptr;
        NSString* localizedName = [iGCControllerElement localizedName];
        return [localizedName UTF8String];
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}

void GCControllerElement_SetPropLocalizedName(const void* ptr, const char* localizedName, const void** exceptionPtr)
{
	@try
	{
		GCControllerElement* iGCControllerElement = (__bridge GCControllerElement*) ptr;
		[iGCControllerElement setLocalizedName:[NSString stringWithUTF8String:localizedName]];
	}
	@catch(NSException* ex) 
	{
		*exceptionPtr = (__bridge_retained void*) ex;
	}
}


const char* GCControllerElement_GetPropSfSymbolsName(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerElement* iGCControllerElement = (__bridge GCControllerElement*) ptr;
        NSString* sfSymbolsName = [iGCControllerElement sfSymbolsName];
        return [sfSymbolsName UTF8String];
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}

void GCControllerElement_SetPropSfSymbolsName(const void* ptr, const char* sfSymbolsName, const void** exceptionPtr)
{
	@try
	{
		GCControllerElement* iGCControllerElement = (__bridge GCControllerElement*) ptr;
		[iGCControllerElement setSfSymbolsName:[NSString stringWithUTF8String:sfSymbolsName]];
	}
	@catch(NSException* ex) 
	{
		*exceptionPtr = (__bridge_retained void*) ex;
	}
}


const char* GCControllerElement_GetPropUnmappedLocalizedName(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerElement* iGCControllerElement = (__bridge GCControllerElement*) ptr;
        NSString* unmappedLocalizedName = [iGCControllerElement unmappedLocalizedName];
        return [unmappedLocalizedName UTF8String];
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}

void GCControllerElement_SetPropUnmappedLocalizedName(const void* ptr, const char* unmappedLocalizedName, const void** exceptionPtr)
{
	@try
	{
		GCControllerElement* iGCControllerElement = (__bridge GCControllerElement*) ptr;
		[iGCControllerElement setUnmappedLocalizedName:[NSString stringWithUTF8String:unmappedLocalizedName]];
	}
	@catch(NSException* ex) 
	{
		*exceptionPtr = (__bridge_retained void*) ex;
	}
}


const char* GCControllerElement_GetPropUnmappedSfSymbolsName(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerElement* iGCControllerElement = (__bridge GCControllerElement*) ptr;
        NSString* unmappedSfSymbolsName = [iGCControllerElement unmappedSfSymbolsName];
        return [unmappedSfSymbolsName UTF8String];
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}

void GCControllerElement_SetPropUnmappedSfSymbolsName(const void* ptr, const char* unmappedSfSymbolsName, const void** exceptionPtr)
{
	@try
	{
		GCControllerElement* iGCControllerElement = (__bridge GCControllerElement*) ptr;
		[iGCControllerElement setUnmappedSfSymbolsName:[NSString stringWithUTF8String:unmappedSfSymbolsName]];
	}
	@catch(NSException* ex) 
	{
		*exceptionPtr = (__bridge_retained void*) ex;
	}
}


bool GCControllerElement_GetPropBoundToSystemGesture(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerElement* iGCControllerElement = (__bridge GCControllerElement*) ptr;
        BOOL boundToSystemGesture = [iGCControllerElement isBoundToSystemGesture];
        return boundToSystemGesture;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return NO;
}


bool GCControllerElement_GetPropAnalog(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerElement* iGCControllerElement = (__bridge GCControllerElement*) ptr;
        BOOL analog = [iGCControllerElement isAnalog];
        return analog;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return NO;
}


void* GCControllerElement_GetPropCollection(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerElement* iGCControllerElement = (__bridge GCControllerElement*) ptr;
        GCControllerElement* collection = [iGCControllerElement collection];
        return (__bridge void*) collection;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}




void GCControllerElement_Dispose(void* ptr)
{
    GCControllerElement* val = (__bridge GCControllerElement*) ptr;
    if(val != nil)
    {
        CFRelease(ptr);
    }
    NSLog(@"Dispose...GCControllerElement");
}

}
