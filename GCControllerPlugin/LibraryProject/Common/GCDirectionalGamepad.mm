//
//  GCDirectionalGamepad.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCDirectionalGamepad.h"
#import "GCControllerConverters.h"



extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties


void GCDirectionalGamepad_Dispose(void* ptr)
{ 
        GCDirectionalGamepad* val = (__bridge GCDirectionalGamepad*) ptr;
        if(val != nil)
        {
            CFRelease(ptr);
        }
        //NSLog(@"Dispose...GCDirectionalGamepad");
}

}
