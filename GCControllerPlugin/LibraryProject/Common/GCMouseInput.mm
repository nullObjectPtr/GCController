//
//  GCMouseInput.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCMouseInput.h"
#import "GCControllerConverters.h"



extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties


void GCMouseInput_Dispose(void* ptr)
{ 
        GCMouseInput* val = (__bridge GCMouseInput*) ptr;
        if(val != nil)
        {
            CFRelease(ptr);
        }
        //NSLog(@"Dispose...GCMouseInput");
}

}
