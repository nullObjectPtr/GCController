//
//  UIImageConfiguration.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#if TARGET_OS_IOS
#import <Foundation/Foundation.h>
#import "UIImageConfiguration.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties


void UIImageConfiguration_Dispose(void* ptr)
{
    UIImageConfiguration* val = (__bridge UIImageConfiguration*) ptr;
    if(val != nil)
    {
        CFRelease(ptr);
    }
    NSLog(@"Dispose...UIImageConfiguration");
}

}
#endif
