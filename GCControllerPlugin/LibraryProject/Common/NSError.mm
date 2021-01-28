//
//  NSError.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "NSError.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
const char* NSError_stringForUserInfoKey(
    void* ptr,
    const char* key,
    void** exception
    )
{
	@try 
	{
		NSError* iNSError = (__bridge NSError*) ptr;
	    NSString* val = [[iNSError userInfo] objectForKey:[NSString stringWithUTF8String:key]];
	    return [val UTF8String];
	}
	@catch(NSException* ex)
	{
		*exception = (__bridge_retained void*) ex;
	}
	return nil;
}



long NSError_intForUserInfoKey(
    void* ptr,
    const char* key,
    void** exception
    )
{
	@try 
	{
		NSError* iNSError = (__bridge NSError*) ptr;
	    NSNumber* val = [[iNSError userInfo] objectForKey:[NSString stringWithUTF8String:key]];
	    return [val longValue];
	}
	@catch(NSException* ex)
	{
		*exception = (__bridge_retained void*) ex;
	}
	return 0;
}



float NSError_floatForUserInfoKey(
    void* ptr,
    const char* key,
    void** exception
    )
{
	@try 
	{
		NSError* iNSError = (__bridge NSError*) ptr;
	    NSNumber* val = [[iNSError userInfo] objectForKey:[NSString stringWithUTF8String:key]];
	    return [val floatValue];
	}
	@catch(NSException* ex)
	{
		*exception = (__bridge_retained void*) ex;
	}
	return 0;
}



void* NSError_errorForUserInfoKey(
    void* ptr,
    const char* key,
    void** exception
    )
{
	@try 
	{
		NSError* iNSError = (__bridge NSError*) ptr;
	    NSError* val = [[iNSError userInfo] objectForKey:[NSString stringWithUTF8String:key]];
	    return (__bridge_retained void*) val;
	}
	@catch(NSException* ex)
	{
		*exception = (__bridge_retained void*) ex;
	}
	return nil;
}



const char* NSError_userInfoAsString(
    void* ptr,
    void** exception
    )
{
	@try 
	{
		NSError* iNSError = (__bridge NSError*) ptr;
        NSString* val = [NSString stringWithFormat:@"%@", [iNSError userInfo]];
	    return [val UTF8String];
	}
	@catch(NSException* ex)
	{
		*exception = (__bridge_retained void*) ex;
	}
	return nil;
}



//VoidMethods
//Properties
NSInteger NSError_GetPropCode(void* ptr)
{
	NSError* iNSError = (__bridge NSError*) ptr;
	NSInteger val = [iNSError code];
	return val;
}


const char* NSError_GetPropLocalizedDescription(void* ptr)
{
	NSError* iNSError = (__bridge NSError*) ptr;
	NSString* val = [iNSError localizedDescription];
	return [val UTF8String];
}


const char* NSError_GetPropLocalizedRecoverySuggestion(void* ptr)
{
	NSError* iNSError = (__bridge NSError*) ptr;
	NSString* val = [iNSError localizedRecoverySuggestion];
	return [val UTF8String];
}


const char* NSError_GetPropLocalizedFailureReason(void* ptr)
{
	NSError* iNSError = (__bridge NSError*) ptr;
	NSString* val = [iNSError localizedFailureReason];
	return [val UTF8String];
}


const char* NSError_GetPropHelpAnchor(void* ptr)
{
	NSError* iNSError = (__bridge NSError*) ptr;
	NSString* val = [iNSError helpAnchor];
	return [val UTF8String];
}


//TODO: PROPERTYSTRINGARRAY
const char* NSError_GetPropDomain(void* ptr)
{
	NSError* iNSError = (__bridge NSError*) ptr;
	NSString* val = [iNSError domain];
	return [val UTF8String];
}




void NSError_Dispose(void* ptr)
{
    NSError* val = (__bridge NSError*) ptr;
    if(val != nil)
    {
        CFRelease(ptr);
    }
    NSLog(@"Dispose...NSError");
}

}
