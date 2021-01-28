//
//  GCControllerCallbacks.cs
//
//  Created by Jonathan Culp <jonathanculp@gmail.com> on 09/24/2020
//  Copyright © 2021 HovelHouseApps. All rights reserved.
//  Unauthorized copying of this file, via any medium is strictly prohibited
//  Proprietary and confidential
//

using System;

namespace HovelHouse.GameController
{
	public delegate void GCExtendedGamepadValueChangedDelegate(
		IntPtr thisPtr,
    	IntPtr gamepad,
		long gamepadTypeToken,
    	IntPtr element,
        long elementTypeToken);
	
	public delegate void GCControllerButtonValueChangedDelegate(
		IntPtr _this,
    	IntPtr button,
    	float value,
    	bool pressed);
    
	public delegate void GCControllerAxisValueChangedDelegate(
		IntPtr _this,
    	IntPtr axis,
    	float value);
    
	public delegate void GCMicroGamepadValueChangedDelegate(
		IntPtr _this,
    	IntPtr gamepad,
    	IntPtr element,
		long elementType);
    
}

