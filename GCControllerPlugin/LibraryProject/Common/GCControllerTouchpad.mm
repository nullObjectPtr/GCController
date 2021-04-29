//
//  GCControllerTouchpad.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCControllerTouchpad.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties


void GCControllerTouchpad_Dispose(void* ptr)
{
    if(@available(macOS 10.16, iOS 14, tvOS 14, *))
    {
        GCControllerTouchpad* val = (__bridge GCControllerTouchpad*) ptr;
        if(val != nil)
        {
            CFRelease(ptr);
        }
        //NSLog(@"Dispose...GCControllerTouchpad");
    }
}

}
