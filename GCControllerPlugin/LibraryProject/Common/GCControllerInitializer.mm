//
//  GCControllerInitializer.mm
//
//  Created by Jonathan on 12/31/19.
//  Copyright © 2019 HovelHouseApps. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GCControllerInitializer.h"
#import "GCControllerConverters.h"

GCControllerConnectedCallback KGCController_ControllerConnected;
GCControllerDisconnectedCallback KGCController_ControllerDisconnected;

extern "C"
{
    void RegisterDelegateFor_GCController_ControllerConnected(GCControllerConnectedCallback del)
    {
        NSLog(@"did register GCControllerConnectedCallback");
        KGCController_ControllerConnected = del;
    }

    void RegisterDelegateFor_GCController_ControllerDisconnected(GCControllerDisconnectedCallback del)
    {
        NSLog(@"did register GCControllerDisconnectedCallback");
        KGCController_ControllerDisconnected = del;
    }

    void GCControllerPluignInit()
    {
        [[GCControllerPluginInitializer Instance] RegisterForNotifications];
    }
}

@implementation GCControllerPluginInitializer

+ (id)Instance {
    static GCControllerPluginInitializer *sharedMyManager = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedMyManager = [[self alloc] init];
    });
    return sharedMyManager;
}

-(void)RegisterForNotifications
{
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(ControllerWasConnected:) name:GCControllerDidConnectNotification object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(ControllerWasDisconnected:) name:GCControllerDidDisconnectNotification object:nil];
}

-(void)ControllerWasConnected:(NSNotification*) Notification
{
    NSLog(@"Controller Connected");
    if(KGCController_ControllerConnected != nil)
    {
        void* controllerPtr = (__bridge void*) [Notification object];
        KGCController_ControllerConnected(controllerPtr);
    }
    else
    {
        NSLog(@"Controller Connected Callback is not set from plugin");
    }
}

-(void)ControllerWasDisconnected:(NSNotification*) Notification
{
    NSLog(@"Controller Disconnected");

    if(KGCController_ControllerDisconnected != nil)
    {
        void* controllerPtr = (__bridge void*) [Notification object];
        KGCController_ControllerConnected(controllerPtr);
    }
    else
    {
        NSLog(@"Controller Disconnected Callback is not set from plugin");
    }
}

@end

