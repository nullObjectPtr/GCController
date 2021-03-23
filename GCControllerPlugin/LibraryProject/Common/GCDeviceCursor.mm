//
//  GCDeviceCursor.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCDeviceCursor.h"
#import "GCControllerConverters.h"



extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties


void GCDeviceCursor_Dispose(void* ptr)
{ 
    if(@available(macOS 11.0, tvOS 14.0, iOS 14.0, * ))
    { 
        GCDeviceCursor* val = (__bridge GCDeviceCursor*) ptr;
        if(val != nil)
        {
            CFRelease(ptr);
        }
        //NSLog(@"Dispose...GCDeviceCursor");
    } 
}

}
