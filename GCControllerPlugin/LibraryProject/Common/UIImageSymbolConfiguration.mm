//
//  UIImageSymbolConfiguration.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "UIImageSymbolConfiguration.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
void* UIImageSymbolConfiguration_configurationWithPointSize_weight(
	CGFloat pointSize, 
	long weight, 
	void** exception
    )
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        @try {
    #if TARGET_OS_IOS || TARGET_OS_TV
            id val = [UIImageSymbolConfiguration configurationWithPointSize:pointSize weight:(UIImageSymbolWeight)weight];
    #else
            id val = [NSImageSymbolConfiguration configurationWithPointSize:pointSize weight:(NSFontWeight)weight];
    #endif
            
            return (__bridge_retained void*) val;
        }
        @catch(NSException* ex)
        {
            *exception = (__bridge_retained void*) ex;
        }
    }

	return nil;
}



void* UIImageSymbolConfiguration_configurationWithPointSize_weight_scale(
	CGFloat pointSize, 
	long weight, 
	long scale, 
	void** exception
    )
{
    if(@available(macOS 10.16, iOS 13, tvOS 13, *))
    {
        @try {
            
    #if TARGET_OS_IOS || TARGET_OS_TV
            id val = [UIImageSymbolConfiguration configurationWithPointSize:pointSize weight:(UIImageSymbolWeight)weight scale:(UIImageSymbolScale)scale];
    #else
            id val = [NSImageSymbolConfiguration configurationWithPointSize:pointSize weight:(NSFontWeight)weight scale:(NSImageSymbolScale)scale];
    #endif
            return (__bridge_retained void*) val;
        }
        @catch(NSException* ex)
        {
            *exception = (__bridge_retained void*) ex;
        }
    }

	return nil;
}



void* UIImageSymbolConfiguration_configurationWithScale(
	long scale, 
	void** exception
    )
{
    if(@available(macOS 10.16, iOS 13, tvOS 13, *))
    {
        @try {
    #if TARGET_OS_IOS || TARGET_OS_TV
            id val = [UIImageSymbolConfiguration configurationWithScale:(UIImageSymbolScale)scale];
    #else
            id val = [NSImageSymbolConfiguration configurationWithScale:(NSImageSymbolScale)scale];
    #endif
            return (__bridge_retained void*) val;
        }
        @catch(NSException* ex)
        {
            *exception = (__bridge_retained void*) ex;
        }
    }

	return nil;
}



void* UIImageSymbolConfiguration_configurationWithTextStyle(
	const char* textStyle, 
	void** exception
    )
{
    if(@available(macOS 10.16, iOS 13, tvOS 13, *))
    {
        @try {
    #if TARGET_OS_IOS || TARGET_OS_TV
            id val = [UIImageSymbolConfiguration configurationWithTextStyle:[NSString stringWithUTF8String:textStyle]];
    #else
            id val = [NSImageSymbolConfiguration configurationWithTextStyle:[NSString stringWithUTF8String:textStyle]];
    #endif
            return (__bridge_retained void*) val;
        }
        @catch(NSException* ex)
        {
            *exception = (__bridge_retained void*) ex;
        }
    }

	return nil;
}



void* UIImageSymbolConfiguration_configurationWithTextStyle_scale(
	const char* textStyle, 
	long scale, 
	void** exception
    )
{
    if(@available(macOS 10.16, iOS 13, tvOS 13, *))
    {
        @try {
            NSLog(@"UIImageSymbolConfiguration_configurationWithTextStyle_scale()");
            
    #if TARGET_OS_IOS || TARGET_OS_TV
            id val = [UIImageSymbolConfiguration configurationWithTextStyle:[NSString stringWithUTF8String:textStyle] scale:(UIImageSymbolScale)scale];
    #else
            id val = [NSImageSymbolConfiguration configurationWithTextStyle:[NSString stringWithUTF8String:textStyle] scale:(NSImageSymbolScale)scale];
    #endif
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
//VoidMethods
//Properties


void UIImageSymbolConfiguration_Dispose(void* ptr)
{
    if(@available(macOS 10.16, iOS 13, tvOS 13, *))
    {
    #if TARGET_OS_IOS || TARGET_OS_TV
        id val = (__bridge UIImageSymbolConfiguration*) ptr;
    #else
        id val = (__bridge NSImageSymbolConfiguration*) ptr;
    #endif
        if(val != nil)
        {
            CFRelease(ptr);
        }
        //NSLog(@"Dispose...UIImageSymbolConfiguration");
    }
}

}
