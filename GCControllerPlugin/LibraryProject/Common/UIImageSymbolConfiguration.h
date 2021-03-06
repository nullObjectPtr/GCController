//
//  UIImageSymbolConfiguration.h
//
//  Created by Jonathan Culp <jonathanculp@gmail.com> on 09/23/2020
//  Copyright © 2021 HovelHouseApps. All rights reserved.
//  Unauthorized copying of this file, via any medium is strictly prohibited
//  Proprietary and confidential
//

#import <Foundation/Foundation.h>
#import "Callbacks.h"

// Class Methods 
extern "C" void* UIImageSymbolConfiguration_configurationWithPointSize_weight(
	CGFloat pointSize,
    long weight,
    void** exception
    );

extern "C" void* UIImageSymbolConfiguration_configurationWithPointSize_weight_scale(
	CGFloat pointSize,
    long weight,
    long scale,
    void** exception
    );

extern "C" void* UIImageSymbolConfiguration_configurationWithScale(
	long scale,
    void** exception
    );

extern "C" void* UIImageSymbolConfiguration_configurationWithTextStyle(
	const char* textStyle,
    void** exception
    );

extern "C" void* UIImageSymbolConfiguration_configurationWithTextStyle_scale(
	const char* textStyle,
    long scale,
    void** exception
    );


// Init Methods 

// Instance methods 

// Void methods 

// Properties 



extern "C" void UIImageSymbolConfiguration_Dispose(void* ptr);
