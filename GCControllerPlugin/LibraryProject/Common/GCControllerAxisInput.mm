//
//  GCControllerAxisInput.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCControllerAxisInput.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties
float GCControllerAxisInput_GetPropValue(const void* ptr, const void** exceptionPtr)
{
    @try
    {
        GCControllerAxisInput* iGCControllerAxisInput = (__bridge GCControllerAxisInput*) ptr;
        float value = [iGCControllerAxisInput value];
        return value;
    }
    @catch(NSException* ex)
    {
        *exceptionPtr = (__bridge_retained void*) ex;
    }
    
    return 0;
}


void GCControllerAxisInput_SetPropValueChangedHandler(const void* ptr, GCControllerAxisValueChangedCallback valueChangedHandler, const void** exceptionPtr)
{
	@try 
	{
		id iGCControllerAxisInput = (__bridge GCControllerAxisInput*) ptr;
		[iGCControllerAxisInput setValueChangedHandler:^(GCControllerAxisInput* axis,
	float value)
			{
				
				valueChangedHandler(ptr, (__bridge_retained void*) axis, value);
				
			}];
	}
	@catch(NSException* ex)
	{
		*exceptionPtr = (__bridge_retained void*)ex;
	}
}



void GCControllerAxisInput_Dispose(void* ptr)
{
    GCControllerAxisInput* val = (__bridge GCControllerAxisInput*) ptr;
    if(val != nil)
    {
        CFRelease(ptr);
    }
    NSLog(@"Dispose...GCControllerAxisInput");
}

}
