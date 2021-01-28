//
//  UITraitCollection.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#if TARGET_OS_IOS
#import <Foundation/Foundation.h>
#import "UITraitCollection.h"
#import "GCControllerConverters.h"


extern "C" {

//ClassMethods
//InitMethods
//InstanceMethods
//VoidMethods
//Properties


void UITraitCollection_Dispose(void* ptr)
{
    UITraitCollection* val = (__bridge UITraitCollection*) ptr;
    if(val != nil)
    {
        CFRelease(ptr);
    }
    NSLog(@"Dispose...UITraitCollection");
}

}
#endif
