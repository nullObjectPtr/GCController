//
//  GCKeyboardInput.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCKeyboardInput.h"
#import "GCControllerConverters.h"



extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties


void GCKeyboardInput_Dispose(void* ptr)
{ 
        GCKeyboardInput* val = (__bridge GCKeyboardInput*) ptr;
        if(val != nil)
        {
            CFRelease(ptr);
        }
        //NSLog(@"Dispose...GCKeyboardInput");
}

}
