//
//  NSException.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "NSException.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties
const char* NSException_GetPropName(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        NSException* iNSException = (__bridge NSException*) ptr;
        NSExceptionName name = [iNSException name];
        return [name UTF8String];
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


const char* NSException_GetPropReason(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        NSException* iNSException = (__bridge NSException*) ptr;
        NSString* reason = [iNSException reason];
        return [reason UTF8String];
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return nil;
}


//TODO: PROPERTYSTRINGARRAY


void NSException_Dispose(void* ptr)
{
    NSException* val = (__bridge NSException*) ptr;
    if(val != nil)
    {
        CFRelease(ptr);
    }
    NSLog(@"Dispose...NSException");
}

}
