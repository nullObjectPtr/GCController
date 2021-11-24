//
//  GCControllerInitializer.h
//
//  Created by Jonathan Culp <jonathanculp@gmail.com> on 09/23/2020
//  Copyright © 2021 HovelHouseApps. All rights reserved.
//  Unauthorized copying of this file, via any medium is strictly prohibited
//  Proprietary and confidential
//

#import <Foundation/Foundation.h>

typedef void (*GCControllerConnectedCallback)(void* ptr);
typedef void (*GCControllerDisconnectedCallback)(void* ptr);

extern GCControllerConnectedCallback KGCController_ControllerConnected;
extern GCControllerDisconnectedCallback KGCController_ControllerDisconnected;

@interface GCControllerPluginInitializer : NSObject
+(id) Instance;
-(void) RegisterForNotifications;
-(void) ControllerWasConnected:(NSNotification*)Notification;
-(void) ControllerWasDisconnected:(NSNotification*)Notification;
@end
