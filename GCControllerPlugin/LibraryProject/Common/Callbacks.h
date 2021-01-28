//
//  Callbacks.h
//  PluginCodeGenerator
//
//  Created by Jonathan on 1/8/20.
//  Copyright © 2020 HovelHouseApps. All rights reserved.
//

typedef void (*GCExtendedGamepadValueChangedCallback)(
                                                      const void* controller,
                                                      void* gamepad,
                                                      long gamepadClassType,
                                                      void* element,
                                                      long elementClassType);

typedef void (*GCControllerButtonValueChangedCallback)(const void* ptr, void* button, float value, bool pressed);
typedef void (*GCControllerAxisValueChangedCallback)(const void* ptr, void* axis, float value);
typedef void (*GCMicroGamepadValueChangedCallback)(const void* ptr, void* gamepad, void* element, long elementClassType);
