//
//  UIImageSymbolScale.cs
//
//  Created by Jonathan Culp <jonathanculp@gmail.com> on 09/24/2020
//  Copyright © 2021 HovelHouseApps. All rights reserved.
//  Unauthorized copying of this file, via any medium is strictly prohibited
//  Proprietary and confidential
//

namespace HovelHouse.GameController
{
    public enum UIImageSymbolScale : long
    {
        // Default and unspecificed are only supported on iOS
        // Rather than have the code split here, just conforming this
        // to what will also work on MacOS
        //Default = -1,
        //Unspecified = 0,
        Small = 1,
        Medium = 2,
        Large = 3
    }
}
